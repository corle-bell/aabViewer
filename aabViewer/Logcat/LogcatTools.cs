using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aabViewer.Logcat
{

    public struct LogInfo
    {
        public string Time;
        public string PId;
        public string TId;
        public string LogLevel;
        public string Tag;
        public string Message;

        public bool IsInvalid
        {
            get
            {
                return string.IsNullOrEmpty(Time) || string.IsNullOrEmpty(PId)
                    || string.IsNullOrEmpty(TId) || string.IsNullOrEmpty(LogLevel)
                    || string.IsNullOrEmpty(Tag) || string.IsNullOrEmpty(Message);
            }
        }
    }

    public class LogcatTools
    {
        public static long TimeStamp()
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            // 获取当前的 UTC 时间
            DateTime now = DateTime.UtcNow;
            // 计算时间差
            TimeSpan timeSpan = now - unixEpoch;
            // 获取毫秒数
            long timestamp = (long)timeSpan.TotalMilliseconds;
            return timestamp;
        }
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

        public static Color GetLogTextColor(string logType)
        {
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

        private static readonly string LogPattern = @"^(\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3})\s+(\d+)\s+(\d+)\s+([VIDWEFS])\s+(\S+)\s+(.*)$";

        public static LogInfo ParseLogLine(string logLine)
        {
            // 创建正则表达式对象
            Regex regex = new Regex(LogPattern);
            Match match = regex.Match(logLine);
            LogInfo info = new LogInfo();
            if (match.Success)
            {
                // 提取匹配到的各个部分
                info.Time = match.Groups[1].Value;
                info.PId = (match.Groups[2].Value);
                info.TId = (match.Groups[3].Value);
                info.LogLevel = match.Groups[4].Value;
                info.Tag = match.Groups[5].Value;
                info.Message = match.Groups[6].Value;
            }
            else
            {
                Console.WriteLine("Invalid log line format.");

                info.Time = "";
                info.PId = "";
                info.TId = "";
                info.LogLevel = "";
                info.Tag = "";
                info.Message = logLine;
            }
            return info;
        }
    }
}
