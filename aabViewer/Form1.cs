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
using System.Xml.Linq;

namespace aabViewer
{
    

    public partial class Form1 : Form
    {
        public List<ConfigNode> configNodes = new List<ConfigNode>();
        public List<KeyNode> keyNodes = new List<KeyNode>();
        public List<string> SignTemplete = new List<string>();
        public string logPath = "";
        public string lastParse;
        public string jarPath;
        public string keyConfigPath = "";
        public const string verion = "v3.1.1";
        

        public Task installTask;
        public Task parseTask;

        private bool needUpdateApks;

        private Timer InitAab = new Timer();
        public Form1(string [] args)
        {
            this.IsMdiContainer = true;

            InitializeComponent();
            ChangeMidBackStyle();
            Init();

            if (CheckEnvironment(false) && args.Length>0)
            {
                text_aab_path.Text = args[0];   
               

                InitAab.Tick += new EventHandler(Call);
                InitAab.Interval = 500;
                InitAab.Enabled = true;
            }

            GetPhoneInfo();

           
        }

        private void Call(object sender, EventArgs e)
        {
            ExecAabCheck();

            InitAab.Stop();
        }

        private void ChangeMidBackStyle()
        {
            //更改Mdi背景样式
            MdiClient mctMdi = new MdiClient();
            foreach (Control conMid in this.Controls)
            {
                //得到Mdi
                if (conMid.GetType().ToString() == "System.Windows.Forms.MdiClient")
                {
                    mctMdi = (System.Windows.Forms.MdiClient)conMid;
                    //改变背景颜色
                    mctMdi.BackColor = Color.FromArgb(255, 255, 250);
                    break;
                }
            }
        }

        #region KeyConfigs
        private void CreateDefaultKeys()
        {
            AddKeys("测试签名", GetCurrentPath() + "Config/debug.keystore", "android", "androiddebugkey", "android");
            SaveKeyConfigs();
        }

        public bool AddKeys(string _name, string _path, string _pass, string _alias, string _alias_pass)
        {
            for (int i = 0; i < keyNodes.Count; i++)
            {
                if (_alias.Trim().Equals(keyNodes[i].alias.Trim()))
                {
                    return false;
                }
            }

            var key = new KeyNode();
            key.name = _name;
            key.path = _path;
            key.password = _pass;
            key.alias = _alias;
            key.alias_password = _alias_pass;

            keyNodes.Add(key);

            this.comboBox1.Items.Add(key.name);
            return true;
        }

