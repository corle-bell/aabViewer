using System;
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
        public List<ConfigNode> configNodes = new List<ConfigNode>();
        public Form1(string [] args)
        {
            InitializeComponent();

            Init();

            if (args.Length>0)
            {
                text_aab_path.Text = args[0];
                ExecAabCheck();
            }
            
        }

        private void Init()
        {
            string configPath = Environment.CurrentDirectory.ToString() + "/Config/data.ini";

            if(File.Exists(configPath))
            {
                string[] data = File.ReadAllLines(configPath);

                configNodes = new List<ConfigNode>();

                int index = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    string[] _split_data = data[i].Split(new char[] { '#' });
                    ConfigNode tmp = new ConfigNode();
                    tmp.name = _split_data[0];
                    tmp.path = _split_data[1];
                    tmp.filter_name = _split_data[2];
                    configNodes.Add(tmp);

                    index = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Cells[0].Value = tmp.name;
                }
            }
            else
            {
                MessageBox.Show("配置文件丢失，请检查配置文件!");
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


            for(int i=0; i<configNodes.Count; i++)
            {
                ConfigNode tmp = configNodes[i];
                if(tmp.filter_name=="")
                {
                    XmlNode t = doc.SelectSingleNode(tmp.path, nsp);
                    string text = t != null ? t.Value : "不存在";
                    this.dataGridView1.Rows[i].Cells[1].Value = text;
                }
                else
                {
                    string text = "none";
                    XmlNodeList nodes = doc.SelectNodes(tmp.path, nsp);
                    foreach (XmlNode item in nodes)
                    {
                        string title = item.SelectSingleNode("@android:name", nsp).Value;
                        if (title == tmp.filter_name)
                        {
                            text = item.SelectSingleNode("@android:value", nsp).Value;
                        }
                    }
                    this.dataGridView1.Rows[i].Cells[1].Value = text;
                }
            }


            
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

    public class ConfigNode
    {
        public string name;
        public string path;
        public string filter_name;
    }
}
