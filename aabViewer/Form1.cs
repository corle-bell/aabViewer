﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Xml;
using System.IO;

namespace aabViewer
{
    public partial class Form1 : Form
    {
        public Form1(string [] args)
        {
            InitializeComponent();

            if(args.Length>0)
            {
                text_aab_path.Text = args[0];
                ExecAabCheck();
            }
        }

        private void ExecAabCheck()
        {
            if(!File.Exists(text_aab_path.Text))
            {
                MessageBox.Show("文件不存在!");
                return;
            }
          
            string filePath = text_aab_path.Text;
            string cmd = string.Format("java -jar bundletool-all-1.8.0.jar dump manifest --bundle  {0}", filePath);
            string xmlData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + CmdTools.Exec(cmd);

            Console.WriteLine(xmlData);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);

            XmlNamespaceManager nsp = new XmlNamespaceManager(doc.NameTable);
            nsp.AddNamespace("android", "http://schemas.android.com/apk/res/android");



            string app_name = doc.SelectSingleNode("/manifest/application/@android:label", nsp).Value;
            string pkg_name = doc.SelectSingleNode("/manifest/@package", nsp).Value;
            string version_code = doc.SelectSingleNode("/manifest/@android:versionCode", nsp).Value;
            string version_name = doc.SelectSingleNode("/manifest/@android:versionName", nsp).Value;

            XmlNode t = doc.SelectSingleNode("/manifest/@android:debuggable", nsp);
            string debuggable = t != null ? t.Value : "";

            string min_sdk = doc.SelectSingleNode("/manifest/uses-sdk/@android:minSdkVersion", nsp).Value;
            string build_sdk = doc.SelectSingleNode("/manifest/uses-sdk/@android:targetSdkVersion", nsp).Value;

            string facebookId = "none";
            XmlNodeList nodes = doc.SelectNodes("/manifest/application/meta-data", nsp);
            foreach (XmlNode item in nodes)
            {
                string title = item.SelectSingleNode("@android:name", nsp).Value;
                if (title == "com.facebook.sdk.ApplicationId")
                {
                    facebookId = item.SelectSingleNode("@android:value", nsp).Value;
                    facebookId = facebookId.Replace("fb", "");
                }
            }

            text_app_name.Text = app_name;
            text_pkg_name.Text = pkg_name;
            text_version_code.Text = version_code;
            text_version_name.Text = version_name;
            text_debug.Text = debuggable == "" ? "否" : "是";
            text_min_sdk.Text = min_sdk;
            text_build_sdk.Text = build_sdk;
            text_facebook_id.Text = facebookId;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(Environment.CurrentDirectory.ToString());
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            openFileDialog.Filter = "AAB文件(*.aab)|*.aab;";
            text_aab_path.Text = openFileDialog.FileName;

            ExecAabCheck();
        }

        private void btn_install_Click(object sender, EventArgs e)
        {
            //设置apks输出路径
            string outPath = Environment.CurrentDirectory.ToString() + "/Temp/";

            if (!File.Exists(text_aab_path.Text))
            {
                MessageBox.Show("文件不存在!");
                return;
            }
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }


            // {0} aab路径 {1}输出路径 {2}keystroe路径 {3}秘钥密码 {4}秘钥别名 {5}秘钥别名密码 {6}配置文件路径
            //string cmd = "java - jar bundletool-all-1.8.0.jar build - apks--bundle ={0} --output ={1} --ks ={2} --ks - pass = pass:{3} --ks - key - alias ={4} --key - pass = pass:{5} --device - spec ={6}";

            //根据连接手机的设备创建apks的指令
            string cmd = "java -jar bundletool-all-1.8.0.jar build-apks --bundle={0} --output={1} --ks={2} --ks-pass=pass:{3} --ks-key-alias={4} --key-pass=pass:{5} --connected-device";
            outPath += "temp.apks";


            if (File.Exists(outPath))
            {
                File.Delete(outPath);
            }

            //填充志林该参数
            cmd = string.Format(cmd, text_aab_path.Text, outPath, text_key_path.Text, text_pass.Text, text_alias.Text, text_key_pass.Text);
            //执行指令
            string ret = CmdTools.Exec(cmd);
            Console.WriteLine("生成:"+ret);
            //安装指令
            cmd = string.Format("java -jar bundletool-all-1.8.0.jar install-apks --apks={0}", outPath);
            ret = CmdTools.Exec(cmd);
            Console.WriteLine("安装:" + ret);

            if(ret.Length==0)
            {
                MessageBox.Show("安装成功!");
            }
        }

        private void btn_base_hash_Click(object sender, EventArgs e)
        {
            string cmd = "keytool -exportcert -alias {0} -storepass {1} -keypass {2} -keystore {3} | openssl sha1 -binary | openssl base64";
            cmd = string.Format(cmd, text_alias.Text, text_pass.Text, text_key_pass.Text, text_key_path.Text);
            text_hash_result.Text = CmdTools.Exec(cmd);
        }

        private void btn_sha1_Click(object sender, EventArgs e)
        {
            string cmd = "keytool -list -alias {0} -storepass {1} -keypass {2} -keystore {3}";
            cmd = string.Format(cmd, text_alias.Text, text_pass.Text, text_key_pass.Text, text_key_path.Text);
            text_hash_result.Text = CmdTools.Exec(cmd);
        }

        private void btn_select_key_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            openFileDialog.Filter = "KeyStore文件(*.keystore,*.jks)|*.keystore;*.jks";
            text_key_path.Text = openFileDialog.FileName;
        }
    }
}