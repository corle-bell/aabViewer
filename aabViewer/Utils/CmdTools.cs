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

        public static List<Process> ProgressList = new List<Process>();

        public static void Enter(Process p)
        {
            SendMessage(p.Handle, 258, 13, 0);
        }

        public static void Clean()
        {
            for(int i=0; i< ProgressList.Count; i++)
            {
                if(ProgressList[i]!=null && !ProgressList[i].HasExited)
                {
                    ProgressList[i].Kill();
                }
            }
            ProgressList.Clear();
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

            ProgressList.Add(p);

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

            ProgressList.Add(p);

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
            p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.CreateNoWindow = true;
            //p.StartInfo.Arguments = "/c";//“/C”表示执行完命令后马上退出 


            ProgressList.Add(p);

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
            ProgressList.Add(p);
            p.Start();
            
            return p;
        }

        public static string ExecAppt(string aaptPath, string _text, ref string _error)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = aaptPath;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = _text;//“/C”表示执行完命令后马上退出 
            p.Start();//启动程序  
           
            string sOutput = p.StandardOutput.ReadToEnd();
            if (_error != null)
            {
                _error = p.StandardError.ReadToEnd();
            }
            return sOutput;
        }

        public static string ExecAdb(string _text, ref string _error)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "adb.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = _text;//“/C”表示执行完命令后马上退出 
            ProgressList.Add(p);
            p.Start();//启动程序 

            string sOutput = p.StandardOutput.ReadToEnd();
            if (_error != null)
            {
                _error = p.StandardError.ReadToEnd();
            }
            return sOutput;
        }
    }
}
