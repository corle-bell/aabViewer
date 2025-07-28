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

        public override string ToString()
        {
            return $"{Time} {PId} {TId} {LogLevel} {Tag} : {Message}";
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
                case "V":
                    return Color.SaddleBrown;                    
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

        public static void Test()
        {
            var t0 = ParseLogLine("04-08 14:31:06.712 10597 10597 V ONet:ScreenStatusLiveData: (LoadedApk.java:1888)Screen STATE_OFF ");
            var t1 = ParseLogLine("2025-07-28	12:23:38.978	1159	3578	D		WindowManager		takeScreenshotToTargetWindow: targetSurface=null, sourceCrop=Rect(0, 0 - 0, 0)");
        }

        private static readonly string LogPattern = @"^(\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2}\.\d+)\s+(\d+)\s+(\d+)\s+([VDIWEFS])\s+([^:]*):\s*(.*)$";

        private static LogInfo ParseLogByRegex(string logLine)
        {
            // 创建正则表达式对象
            Regex regex = new Regex(LogPattern);
            Match match = regex.Match(logLine);
            LogInfo info = new LogInfo();
            if (match.Success)
            {
                // 提取匹配到的各个部分
                info.Time = $"{match.Groups[1].Value.Trim()} {match.Groups[2].Value.Trim()}";
                info.PId = (match.Groups[3].Value).Trim();
                info.TId = (match.Groups[4].Value).Trim();
                info.LogLevel = match.Groups[5].Value.Trim();
                info.Tag = match.Groups[6].Value.Trim();
                info.Message = match.Groups[7].Value;
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

        private static LogInfo ParseLogBySplit(string logLine)
        {
            string[] log_groups = logLine.Split(new char[] { '\t', ' '}, StringSplitOptions.RemoveEmptyEntries);

            LogInfo info = new LogInfo();
            if (log_groups!=null && log_groups.Length>=7)
            {
                info.Time = $"{log_groups[0].Trim()} {log_groups[1].Trim()}";
                info.PId = (log_groups[2]).Trim();
                info.TId = (log_groups[3]).Trim();
                info.LogLevel = log_groups[4].Trim();
                info.Tag = log_groups[5].Trim();
               

                StringBuilder sb = new StringBuilder();

                for (int i=6; i<log_groups.Length; i++)
                {
                    sb.Append(log_groups[i]);
                }

                info.Message = sb.ToString();
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
        public static LogInfo ParseLogLine(string logLine)
        {
            return ParseLogBySplit(logLine);
        }
    }
}
