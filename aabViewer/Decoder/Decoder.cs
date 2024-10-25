using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public virtual void Run(Form1 view)
        {
            if (RunTask == null)
            {
                LoadingForm.ShowLoading(view);

                TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
                RunTask = Task.Run(() => {

                    LoadingForm.PerformStep("正在启动应用!");

                    var cmd = $"adb shell am start {PackageName}/{LauncherActivity}";
                    var error = "";
                    var result = CmdTools.Exec(cmd, ref error);

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
