using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace aabViewer
{
    public class Decoder_AAB: Decoder
    {
        public Task installTask;
        public Task parseTask;

       

        public override void Init(Form1 view)
        {
            string str = WinformTools.GetCurrentPath();
            ConfigPath = str + "Config/data.ini";

            CheckConfig();

            if (File.Exists(ConfigPath))
            {
                string[] data = File.ReadAllLines(ConfigPath);

                configNodes = new List<ConfigNode>();

                for (int i = 0; i < data.Length; i++)
                {
                    string[] _split_data = data[i].Split(new char[] { '#' });
                    ConfigNode tmp = new ConfigNode();
                    tmp.name = _split_data[0];
                    tmp.path = _split_data[1];
                    tmp.filter_name = _split_data[2];
                    configNodes.Add(tmp);
                }
            }
        }

        public override void SwitchUI(Form1 view)
        {
            view.InstallButton_Left = "device模式安装";
            view.InstallButton_Right = "universal模式安装";
        }

        public override void Decode(Form1 view)
        {
            string error = "";
            string filePath = view.FilePath;
            string cmd = string.Format("java -jar \"{0}\" dump manifest --bundle \"{1}\"", Define.jarPath, filePath);
            string xmlData = "";
            var configNodes = this.configNodes;
            var dataGridView1 = view.DataView;

            LoadingForm.ShowLoading(view);

            if (configNodes.Count >= dataGridView1.RowCount)
            {
                int len = configNodes.Count - dataGridView1.RowCount;
                for (int i=0; i< len; i++)
                {
                    dataGridView1.Rows.Add();
                }
            }

            TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
            parseTask = Task.Run(() => {

                LoadingForm.PerformStep("开始解析数据~~~");

                xmlData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + CmdTools.Exec(cmd, ref error);

                ManifestContent = xmlData;

                LoadingForm.PerformStep("获取ICON ~~~");

                SetIconFromZip(view, filePath, "base/res/mipmap-xxxhdpi-v4/app_icon.png");
           
                parseTask = null;
            }).ContinueWith(m =>
            {
                LoadingForm.HideLoading();

                if (error.Length > 0)
                {
                    WinformTools.WriteLog(error);
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

                FindInfo(doc, nsp);

                for (int i = 0; i < configNodes.Count; i++)
                {
                    ConfigNode tmp = configNodes[i];
                    dataGridView1.Rows[i].Cells[0].Value = tmp.name;

                    if (tmp.filter_name == "")
                    {
                        XmlNode t = doc.SelectSingleNode(tmp.path, nsp);
                        string text = t != null ? t.Value : "不存在";

                        if (text.StartsWith("@string/"))
                        {
                            var s_value = GetStringValue(text, filePath);
                            dataGridView1.Rows[i].Cells[1].Value = s_value;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[1].Value = text;
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
                        dataGridView1.Rows[i].Cells[1].Value = text;
                    }
                }

                SignManager.I.Match(view, PackageName);

            }, ui);


            view.needUpdateApks = true;
        }


        public override void Install(bool isDevice, Form1 view)
        {
            if (installTask == null)
            {
                LoadingForm.ShowLoading(view);

                TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
                installTask = Task.Run(() => {
                    InstallApk(view.FilePath, isDevice, view);
                    installTask = null;
                }).ContinueWith(m =>
                {
                    LoadingForm.HideLoading();
                }, ui);

            }
        }

        private void CheckConfig()
        {
            var ToolsPath = WinformTools.GetParentPath(WinformTools.GetCurrentPath(), 2);
            ToolsPath = Path.Combine(ToolsPath, "Tools");
            WinformTools.CheckAndCopy(ConfigPath, Path.Combine(ToolsPath, Define.AAB_INI), $"缺少{Define.AAB_INI}");
        }

        void FindInfo(XmlDocument doc, XmlNamespaceManager nsp)
        {
            XmlNode node = doc.SelectSingleNode("/manifest/@package", nsp);
            PackageName = node.Value;


            XmlNodeList nodes = doc.SelectNodes("/manifest/application/activity", nsp);
            foreach (XmlNode item in nodes)
            {
                if (item.InnerXml.Contains("android.intent.category.LAUNCHER")
                    && item.InnerXml.Contains("android.intent.action.MAIN"))
                {
                    LauncherActivity = item.SelectSingleNode("@android:name", nsp).Value;
                }
            }
        }
        

        private void InstallApk(string filePath, bool createForCnnectDevices, Form1 view)
        {
            //设置apks输出路径
            string outPath = WinformTools.GetCurrentPath() + "Temp/";
            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件不存在!");
                return;
            }


            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }


            outPath += "temp.apks";
            if (File.Exists(outPath) && view.needUpdateApks)
            {
                File.Delete(outPath);
            }

            // {0} aab路径 {1}输出路径 {2}keystroe路径 {3}秘钥密码 {4}秘钥别名 {5}秘钥别名密码 {6}配置文件路径
            //string cmd = "java - jar bundletool-all-1.8.0.jar build - apks--bundle ={0} --output ={1} --ks ={2} --ks - pass = pass:{3} --ks - key - alias ={4} --key - pass = pass:{5} --device - spec ={6}";

            string cmd = "";
            string error = "";
            string ret = "";
            if (view.needUpdateApks)
            {
                LoadingForm.PerformStep("正在生成apks~~~~~");
                cmd = create_install_cmd(outPath, view, createForCnnectDevices);

                //执行指令
                error = "";
                ret = CmdTools.Exec(cmd, ref error);
                Console.WriteLine(cmd);

                if (!File.Exists(outPath))
                {
                    WinformTools.WriteLog("Create Error: " + error);
                    WinformTools.WriteLog("Create Ret: " + ret);

                    MessageBox.Show("生成失败!");
                    return;
                }
                else
                {
                    if (ret.Length > 0) MessageBox.Show("Info:" + ret);
                    if (error.Length > 0) MessageBox.Show("Info:" + error);

                    SignManager.I.UpdateSign(PackageName, view);
                }

                view.needUpdateApks = false;
            }


            LoadingForm.PerformStep("开始进行安装~~~~~");

            //安装指令
            error = "";
            cmd = string.Format("java -jar \"{0}\" install-apks --apks=\"{1}\"", Define.jarPath, outPath);
            ret = CmdTools.Exec(cmd, ref error);


            if (ret.Length == 0)
            {
                MessageBox.Show("安装成功!");
            }
            else
            {
                WinformTools.WriteLog("Install Error: " + error);
                WinformTools.WriteLog("Install Ret: " + ret);

                MessageBox.Show("安装失败!");
            }
        }

        private string create_install_cmd(string outPath, Form1 view,  bool createForCnnectDevices = true)
        {
            string _key_path = view.KeyPath;
            string _alias_pass = view.AliasPass;
            string _key_alias = view.KeyAlias;
            string _key_pass = view.KeyPass;

            if (!File.Exists(_key_path))
            {
                _key_path = WinformTools.GetCurrentPath() + "Config/debug.keystore";
                _alias_pass = "android";
                _key_pass = "android";
                _key_alias = "androiddebugkey";
            }

            {
                //根据连接手机的设备创建apks的指令
                string cmd = "java -jar \"{0}\" build-apks --bundle=\"{1}\" --output=\"{2}\" --ks=\"{3}\" --ks-pass=pass:{4} --ks-key-alias={5} --key-pass=pass:{6} {7}";
                //填充志林该参数
                cmd = string.Format(cmd, Define.jarPath, view.FilePath, outPath, _key_path, _alias_pass, _key_alias, _key_pass, createForCnnectDevices ? "--connected-device" : "--mode=universal");
                return cmd;
            }
        }


        public string GetStringValue(string _key, string _abbPath)
        {
            string t_key = _key.Replace("@", "");
            string _cmd_string_name = string.Format("java -jar \"{0}\" dump resources --bundle \"{1}\" --resource={2} --values true", Define.jarPath, _abbPath, t_key);
            var ret = CmdTools.Exec(_cmd_string_name);

            foreach (Match match in Regex.Matches(ret, "\"([^\"]*)\""))
                ret = match.ToString();

            return ret.Replace("\"", "");
        }
    }
}
