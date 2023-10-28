 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Threading.Tasks;
 using System.Diagnostics;
using System.Runtime.InteropServices;

namespace aabViewer
{
   
    public class CmdTools
    {
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(

         IntPtr hWnd,   // handle to destination window

         int Msg,    // message

         uint wParam, // first message parameter

         uint lParam // second message parameter

        );

        public static void Enter(Process p)
        {
            SendMessage(p.Handle, 258, 13, 0);
        }

        public static string Exec(string _text)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "/c " + _text;//“/C”表示执行完命令后马上退出 
            p.Start();//启动程序              
            string sOutput = p.StandardOutput.ReadToEnd();
            return sOutput;
        }

        public static string Exec(string _text, ref string _error)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "/c " + _text;//“/C”表示执行完命令后马上退出 
            p.Start();//启动程序  
            string sOutput = p.StandardOutput.ReadToEnd();
            if (_error != null)
            {
                _error = p.StandardError.ReadToEnd();
            }
            return sOutput;
        }

        public static Process Exec1(string _text)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.CreateNoWindow = true;
            //p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.Arguments = _text;//“/C”表示执行完命令后马上退出 
            p.Start();//启动程序
            return p;
        }

        public static Process Exec2(string _text)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe"; //命令
            p.StartInfo.Arguments = _text;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.Start();
            return p;
        }
    }
}
