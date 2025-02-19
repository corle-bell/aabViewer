using aabViewer;
using aabViewer.Logcat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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

        public Form1 Root;
        public MainForm()
        {
            InitializeComponent();
            InitializeAdbProcess();

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);

            this.tagFilterFilter.DelayTextChange += new EventHandler(this.FilterTextBox_TextChanged);
            this.textBoxStringFilter.DelayTextChange += new EventHandler(this.FilterTextBox_TextChanged);
            this.comboBoxTypeFilter.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);


            this.tagFilterFilter.SetWatermark("TAG筛选");
            this.textBoxStringFilter.SetWatermark("文本筛选");

            LoadProcessList();

            this.checkBox1.Checked = true;

            // 初始化定时器，用于批量更新 ListBox
            batchUpdateTimer = new System.Timers.Timer(300);
            batchUpdateTimer.Elapsed += BatchUpdateTimer_Elapsed;
            batchUpdateTimer.Start();
        }

        private void BatchUpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (pendingLogs.Count > 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    lock (pendingLogs)
                    {
                        listBoxLogs.BeginUpdate();

                        foreach (var log in pendingLogs)
                        {
                            listBoxLogs.AddLog(log);
                        }

                        listBoxLogs.EnsureVisibleLast();

                        listBoxLogs.EndUpdate();
                        pendingLogs.Clear();

                    }
                });
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isReading)
            {
                try
                {
                    // 停止当前的 logcat 进程
                    adbProcess.CancelOutputRead();
                    adbProcess.Kill();
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
            batchUpdateTimer.Stop();
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
            if (!string.IsNullOrEmpty(e.Data))
            {
                var log = LogcatTools.ParseLogLine(e.Data);
                if (isReading && FilterLog(log))
                {
                    lock (pendingLogs)
                    {
                        pendingLogs.Add(log);
                        allLogs.Add(log);
                    }
                }
            }
        }

        private bool FilterLog(LogInfo log)
        {
            string tagFilter = "";
            string stringFilter = "";
            string typeFilter = "";
            string pidFilter = "";

            // 获取进程筛选条件
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

            // 获取类型筛选条件
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

            if (!string.IsNullOrEmpty(tagFilter) && !log.Tag.Contains(tagFilter))
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
                adbProcess.Kill();
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

        private void ListBoxLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                Color c = Color.LightGray;
                if(e.Index % 2 == 0)
                {
                    c = Color.FromArgb(200, c);
                }
                using (SolidBrush brush = new SolidBrush(c))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }

                string log = listBoxLogs.Items[e.Index].ToString();
                Color textColor = LogcatTools.GetLogTextColor(log);
                using (SolidBrush textBrush = new SolidBrush(textColor))
                {
                    e.Graphics.DrawString(log, e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                }
            }
            e.DrawFocusRectangle();
        }

        public void FilterByTag(string _tag)
        {
            this.tagFilterFilter.Text = _tag;
        }
        
        private void FilterTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
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

        private void buttonRefreshProcessList_Click(object sender, EventArgs e)
        {
            LoadProcessList();
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

        private void LoadProcessList()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = "shell dumpsys activity activities";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

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
    }
}