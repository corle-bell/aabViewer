 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Threading.Tasks;
 using System.Diagnostics;
namespace aabViewer
{
    public class CmdTools
    {
        public static string Exec(string _text)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c " + _text;//“/C”表示执行完命令后马上退出 
            p.Start();//启动程序  
            string sOutput = p.StandardOutput.ReadToEnd();
            return sOutput;
        }
    }
}
