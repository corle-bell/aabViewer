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
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Reflection;

namespace aabViewer
{
    public enum AFileType
    {
        APK,
        AAB,
        XAPK,
        APKS
    }

    public partial class Form1 : Form
    {
       
        public List<KeyNode> keyNodes = new List<KeyNode>();
        public List<string> SignTemplete = new List<string>();
       

        public Task PhoneInfoTask;

        public bool needUpdateApks;

        private Decoder decoderAAB;
        private Decoder decoderAPK;
        private Decoder decoderXAPK;

        private AFileType fileType = AFileType.AAB;

        public aabViewer.Logcat.MainForm LogcatForm;
        public Decoder decoder
        {
            get
            {
                switch(fileType)
                {
                    case AFileType.AAB:
                        return decoderAAB;
                    case AFileType.APK:
                        return decoderAPK;
                    case AFileType.XAPK:
                        return decoderXAPK;
                    default:
                        return decoderAPK;
                }
            }
        }

        #region for decoder
        public Decoder ApkDecoder => decoderAPK;
        public PictureBox IconImg => pictureBox1;
        public DataGridView DataView => dataGridView1;

        public string FilePath => text_aab_path.Text;

        public string KeyPath => text_key_path.Text;
        public string AliasPass => text_pass.Text;
        public string KeyAlias => text_alias.Text;
        public string KeyPass => text_key_pass.Text;

        public string InstallButton_Left
        {
            get
            {
                return btn_install.Text;
            }
            set
            {
                btn_install.Text = value;
            }
        }

        public string InstallButton_Right
        {
            get
            {
                return btn_install_part.Text;
            }
            set
            {
                btn_install_part.Text = value;
            }
        }

        #endregion


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
            DecodeFile();

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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            CmdTools.Clean();

            if(LogcatForm!=null)
            {
                LogcatForm.Close();
            }
        }

        #region KeyConfigs
        private void CreateDefaultKeys()
        {
            AddKeys("测试签名", WinformTools.GetCurrentPath() + "Config/debug.keystore", "android", "androiddebugkey", "android");
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
            File.WriteAllText(Define.keyConfigPath, builder.ToString());
        }

