using aabViewer;
using aabViewer.Logcat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace aabViewer.Logcat
{
    public partial class MainForm : Form
    {
        private Process adbProcess;
        private bool isReading = false;
        private List<LogInfo> allLogs = new List<LogInfo>();
        private Dictionary<int, string> processList = new Dictionary<int, string>();
        private List<LogInfo> pendingLogs = new List<LogInfo>();
        private System.Timers.Timer batchUpdateTimer;
        private bool isClose;
        private bool isPending = true;
        public Form1 Root;

        public UCheckComboBox TagExlude
        {
            get
            {
                return this.tagExludeFilter;
            }
        }

        public string configPath;
        public MainForm()
        {
            InitializeComponent();
            InitializeAdbProcess();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);            

            this.textBoxStringFilter.DelayTextChange += new EventHandler(this.FilterTextBox_TextChanged);
            this.comboBoxTypeFilter.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);

            
            this.textBoxStringFilter.SetWatermark("文本筛选");

            this.tagFilterFilter.OnLeave += new EventHandler(this.FilterTextBox_TextChanged);
            this.tagExludeFilter.OnLeave += new EventHandler(this.FilterTextBox_TextChanged);



            this.checkBox1.Checked = true;

            // 初始化定时器，用于批量更新 ListBox
            batchUpdateTimer = new System.Timers.Timer(300);
            batchUpdateTimer.Elapsed += BatchUpdateTimer_Elapsed;
            batchUpdateTimer.Start();

            configPath = Path.Combine(WinformTools.GetCurrentPath(), "Config/LogcatConfig.json");

            if(File.Exists(configPath))
            {
                LitJson.JsonData config = LitJson.JsonMapper.ToObject(File.ReadAllText(configPath));

                foreach(var t in config["Include"])
                {
                    tagFilterFilter.AddItem(t.ToString(), false);
                }

                foreach (var t in config["Exclude"])
                {
                    tagExludeFilter.AddItem(t.ToString(), false);
                }
            }

            LoadProcessListAsync();
        }

        private void BatchUpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (pendingLogs.Count > 0 && isPending)
            {
                // 检查窗体是否已经被释放
                if (this.IsDisposed)
                {
                    // 如果窗体已经被释放，停止定时器
                    batchUpdateTimer.Stop();
                    return;
                }

                this.Invoke((MethodInvoker)delegate
                {
                    // 再次检查窗体是否已经被释放
                    if (this.IsDisposed)
                    {
                        batchUpdateTimer.Stop();
                        return;
                    }

                    // 检查 listBoxLogs 是否已经被释放
                    if (listBoxLogs != null && !listBoxLogs.IsDisposed)
                    {
                        lock (pendingLogs)
                        {
                            listBoxLogs.BeginUpdate();

                            foreach (var log in pendingLogs)
                            {
                                listBoxLogs.AddLog(log);
                            }

                            if(this.checkBox1.Checked)
                            {
                                listBoxLogs.EnsureVisibleLast();
                            }

                            listBoxLogs.EndUpdate();
                            pendingLogs.Clear();
                        }
                    }
                });
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            batchUpdateTimer.Stop();           

            isClose = true;
            if (isReading)
            {
                try
                {
                    pendingLogs.Clear();
                    // 停止当前的 logcat 进程
                    adbProcess.CancelOutputRead();
                    if (!adbProcess.HasExited)
                    {
                        adbProcess.Kill();
                    }
                    adbProcess.Close();
                    isReading = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"停止日志捕获时出错：{ex.Message}");
                }
            }

            if(Root!=null)
            {
                Root.LogcatForm = null;
            }
            
        }

        private void InitializeAdbProcess()
        {
            adbProcess = new Process();
            adbProcess.StartInfo.FileName = "adb";
            adbProcess.StartInfo.Arguments = "logcat -v threadtime";
            adbProcess.StartInfo.UseShellExecute = false;
            adbProcess.StartInfo.RedirectStandardOutput = true;
            adbProcess.StartInfo.CreateNoWindow = true;
            adbProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            adbProcess.OutputDataReceived += AdbProcess_OutputDataReceived;
        }
   

        private void AdbProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data) && !isClose)
            {
                var log = LogcatTools.ParseLogLine(e.Data);
                if (isReading)
                {
                    if(FilterLog(log))
                    {
                        lock (pendingLogs)
                        {
                            pendingLogs.Add(log);                            
                        }
                    }
                    allLogs.Add(log);
                }
            }
        }

        private bool FilterLog(LogInfo log)
        {
            string tagFilter = "";
            string stringFilter = "";
            string typeFilter = "";
            string pidFilter = "";
            string tagExludeFilterText = "";

            // 获取TAG筛选条件
            if (tagFilterFilter.InvokeRequired)
            {
                tagFilterFilter.Invoke((MethodInvoker)delegate
                {
                    tagFilter = tagFilterFilter.Text.Trim();
                });
            }
            else
            {
                tagFilter = tagFilterFilter.Text.Trim();
            }

            // 获取TAG剔除条件
            if (tagExludeFilter.InvokeRequired)
            {
                tagExludeFilter.Invoke((MethodInvoker)delegate
                {
                    tagExludeFilterText = tagExludeFilter.Text.Trim();
                });
            }
            else
            {
                tagExludeFilterText = tagExludeFilter.Text.Trim();
            }

            // 获取字符串筛选条件
            if (textBoxStringFilter.InvokeRequired)
            {
                textBoxStringFilter.Invoke((MethodInvoker)delegate
                {
                    stringFilter = textBoxStringFilter.Text.Trim();
                });
            }
            else
            {
                stringFilter = textBoxStringFilter.Text.Trim();
            }

            // 获取类型筛选条件
            if (comboBoxTypeFilter.InvokeRequired)
            {
                comboBoxTypeFilter.Invoke((MethodInvoker)delegate
                {
                    typeFilter = comboBoxTypeFilter.SelectedItem?.ToString();
                });
            }
            else
            {
                typeFilter = comboBoxTypeFilter.SelectedItem?.ToString();
            }

            // 获取PID筛选条件
            if (comboBoxProcess.InvokeRequired)
            {
                comboBoxProcess.Invoke((MethodInvoker)delegate
                {
                    pidFilter = comboBoxProcess.SelectedItem?.ToString();
                });
            }
            else
            {
                pidFilter = comboBoxProcess.SelectedItem?.ToString();
            }

            if (!string.IsNullOrEmpty(tagExludeFilterText) && tagExludeFilterText.Contains(log.Tag))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(tagFilter) && !tagFilter.Contains(log.Tag))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(stringFilter) && !log.Message.Contains(stringFilter))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(typeFilter))
            {
                string logType = log.LogLevel;
                if (logType != typeFilter)
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(pidFilter) && !string.IsNullOrEmpty(log.PId))
            {
                int pid = int.Parse(pidFilter.Split('|')[0]);
                if (!log.PId.Equals(pid.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!isReading)
            {
                adbProcess.Start();
                adbProcess.BeginOutputReadLine();
                isReading = true;
                buttonStart.Text = "停止读取";
            }
            else
            {
                adbProcess.CancelOutputRead(); 
                if(!adbProcess.HasExited)
                {
                    adbProcess.Kill();
                }
                isReading = false;
                buttonStart.Text = "开始读取";
            }
        }

        private void buttonClearLogs_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空程序内部存储的日志列表
                allLogs.Clear();

                // 清空界面上的日志列表
                listBoxLogs.ClearAll();

                // 清空完整日志显示框
                textBoxFullLog.Text = "";

                MessageBox.Show("程序读取的日志已清理！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清理日志时出错：{ex.Message}");
            }
        }

       
        public void FilterByTag(string _tag)
        {
            this.tagFilterFilter.AddItem(_tag, false);
        }
        
        private void FilterTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        public void ApplyFilter()
        {
            listBoxLogs.SuspendLayout();
            listBoxLogs.BeginUpdate();

            List<LogInfo> filterLogs = new List<LogInfo>();
            foreach (var log in allLogs)
            {
                if (FilterLog(log))
                {
                    filterLogs.Add(log);                    
                }
            }

            listBoxLogs.UpdateLogs(filterLogs);

            listBoxLogs.EndUpdate();
            listBoxLogs.ResumeLayout();
        }

        private void listBoxLogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxLogs.SelectedIndices.Count == 1)
            {
                textBoxFullLog.Text = listBoxLogs.SelectToString();
            }
        }

        private void buttonSaveSelected_Click(object sender, EventArgs e)
        {
            if (listBoxLogs.SelectedIndices.Count >= 0)
            {
                string selectedLog = listBoxLogs.SelectToString();
                SaveLogToFile(selectedLog);
            }
        }

        private void buttonSaveAll_Click(object sender, EventArgs e)
        {
            string allLogText = listBoxLogs.AllToString();
            SaveLogToFile(allLogText);
        }

        private void SaveLogToFile(string logText)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "文本文件 (*.txt)|*.txt";
            saveFileDialog.Title = "保存日志文件";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, logText);
                    MessageBox.Show("日志保存成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存日志时出错：{ex.Message}");
                }
            }
        }

        private void buttonRefreshProcessList_ClickAsync(object sender, EventArgs e)
        {
            LoadProcessListAsync();
        }

        // 异步加载进程列表的方法
        private async Task LoadProcessListAsync()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = "shell dumpsys activity activities";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                // 异步启动进程
                process.Start();

                // 异步读取输出
                string output = await process.StandardOutput.ReadToEndAsync();

                // 异步等待进程退出
                await Task.Run(() => process.WaitForExit());

                // 清空原有的进程列表和 ComboBox 内容
                processList.Clear();
                comboBoxProcess.Items.Clear();

                var dict = ExtractProcesses(output);

                // 使用示例
                foreach (var m in dict)
                {
                    string processName = m.Value;
                    int pid = m.Key;
                    processList[pid] = processName;
                    comboBoxProcess.Items.Add($"{pid}|{processName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取前台进程列表时出错: {ex.Message}");
            }
        }

        public static Dictionary<int, string> ExtractProcesses(string logContent)
        {
            var processes = new Dictionary<int, string>();
            string pattern = @"ProcessRecord\{[\w]+\s+(\d+):([^/]+)/";

            foreach (Match match in Regex.Matches(logContent, pattern))
            {
                if (match.Groups.Count >= 3)
                {
                    int pid;
                    if (int.TryParse(match.Groups[1].Value, out pid))
                    {
                        string packageName = match.Groups[2].Value.Trim();
                        if (!processes.ContainsKey(pid))
                        {
                            processes.Add(pid, packageName);
                        }
                    }
                }
            }

            return processes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LitJson.JsonData Ret = new LitJson.JsonData();
            LitJson.JsonData IncludeFilter = new LitJson.JsonData();
            LitJson.JsonData ExcludeFilter = new LitJson.JsonData();

            IncludeFilter.SetJsonType(LitJson.JsonType.Array);
            ExcludeFilter.SetJsonType(LitJson.JsonType.Array);

            Ret["Include"] = IncludeFilter;
            Ret["Exclude"] = ExcludeFilter;

            foreach (var t in this.tagFilterFilter.ListBox.Items)
            {
                var item = t as UCheckComboBoxItem;
                IncludeFilter.Add(item.Name);
            }

            foreach (var t in this.tagExludeFilter.ListBox.Items)
            {
                var item = t as UCheckComboBoxItem;
                ExcludeFilter.Add(item.Name);
            }
            File.WriteAllText(configPath, Ret.ToJson());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isPending = !isPending;

            var btn = sender as Button;
            btn.Text = isPending ? "暂停" : "继续";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var file = WinformTools.FileSelect("选择文件", "");

            if (!File.Exists(file)) return;
            var lines = File.ReadAllLines(file);

            listBoxLogs.BeginUpdate();
            for (var i=0; i<lines.Length; i++)
            {
                var log = LogcatTools.ParseLogLine(lines[i]);

                if (FilterLog(log))
                {
                    listBoxLogs.AddLog(log);
                }
                allLogs.Add(log);
            }
            if (this.checkBox1.Checked)
            {
                listBoxLogs.EnsureVisibleLast();
            }
            listBoxLogs.EndUpdate();
        }
    }
}