        public void SaveKeyConfigs()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < keyNodes.Count; i++)
            {
                builder.AppendLine(keyNodes[i].Format());
            }
            File.WriteAllText(keyConfigPath, builder.ToString());
        }

        private void LoadKeyConfig()
        {
            string[] data = File.ReadAllLines(keyConfigPath);
            for (int i = 0; i < data.Length; i++)
            {
                var key = new KeyNode().Init(data[i]);
                keyNodes.Add(key);
                this.comboBox1.Items.Add(key.name);
            }

            this.comboBox1.SelectedIndex = 0;
        }

        private void LoadSignConfig(string path)
        {
            string[] data = File.ReadAllLines(path);
            string[] arr = data[0].Split(new char[] { '#' });
            SignTemplete.AddRange(arr);
        }
        #endregion

        private void Init()
        {
            string str = GetCurrentPath();

            string configPath = str + "Config/data.ini";
            logPath = str + "log.txt";
            jarPath = str + "bundletool-all-1.8.0.jar";
            keyConfigPath = str+ "Config/keys.ini";
            string signConfigPath = str + "Config/sign.ini";

            if(File.Exists(signConfigPath))
            {
                LoadSignConfig(signConfigPath);
            }

            if (!File.Exists(keyConfigPath))
            {
                CreateDefaultKeys();
            }
            else
            {
                LoadKeyConfig();
            }

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
            string xmlData = "";

            LoadingForm.ShowLoading(this);

            TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
            parseTask = Task.Run(() => {

                LoadingForm.PerformStep("开始解析数据~~~");

                xmlData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + CmdTools.Exec(cmd, ref error);


                LoadingForm.PerformStep("获取ICON ~~~");

                using (ZipFile zip = ZipFile.Read(text_aab_path.Text))
                {
                    MemoryStream outputStream = new MemoryStream();
                    ZipEntry e = zip["base/res/mipmap-xxxhdpi-v4/app_icon.png"];
                    e.Extract(outputStream);

                    Image img = Image.FromStream(outputStream);

                    pictureBox1.Invoke(new Action(() => pictureBox1.BackgroundImage = img));
                    pictureBox1.Invoke(new Action(() => pictureBox1.BackgroundImageLayout = ImageLayout.Stretch));

                }

                parseTask = null;
            }).ContinueWith(m =>
            {
                LoadingForm.HideLoading();

                if (error.Length > 0)
                {
                    WriteLog(error);
                }

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(xmlData);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }

                XmlNamespaceManager nsp = new XmlNamespaceManager(doc.NameTable);
                nsp.AddNamespace("android", "http://schemas.android.com/apk/res/android");


                for (int i = 0; i < configNodes.Count; i++)
                {
                    ConfigNode tmp = configNodes[i];
                    if (tmp.filter_name == "")
                    {
                        XmlNode t = doc.SelectSingleNode(tmp.path, nsp);
                        string text = t != null ? t.Value : "不存在";

                        if (text.StartsWith("@string/"))
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

                
            }, ui);


            needUpdateApks = true;


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
            if(installTask==null)
            {
                LoadingForm.ShowLoading(this);

                TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
                installTask = Task.Run(()=> { 
                    InstallApk(true);
                    installTask = null;
                }).ContinueWith(m =>
                {
                    LoadingForm.HideLoading();
                }, ui);

            }
        }

        private void btn_install_part_Click(object sender, EventArgs e)
        {
            if (installTask == null)
            {
                LoadingForm.ShowLoading(this);

                TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
                installTask = Task.Run(() => {
                    InstallApk(false);
                    installTask = null;
                }).ContinueWith(m =>
                {
                    LoadingForm.HideLoading();
                }, ui);
            }
        }

        #region ApkInstall
        private void InstallApk(bool createForCnnectDevices)
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
            if (File.Exists(outPath) && needUpdateApks)
            {
                File.Delete(outPath);
            }

            // {0} aab路径 {1}输出路径 {2}keystroe路径 {3}秘钥密码 {4}秘钥别名 {5}秘钥别名密码 {6}配置文件路径
            //string cmd = "java - jar bundletool-all-1.8.0.jar build - apks--bundle ={0} --output ={1} --ks ={2} --ks - pass = pass:{3} --ks - key - alias ={4} --key - pass = pass:{5} --device - spec ={6}";

            string cmd = "";
            string error = "";
            string ret = "";
            if (needUpdateApks)
            {
                LoadingForm.PerformStep("正在生成apks~~~~~");
                cmd = create_install_cmd(outPath, createForCnnectDevices);

                //执行指令
                error = "";
                ret = CmdTools.Exec(cmd, ref error);
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
                    if (error.Length > 0) MessageBox.Show("Info:" + error);
                }

                needUpdateApks = false;
            }
            

            LoadingForm.PerformStep("开始进行安装~~~~~");

            //安装指令
            error = "";
            cmd = string.Format("java -jar \"{0}\" install-apks --apks=\"{1}\"", jarPath, outPath);
            ret = CmdTools.Exec(cmd, ref error);


            if (ret.Length == 0)
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

        private string create_install_cmd(string outPath, bool createForCnnectDevices=true)
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
                string cmd = "java -jar \"{0}\" build-apks --bundle=\"{1}\" --output=\"{2}\" --ks=\"{3}\" --ks-pass=pass:{4} --ks-key-alias={5} --key-pass=pass:{6} {7}";                
                //填充志林该参数
                cmd = string.Format(cmd, jarPath, text_aab_path.Text, outPath, _key_path, _alias_pass, _key_alias, _key_pass, createForCnnectDevices?"--connected-device":"--mode=universal");
                return cmd;
            }
        }
        #endregion 

        private void btn_base_hash_Click(object sender, EventArgs e)
        {
            string openssl = GetCurrentPath() + "openssl.exe";
            openssl = "openssl";
            string cmd = "keytool -exportcert -alias {0} -storepass {1} -keypass {2} -keystore \"{3}\" | {4} sha1 -binary | {4} base64";            
            cmd = string.Format(cmd, text_alias.Text, text_pass.Text, text_key_pass.Text, text_key_path.Text, openssl);
            text_hash_result.Text = CmdTools.Exec(cmd);
        }

        private void btn_sha1_Click(object sender, EventArgs e)
        {
            string cmd = "keytool -list -alias {0} -storepass {1} -keypass {2} -keystore \"{3}\"";
            cmd = string.Format(cmd, text_alias.Text, text_pass.Text, text_key_pass.Text, text_key_path.Text);
            text_hash_result.Text = FormatForKeyWord(CmdTools.Exec(cmd), "SHA1");
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            string cmd = "keytool -list -v -storepass {0} -keystore \"{1}\"";
            cmd = string.Format(cmd, text_pass.Text, text_key_path.Text);
            text_hash_result.Text = FormatForKeyWord(CmdTools.Exec(cmd), "MD5");
        }

        private void btn_select_key_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            openFileDialog.Filter = "KeyStore文件(*.keystore,*.jks)|*.keystore;*.jks";
            text_key_path.Text = openFileDialog.FileName;
        }

        private string FormatForKeyWord(string result, string key)
        {
            string[] arr = result.Split(new char[] { '\n', '\r' });
            for(int i=0; i<arr.Length; i++)
            {
                var ss = arr[i];
                if(ss.Contains(key))
                {
                    return ss;
                }
            }
            return "none";
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

        #region Tool Fun
        public void WriteLog(string _txt)
        {
            Console.WriteLine(_txt);
            File.AppendAllText(logPath, "\r\n" + GetTime() + _txt);
        }

        private string GetTime()
        {
            return System.DateTime.Now.ToString() + " ";
        }

        private string GetCurrentPath()
        {
            string str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace("aabViewer.exe", "");
            return str;
        }



        private bool CheckEnvironment(bool isForce = false)
        {
            string doneFile = GetCurrentPath() + "Config/init.txt";
            if (File.Exists(doneFile) && !isForce)
            {
                return true;
            }
            string ret = "";
            string error = "";
            string java = CmdTools.Exec("java -version", ref error);
            if (error.Contains("不是内部或外部命令"))
            {
                ret += "\r\n缺少Java环境";
                WriteLog("Java：" + java + error);
            }
            if (!File.Exists(jarPath))
            {
                ret += "\r\n缺少 bundletool-all-1.8.0.jar";
            }

            if (ret != "")
            {
                MessageBox.Show("运行环境监测失败:" + ret);
                return false;
            }
            else
            {
                File.WriteAllText(GetCurrentPath() + "Config/init.txt", "Done");
                return true;
            }
        }
        #endregion
        #region Command
        private void 环境监测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckEnvironment(true))
            {
                MessageBox.Show("运行环境完整!");
            }
        }
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AbbViewer " + verion);
        }

        private void 配置说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrowserHelper.OpenDefaultBrowserUrl("https://github.com/corle-bell/aabViewer");
        }



        private void 连接手机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetPhoneInfo(false);
        }

        private void 保存Key配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddKeys(text_alias.Text, text_key_path.Text, text_key_pass.Text, text_alias.Text, text_key_pass.Text);
            SaveKeyConfigs();
        }
        #endregion

        #region PhoneInfo
        private void GetPhoneInfo(bool isInit = true)
        {
            string brand = "";
            string model = "";
            string sys_ver = "";
            TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
            installTask = Task.Run(() => {

                brand = CmdTools.Exec("adb -d shell getprop ro.product.brand");
                model = CmdTools.Exec("adb -d shell getprop ro.product.model");
                sys_ver = CmdTools.Exec("adb shell getprop ro.build.version.release");

                installTask = null;

            }).ContinueWith(m =>
            {
                this.text_model.Text = string.Format("{0} {1}", brand, model);
                this.text_version.Text = string.Format("Android {0}", sys_ver);

                if (brand == "")
                {
                    this.label_status.ForeColor = Color.Red;
                    this.label_status.Text = "未连接";

                    if (!isInit) MessageBox.Show("未找到设备!");
                }
                else
                {
                    this.label_status.ForeColor = Color.Green;
                    this.label_status.Text = "已连接";

                    if (!isInit) MessageBox.Show("已连接到 " + this.text_model.Text);
                }
            }, ui);


        }
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == UsbMessage.WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case UsbMessage.WM_DEVICECHANGE:
                            Console.WriteLine("USB change");
                            break;
                        case UsbMessage.DBT_DEVICEARRIVAL://U盘插入   
                            Console.WriteLine("DBT_DEVICEARRIVAL");
                            break;
                        case UsbMessage.DBT_CONFIGCHANGECANCELED:
                            break;
                        case UsbMessage.DBT_CONFIGCHANGED:
                            break;
                        case UsbMessage.DBT_CUSTOMEVENT:
                            break;
                        case UsbMessage.DBT_DEVICEQUERYREMOVE:
                            break;
                        case UsbMessage.DBT_DEVICEQUERYREMOVEFAILED:
                            break;
                        case UsbMessage.DBT_DEVICEREMOVECOMPLETE:   //U盘卸载
                            Console.WriteLine("DBT_DEVICEREMOVECOMPLETE");
                            break;
                        case UsbMessage.DBT_DEVICEREMOVEPENDING:
                            Console.WriteLine("DBT_DEVICEREMOVEPENDING");
                            break;
                        case UsbMessage.DBT_DEVICETYPESPECIFIC:
                            break;
                        case UsbMessage.DBT_DEVNODES_CHANGED: //手机插入时响应
                            GetPhoneInfo(true);
                            break;
                        case UsbMessage.DBT_QUERYCHANGECONFIG:
                            break;
                        case UsbMessage.DBT_USERDEFINED:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            base.WndProc(ref m);
        }
        #endregion

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var key = keyNodes[this.comboBox1.SelectedIndex];
            text_key_path.Text = key.path;
            text_pass.Text = key.password;
            text_alias.Text = key.alias;
            text_key_pass.Text = key.alias_password;
        }

        private void 清理缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            needUpdateApks = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignCreator sc = new SignCreator().Init(this);
            sc.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id = this.comboBox1.SelectedIndex;
            keyNodes.RemoveAt(id);
            this.comboBox1.Items.RemoveAt(id);

            id = id >= keyNodes.Count ? keyNodes.Count-1 : id;
            if(id>=0) this.comboBox1.SelectedIndex = id;
            SaveKeyConfigs();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string cmd = "keytool -list -v -alias {0} -storepass {1} -keystore \"{2}\" -keypass {1}";
            cmd = string.Format(cmd, text_alias.Text, text_pass.Text, text_key_path.Text);

            var ret = CmdTools.Exec(cmd);
            MessageBox.Show(ret);
        }
    }

    public class ConfigNode
    {
        public string name;
        public string path;
        public string filter_name;
    }

    public class KeyNode
    {
        public string name;
        public string path;
        public string alias;
        public string password;
        public string alias_password;

        public string Format()
        {
            return $"{name}#{path}#{password}#{alias}#{alias_password}";
        }

        public KeyNode Init(string _data)
        {
            string[] arr = _data.Split(new char[] { '#' });
            name = arr[0];
            path = arr[1];
            password = arr[2];
            alias = arr[3];
            alias_password = arr[4];
            return this;
        }
    }

    public class UsbMessage
    {
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;
    }
}
