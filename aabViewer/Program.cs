using aabViewer.VersionUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aabViewer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length>0 && args[0].Equals("Logcat"))
            {
                Application.Run(new aabViewer.Logcat.MainForm());
            }
            else
            {
                Application.Run(new Form1(args));
            }
        }

    }
}
