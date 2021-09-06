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
using Ionic.Zip;
using System.Text.RegularExpressions;
namespace aabViewer
{
   

    public partial class Form1 : Form
    {
        public List<ConfigNode> configNodes = new List<ConfigNode>();
        public string logPath = "";
        public string lastParse;
        public string jarPath;
        public const string verion = "v2.5";
        public Form1(string [] args)
        {
            InitializeComponent();

            Init();

            if (CheckEnvironment(false) && args.Length>0)
            {
                text_aab_path.Text = args[0];
                ExecAabCheck();
            }
        }

        private void Init()
        {
            string str = GetCurrentPath();

            string configPath = str + "Config/data.ini";
            logPath = str + "log.txt";
            jarPath = str + "bundletool-all-1.8.0.jar";

            if (File.Exists(configPath))
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

            this.AllowDrop = true;



            this.dataGridView1.DragEnter += this.Form1_DragEnter;
            this.flowLayoutPanel2.DragEnter += this.Form1_DragEnter;
            this.DragEnter += this.Form1_DragEnter;


            this.dataGridView1.DragDrop += this.Form1_DragDrop;
            this.flowLayoutPanel2.DragDrop += this.Form1_DragDrop;
            this.DragDrop += this.Form1_DragDrop;
        }

        private void ExecAabCheck()
        {
            if(!File.Exists(text_aab_path.Text))
            {
                MessageBox.Show("文件不存在!");
                return;
            }

            string error = "";
            string filePath = text_aab_path.Text;
            string cmd = string.Format("java -jar \"{0}\" dump manifest --bundle \"{1}\"", jarPath, filePath);
            string xmlData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + CmdTools.Exec(cmd, ref error);

            

            Console.WriteLine(cmd);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlData);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            XmlNamespaceManager nsp = new XmlNamespaceManager(doc.NameTable);
            nsp.AddNamespace("android", "http://schemas.android.com/apk/res/android");


            for(int i=0; i<configNodes.Count; i++)
            {
                ConfigNode tmp = configNodes[i];
                if(tmp.filter_name=="")
                {
                    XmlNode t = doc.SelectSingleNode(tmp.path, nsp);
                    string text = t != null ? t.Value : "不存在";

                    if(text.StartsWith("@string/"))
                    {
                        var s_value = GetStringValue(text, filePath);
                        this.dataGridView1.Rows[i].Cells[1].Value = s_value;
                    }
                    else
                    {
                        this.dataGridView1.Rows[i].Cells[1].Value = text;
                    }
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

            using (ZipFile zip = ZipFile.Read(text_aab_path.Text))
            {
                MemoryStream outputStream = new MemoryStream();
                ZipEntry e = zip["base/res/mipmap-xxxhdpi-v4/app_icon.png"];
                e.Extract(outputStream);

                Image img = Image.FromStream(outputStream);
                this.pictureBox1.BackgroundImage = img;
                this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            }

        }

        private string GetStringValue(string _key, string _abbPath)
        {
            string t_key = _key.Replace("@", "");
            string _cmd_string_name = string.Format("java -jar \"{0}\" dump resources --bundle \"{1}\" --resource={2} --values true", jarPath,  _abbPath, t_key);
            var ret = CmdTools.Exec(_cmd_string_name);

            foreach (Match match in Regex.Matches(ret, "\"([^\"]*)\""))
                ret = match.ToString();

            return ret.Replace("\"", "");
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
            string outPath = GetCurrentPath() + "Temp/";

            if (!File.Exists(text_aab_path.Text))
            {
                MessageBox.Show("文件不存在!");
                return;
            }
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }


            outPath += "temp.apks";
            if (File.Exists(outPath))
            {
                File.Delete(outPath);
            }

            // {0} aab路径 {1}输出路径 {2}keystroe路径 {3}秘钥密码 {4}秘钥别名 {5}秘钥别名密码 {6}配置文件路径
            //string cmd = "java - jar bundletool-all-1.8.0.jar build - apks--bundle ={0} --output ={1} --ks ={2} --ks - pass = pass:{3} --ks - key - alias ={4} --key - pass = pass:{5} --device - spec ={6}";


            string cmd = create_install_cmd(outPath);

            //执行指令
            string error = "";
            string ret = CmdTools.Exec(cmd, ref error);
            Console.WriteLine(cmd);

            if (!File.Exists(outPath))
            {
                WriteLog("Create Error: " + error);
                WriteLog("Create Ret: " + ret);
                
                MessageBox.Show("生成失败!");
                return;
            }
            else
            {
                if (ret.Length > 0) MessageBox.Show("Info:" + ret);
                if(error.Length>0) MessageBox.Show("Info:" + error);
            }

            //安装指令
            error = "";
            cmd = string.Format("java -jar \"{0}\" install-apks --apks=\"{1}\"", jarPath, outPath);
            ret = CmdTools.Exec(cmd, ref error);

           
            if(ret.Length==0)
            {
                MessageBox.Show("安装成功!");
            }
            else
            {
                WriteLog("Install Error: " + error);
                WriteLog("Install Ret: " + ret);

                MessageBox.Show("安装失败!");
            }
        }

