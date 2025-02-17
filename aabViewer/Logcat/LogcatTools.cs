using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aabViewer.Logcat
{
    public class LogcatTools
    {
        public static string GetLogType(string log)
        {
            Match match = Regex.Match(log, @"\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}\s+\d+\s+\d+\s+([A-Z])");
            if (match.Success)
            {
                string logType = match.Groups[1].Value;
                return logType;
            }
            return "";
        }

        public static string GetPid(string log)
        {
            string processId = "";
            string pattern = @"^\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}\.\d{3}\s+(\d+)";
            Match match = Regex.Match(log, pattern);
            if (match.Success)
            {
                processId = match.Groups[1].Value;
            }
            return processId;
        }

        public static Color GetLogTextColor(string log)
        {
            // 更精确的正则表达式来匹配日志类型
            string logType = GetLogType(log);
            switch (logType)
            {
                case "D": // Debug 日志，使用蓝色
                    return Color.Blue;
                case "I": // Info 日志，使用绿色
                    return Color.Green;
                case "W": // Warning 日志，使用橙色
                    return Color.Orange;
                case "E": // Error 日志，使用红色
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }

    }
}
