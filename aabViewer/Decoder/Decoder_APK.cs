using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace aabViewer
{
    public class Decoder_APK : Decoder
    {
        public Task installTask;
        public Task parseTask;

        public string DumpData;

        public List<string> AxmlLines = new List<string>();
        public override void Init(Form1 view)
        {
            
            string str = WinformTools.GetCurrentPath();
            ConfigPath = str + "Config/data_apk.ini";

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
            view.InstallButton_Left = "安装并运行";
            view.InstallButton_Right = "安装";
        }

        public override void Decode(Form1 view)
        {
            DecodeByFile(view, view.FilePath);
        }

        public void DecodeByFile(Form1 view, string apkpath, Action callback=null)
        {
            string error = "";
            string filePath = apkpath;
            string iconCmd = string.Format("dump badging \"{0}\"", filePath);
            string xmlCmd = string.Format("dump xmltree \"{0}\" AndroidManifest.xml", filePath);
            string xmlData = "";
            var configNodes = this.configNodes;
            var dataGridView1 = view.DataView;

            LoadingForm.ShowLoading(view);

            if (configNodes.Count >= dataGridView1.RowCount)
            {
                int len = configNodes.Count - dataGridView1.RowCount;
                for (int i = 0; i < len; i++)
                {
                    dataGridView1.Rows.Add();
                }
            }

            TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
            parseTask = Task.Run(() => {

                LoadingForm.PerformStep("开始解析数据~~~");

                ManifestContent = DumpData = CmdTools.ExecAppt(Define.aaptPath, iconCmd, ref error);

                var iconPath = Regex.Match(DumpData, "(?<=application-icon-480:)'([^']*)'").Value.Replace("'", "");

                LoadingForm.PerformStep("获取ICON ~~~");

                SetIconFromZip(view, filePath, iconPath);
                
                parseTask = null;
            }).ContinueWith(m =>
            {
                LoadingForm.PerformStep("解析AndroidManifest ~~~");

                xmlData = CmdTools.ExecAppt(Define.aaptPath, xmlCmd, ref error);



                string[] lines = Regex.Split(xmlData, "\r\n|\n|\r");
                AxmlLines.Clear();
                AxmlLines.AddRange(lines);

                if (error.Length > 0)
                {
                    WinformTools.WriteLog(error);
                }

                LoadingForm.HideLoading();


                for (int i = 0; i < configNodes.Count; i++)
                {
                    ConfigNode tmp = configNodes[i];

                    dataGridView1.Rows[i].Cells[0].Value = tmp.name;
                    dataGridView1.Rows[i].Cells[1].Value = GetValueString(tmp);

                }

                PackageName = GetValueString("badging", "package: name=");
                this.LauncherActivity = FindLauncherActivity();

                callback?.Invoke();
            }, ui);


            view.needUpdateApks = true;
        }

        public override void Install(bool isRun, Form1 view)
        {
            if (installTask == null)
            {
                LoadingForm.ShowLoading(view);

                bool _isRun = false;
                TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
                installTask = Task.Run(() => {

                    LoadingForm.PerformStep("正在安装!");

                    _isRun = Install(view, isRun);
                    installTask = null;
                }).ContinueWith(m =>
                {
                    LoadingForm.HideLoading();

                    if(_isRun)
                    {
                        Run(view);
                    }
                }, ui);

            }
           
        }

        private bool Install(Form1 view, bool isRun)
        {
            var cmd = $"install -r \"{view.FilePath}\"";
            var error = "";
            var result = CmdTools.ExecAdb(cmd, ref error);

            if(result.Contains("Success"))
            {
                if(!isRun)
                {
                    MessageBox.Show("安装成功!");
                }
                return isRun;
            }
            else
            {
                MessageBox.Show(result);
            }
            return false;
        }

        private string FindLauncherActivity()
        {
            var item = AxmlLines.Find((x) => { return x.Contains("android.intent.category.LAUNCHER"); });
            var index = AxmlLines.IndexOf(item);

            for(int i=index-1; i>=0; i--)
            {
                if(AxmlLines[i].Contains("activity"))
                {
                    var str = Regex.Match(AxmlLines[i+2], "(?<==\")[^\"]+(?=\")").Value.Replace("\"", "");
                    return str;
                }
            }
            return "";
        }

        private void CheckConfig()
        {
            var ToolsPath = WinformTools.GetParentPath(WinformTools.GetCurrentPath(), 2);
            ToolsPath = Path.Combine(ToolsPath, "Tools");
            WinformTools.CheckAndCopy(ConfigPath, Path.Combine(ToolsPath, Define.APK_INI), $"缺少{Define.APK_INI}");
        }

        private string FindLine(string key, int offset=0)
        {
            for(int i=0; i< AxmlLines.Count; i++)
            {
                if(AxmlLines[i].Contains(key))
                {
                    return AxmlLines[i + offset];
                }
            }
            return "";
        }

        private string GetValueString(ConfigNode node)
        {
            return GetValueString(node.path, node.filter_name);
        }

        private string GetValueString(string type, string key)
        {
            if (type.Equals("badging"))
            {
                var regex_str = string.Format(@"(?<={0})'([^']*)'", key);
                var str = Regex.Match(DumpData, regex_str).Value.Replace("'", "");
                return str;
            }
            else if (type.Equals("xml_value"))
            {
                var str = FindLine(key);
                str = Regex.Match(str, @"(?<=\=).*$").Value.Replace("\"", "");
                if (str.Contains("0x"))
                {
                    str = Regex.Match(str, @"(?<=\)0x)\w+").Value;
                    str = Convert.ToInt32(str, 16).ToString();
                }
                return str;
            }
            else if (type.Equals("xml_key"))
            {
                var str = FindLine(key, 1);
                str = Regex.Match(str, "(?<==\")[^\"]+(?=\")").Value.Replace("\"", "");
                return str;
            }
            return "";
        }
    }
}