        private string create_install_cmd(string outPath)
        {
            string _key_path = text_key_path.Text;
            string _alias_pass = text_pass.Text;
            string _key_alias = text_alias.Text;
            string _key_pass = text_key_pass.Text;

            if (!File.Exists(_key_path))
            {
                _key_path = GetCurrentPath() + "Config/debug.keystore";
                _alias_pass = "android";
                _key_pass = "android";
                _key_alias = "androiddebugkey";
            }

            {
                //根据连接手机的设备创建apks的指令
                string cmd = "java -jar \"{0}\" build-apks --bundle=\"{1}\" --output=\"{2}\" --ks=\"{3}\" --ks-pass=pass:{4} --ks-key-alias={5} --key-pass=pass:{6} --connected-device";
                //填充志林该参数
                cmd = string.Format(cmd, jarPath, text_aab_path.Text, outPath, _key_path, _alias_pass, _key_alias, _key_pass);
                return cmd;
            }
        }

        private void btn_base_hash_Click(object sender, EventArgs e)
        {
            string openssl = GetCurrentPath() + "openssl.exe";            
            string cmd = "keytool -exportcert -alias {0} -storepass {1} -keypass {2} -keystore \"{3}\" | \"{4}\" sha1 -binary | \"{4}\" base64";            
            cmd = string.Format(cmd, text_alias.Text, text_pass.Text, text_key_pass.Text, text_key_path.Text, openssl);
            text_hash_result.Text = CmdTools.Exec(cmd);
        }

        private void btn_sha1_Click(object sender, EventArgs e)
        {
            string cmd = "keytool -list -alias {0} -storepass {1} -keypass {2} -keystore \"{3}\"";
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

       

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            
            if(path.Contains(".aab"))
            {
                text_aab_path.Text = path;
                ExecAabCheck();
            }
            else if (path.Contains(".jks") || path.Contains(".keystore"))
            {
                text_key_path.Text = path;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }

        private void WriteLog(string _txt)
        {
            Console.WriteLine(_txt);
            File.AppendAllText(logPath, "\r\n" + GetTime() + _txt);
        }

        private string GetTime()
        {
            return System.DateTime.Now.ToString()+" ";
        }

        private string GetCurrentPath()
        {
            string str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace("aabViewer.exe", "");
            return str;
        }

        private void 环境监测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CheckEnvironment(true))
            {
                MessageBox.Show("运行环境完整!");
            }
        }

        private bool CheckEnvironment(bool isForce=false)
        {
            string doneFile = GetCurrentPath() + "Config/init.txt";
            if(File.Exists(doneFile) && !isForce)
            {
                return true;
            }
            string ret = "";
            string openssl = GetCurrentPath() + "openssl.exe";
            if(!File.Exists(openssl))
            {
                ret += "\r\n缺少OpenSSL";
            }
            string error = "";
            string java = CmdTools.Exec("java -version", ref error);
            if(!error.Contains("Java(TM) SE Runtime Environment"))
            {
                ret += "\r\n缺少Java环境";
                WriteLog("Java：" + java + error);
            }
            if (!File.Exists(jarPath))
            {
                ret += "\r\n缺少 bundletool-all-1.8.0.jar";
            }

            if(ret!="")
            {
                MessageBox.Show("运行环境监测失败:"+ret);
                return false;
            }
            else
            {
                File.WriteAllText(GetCurrentPath() + "Config/init.txt", "Done");
                return true;
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AbbViewer "+ verion);
        }

        private void 配置说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrowserHelper.OpenDefaultBrowserUrl("https://github.com/corle-bell/aabViewer");
        }
    }

    public class ConfigNode
    {
        public string name;
        public string path;
        public string filter_name;
    }
}
