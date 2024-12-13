using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace aabViewer
{
    public class WinformTools
    {
        /*public static string FolderSelect(string _default)
        {
            var commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.IsFolderPicker = true;	//设置为true为选择文件夹，设置为false为选择文件
            commonOpenFileDialog.Title = "选择文件夹";            
            commonOpenFileDialog.InitialDirectory = _default;
            var result = commonOpenFileDialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                return commonOpenFileDialog.FileName;
            }
            return "";
        }*/

        public static string FileSelect(string _title, string _filter)
        {
            string ret = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = _filter;
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ret = openFileDialog.FileName;
            }
            return ret;
        }

        public static string GetTime()
        {
            return System.DateTime.Now.ToString() + " ";
        }

        public static string GetCurrentPath()
        {
            string str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace("aabViewer.exe", "");
            return str;
        }

        public static void Log(string _txt)
        {
            Debug.WriteLine(_txt);
        }

        public static string FileToBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

        public static string GetFileTail(string file)
        {
            return Path.GetExtension(file);
        }


        public static string GetLastPath(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            return di.Name;
        }

        public static void CopyFolder(string _src, string _dest)
        {
            string[] directorys = Directory.GetDirectories(_src, "*.*", SearchOption.AllDirectories);
            string[] files = Directory.GetFiles(_src, "*.*", SearchOption.AllDirectories);
            foreach(var sub_dir in directorys)
            {
                var folder = sub_dir.Replace(_src, _dest);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }

            foreach (var file in files)
            {
                File.Copy(file, file.Replace(_src, _dest));
            }
        }

        public static void MoveFolder(string _src, string _dest)
        {
            string[] directorys = Directory.GetDirectories(_src, "*.*", SearchOption.AllDirectories);
            string[] files = Directory.GetFiles(_src, "*.*", SearchOption.AllDirectories);
            foreach (var sub_dir in directorys)
            {
                var folder = sub_dir.Replace(_src, _dest);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }

            foreach (var file in files)
            {
                File.Move(file, file.Replace(_src, _dest));
            }
        }

        public static void OpenFolder(string path)
        {
            if(Directory.Exists(path))
            {
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }

        public static void OpenUrl(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        public static void CallExe(string exe, string filePath)
        {
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(exe, filePath);
            }
        }

        public static void OpenFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(filePath);
            }
        }

        public static void DeleteFilesInDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (string filePath in Directory.GetFiles(directoryPath))
                {
                    File.Delete(filePath);
                }

                foreach (string subdirectory in Directory.GetDirectories(directoryPath))
                {
                    DeleteFilesInDirectory(subdirectory);
                }
            }
        }

        public static bool GetJavaHome(out string error)
        {
            bool isJava = false;
            error = "";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = "-version",
                RedirectStandardError = true,
                UseShellExecute = false
            };

            try
            {
                Process process = Process.Start(psi);
                process.WaitForExit();
                isJava = true;
            }
            catch (Exception e)
            {
                error = e.ToString();
            }
            return isJava;
        }

        public static void WriteLog(string _txt)
        {
            Console.WriteLine(_txt);
            File.AppendAllText(Define.logPath, "\r\n" + GetTime() + _txt);
        }

        public static string GetParentPath(string path, int backTimes=1)
        {
            var ret = path;
            try
            {
                for (int i = 0; i < backTimes + 1; i++)
                {
                    ret = Directory.GetParent(ret).FullName;
                }
            }
            catch(Exception e)
            {
                return ret;
            }
            return ret;
        }


        public static void CheckAndCopy(string filedest, string file_src, string message)
        {
            if (!File.Exists(filedest))
            {
                if (!File.Exists(file_src))
                {
                    MessageBox.Show(message);
                }
                else
                {
                    File.Copy(file_src, filedest);
                }
            }
        }

        public static long TimeStamp()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime now = DateTime.UtcNow;
            long timestamp = (long)(now - epoch).TotalSeconds;
            return timestamp;
        }
    }

}
