using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aabViewer
{
    /// <summary>
    /// 基于.NET 2.0的TextBox工具类
    /// </summary>
    public static class TextBoxToolV2
    {
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        private static extern Int32 SendMessage
         (IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// 为TextBox设置水印文字
        /// </summary>
        /// <param name="textBox">TextBox</param>
        /// <param name="watermark">水印文字</param>
        public static void SetWatermark(this TextBox textBox, string watermark)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, watermark);
        }
        /// <summary>
        /// 清除水印文字
        /// </summary>
        /// <param name="textBox">TextBox</param>
        public static void ClearWatermark(this TextBox textBox)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, string.Empty);
        }
    }

    public class ConfigNode
    {
        public string name;
        public string path;
        public string filter_name;
    }

    public class KeyNode
    {
        public string name;
        public string path;
        public string alias;
        public string password;
        public string alias_password;

        public string Format()
        {
            return $"{name}#{path}#{password}#{alias}#{alias_password}";
        }

        public KeyNode Init(string _data)
        {
            string[] arr = _data.Split(new char[] { '#' });
            name = arr[0];
            path = arr[1];
            password = arr[2];
            alias = arr[3];
            alias_password = arr[4];
            return this;
        }
    }

    public class UsbMessage
    {
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;
    }

    public class Define
    {
        public static string logPath = "";
        public static string lastParse;
        public static string jarPath;
        public static string aaptPath;
        public static string keyConfigPath = "";
        public const string verion = "v5.0.1";

        public const string LogFile = "log.txt";
        public const string BundleToolFile = "bundletool-all-1.8.0.jar";
        public const string KeysFile = "Config/keys.ini";
        public const string SignFile = "Config/sign.ini";
        public const string AAPTFile = "aapt.exe";

        public const string AAB_INI = "Config/data.ini";
        public const string APK_INI = "Config/data_apk.ini";
    }
}
