using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Drawing;

namespace aabViewer
{
    public class Decoder
    {
        public string ManifestContent;
        public List<ConfigNode> configNodes = new List<ConfigNode>();
        public string LauncherActivity;
        public string PackageName;
        public Task RunTask;
        public string ConfigPath;
        public virtual void Init(Form1 view)
        {

        }

        public virtual void Decode(Form1 view)
        {

        }

        public virtual void Install(bool isDevice, Form1 view)
        {

        }

        public virtual void SwitchUI(Form1 view)
        {

        }

        public void SetIconFromZip(Form1 view, string filePath, string iconPath)
        {
            using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    var zipArchiveEntry = archive.GetEntry(iconPath);

                    if (zipArchiveEntry != null)
                    {
                        var outputStream = zipArchiveEntry.Open();

                        Image img = Image.FromStream(outputStream);
                        view.IconImg.Invoke(new Action(() => view.IconImg.BackgroundImage = img));
                        view.IconImg.Invoke(new Action(() => view.IconImg.BackgroundImageLayout = ImageLayout.Stretch));
                    }
                    else
                    {
                        WinformTools.WriteLog($"Get Icon Fail {iconPath}");
                    }
                }
            }
        }

        public virtual void Run(Form1 view)
        {
            if (RunTask == null)
            {
                LoadingForm.ShowLoading(view);

                TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
                RunTask = Task.Run(() => {

                    LoadingForm.PerformStep("正在启动应用!");

                    var error = "";
                    string result = "";
                    bool started = false;

                    // LauncherActivity 非空时优先用 am start(更快、输出更明确)
                    if (!string.IsNullOrEmpty(LauncherActivity))
                    {
                        var cmd = $"shell am start {PackageName}/{LauncherActivity}";
                        result = CmdTools.ExecAdb(cmd, ref error);
                        // am start 成功输出形如 "Starting: Intent { cmp=... }"
                        started = result.Contains("Starting:") && !result.Contains("Error");
                    }

                    // am start 失败或未解析到 LauncherActivity 时，回退用 monkey
                    // 由系统 PackageManager 自行解析启动 Activity，对 manifest 结构零依赖
                    if (!started)
                    {
                        var mcmd = $"shell monkey -p {PackageName} -c android.intent.category.LAUNCHER 1";
                        error = "";
                        result = CmdTools.ExecAdb(mcmd, ref error);
                        // monkey 成功输出含 "Events injected:"
                        if (result.Contains("Events injected:") && !result.ToLower().Contains("no activities found"))
                        {
                            result = "启动成功";
                        }
                    }

                    MessageBox.Show(result);

                    RunTask = null;
                }).ContinueWith(m =>
                {
                    LoadingForm.HideLoading();

                }, ui);
            }
        }
    }
}