        private void LoadKeyConfig()
        {
            string[] data = File.ReadAllLines(Define.keyConfigPath);
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

        private void CheckTools()
        {
            var ToolsPath = WinformTools.GetParentPath(WinformTools.GetCurrentPath(), 2);
            ToolsPath = Path.Combine(ToolsPath, "Tools");
            WinformTools.CheckAndCopy(Define.jarPath, Path.Combine(ToolsPath, Define.BundleToolFile), $"缺少{Define.BundleToolFile}");
            WinformTools.CheckAndCopy(Define.aaptPath, Path.Combine(ToolsPath, Define.AAPTFile), $"缺少{Define.AAPTFile}");
        }


        private void Init()
        {
            string str = WinformTools.GetCurrentPath();


            Define.logPath = str + Define.LogFile;
            Define.jarPath = str + Define.BundleToolFile;
            Define.keyConfigPath = str + Define.KeysFile;
            string signConfigPath = str + Define.SignFile;
            Define.aaptPath = str + Define.AAPTFile;



            if (File.Exists(signConfigPath))
            {
                LoadSignConfig(signConfigPath);
            }

            if (!File.Exists(Define.keyConfigPath))
            {
                CreateDefaultKeys();
            }
            else
            {
                LoadKeyConfig();
            }

            CheckTools();

            this.AllowDrop = true;

            this.dataGridView1.DragEnter += this.Form1_DragEnter;
            this.flowLayoutPanel2.DragEnter += this.Form1_DragEnter;
            this.DragEnter += this.Form1_DragEnter;


            this.dataGridView1.DragDrop += this.Form1_DragDrop;
            this.flowLayoutPanel2.DragDrop += this.Form1_DragDrop;
            this.DragDrop += this.Form1_DragDrop;


            for (int i = 0; i < 10; i++)
            {
                dataGridView1.Rows.Add();
            }

            decoderAAB = new Decoder_AAB();
            decoderAPK = new Decoder_APK();
            decoderXAPK = new Decoder_XAPK();

            decoderAAB.Init(this);
            decoderAPK.Init(this);
            decoderXAPK.Init(this);
        }

        private void DecodeFile()
        {
            if(!File.Exists(text_aab_path.Text))
            {
                MessageBox.Show("文件不存在!");
                return;
            }

            string extName = Path.GetExtension(text_aab_path.Text).ToLower();
            if(extName.Contains("aab"))
            {
                fileType = AFileType.AAB;
            }
            else if (extName.Contains("xapk"))
            {
                fileType = AFileType.XAPK;
            }
            else if (extName.Contains("apk"))
            {
                fileType = AFileType.APK;
            }

            decoder.SwitchUI(this);

            decoder.Decode(this);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            text_aab_path.Text = openFileDialog.FileName;

            DecodeFile();
        }

        private void btn_install_Click(object sender, EventArgs e)
        {
            decoder.Install(true, this);
        }

        private void btn_install_part_Click(object sender, EventArgs e)
        {
            decoder.Install(false, this);
        }

     
        private void btn_base_hash_Click(object sender, EventArgs e)
        {
            string openssl = WinformTools.GetCurrentPath() + "openssl.exe";
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
            var pathExt = Path.GetExtension(path).ToLower();
            if(pathExt.Contains(".aab") | pathExt.Contains(".apk") | pathExt.Contains(".xapk"))
            {
                text_aab_path.Text = path;
                DecodeFile();
            }
            else if (pathExt.Contains(".jks") || pathExt.Contains(".keystore"))
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
        


        private bool CheckEnvironment(bool isForce = false)
        {
            string doneFile = WinformTools.GetCurrentPath() + "Config/init.txt";
            if (File.Exists(doneFile) && !isForce)
            {
                return true;
            }
            string ret = "";
            string error = "";
            if (!WinformTools.GetJavaHome(out error))
            {
                ret += "\r\n缺少Java环境";
                WinformTools.WriteLog("Java：" + error);
            }
            if (!File.Exists(Define.jarPath))
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
                File.WriteAllText(WinformTools.GetCurrentPath() + "Config/init.txt", "Done");
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
            MessageBox.Show("AbbViewer " + Define.verion);
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
            PhoneInfoTask = Task.Run(() => {

                string error = "";
                brand = CmdTools.ExecAdb("-d shell getprop ro.product.brand", ref error);
                if(!string.IsNullOrEmpty(brand))
                {
                    model = CmdTools.ExecAdb("-d shell getprop ro.product.model", ref error);
                }
                if (!string.IsNullOrEmpty(model))
                {
                    sys_ver = CmdTools.ExecAdb("shell getprop ro.build.version.release", ref error);
                }
                
                PhoneInfoTask = null;

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
            string cmd = "keytool -list -v -storepass {0} -keystore \"{1}\"";
            cmd = string.Format(cmd, text_pass.Text, text_key_path.Text);

            var ret = CmdTools.Exec(cmd);
            MessageBox.Show(ret);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ScrollTextBox sc = new ScrollTextBox();
            sc.Show(decoder.ManifestContent);
        }

        private void 查看LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinformTools.OpenFile(WinformTools.GetCurrentPath() + "/log.txt");
        }

        private void 查看缓存目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinformTools.OpenFolder(Path.Combine(WinformTools.GetCurrentPath(), "Temp"));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            decoder.Run(this);
        }

        private void 清理进程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CmdTools.Clean();
        }

        private void 打开LogcatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(LogcatForm!=null)
            {
                LogcatForm.Focus();
                return;
            }

            LogcatForm = new aabViewer.Logcat.MainForm();
            LogcatForm.StartPosition = FormStartPosition.CenterScreen;
            LogcatForm.Show();
            LogcatForm.Root = this;
        }

        private void 开启调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string error = "";
            var ret = CmdTools.ExecAdb($"shell setprop debug.firebase.analytics.app {decoder.PackageName}", ref error);
            if (string.IsNullOrEmpty(error))
            {
                MessageBox.Show(ret);
            }
            else
            {
                MessageBox.Show(error);
            }
        }

        private void 关闭调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string error = "";
            var ret = CmdTools.ExecAdb("shell setprop debug.firebase.analytics.app .none.", ref error);
            if (string.IsNullOrEmpty(error))
            {
                MessageBox.Show(ret);
            }
            else
            {
                MessageBox.Show(error);
            }
        }
    }

    
}
