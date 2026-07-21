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

        // 统一兼容旧格式(MM-DD 空格分隔, TAG: 消息)与新格式(YYYY-MM-DD Tab分隔, TAG\t消息)
        private static readonly Regex LineRegex = new Regex(
        @"^(?<date>\d{2}-\d{2}|\d{4}-\d{2}-\d{2})\s+" +
        @"(?<time>\d{2}:\d{2}:\d{2}\.\d+)\s+" +
        @"(?<pid>\d+)\s+" +
        @"(?<tid>\d+)\s+" +
        @"(?<level>[VDIWEFS])\s+" +
        @"(?<tag>.+?)(?::\s+|\t+\s*)(?<msg>.*)$",
        RegexOptions.Compiled
    );

        private static LogInfo ParseLogByRegex(string logLine)
        {
            // 创建正则表达式对象
            Regex regex = LineRegex;
            Match match = regex.Match(logLine);
            LogInfo info = new LogInfo();
            if (match.Success)
            {
                // 提取匹配到的各个部分
                // Time 字段：保留完整时间戳（含日期）
                info.Time = $"{match.Groups["date"].Value} {match.Groups["time"].Value}";

                info.PId = match.Groups["pid"].Value;
                info.TId = match.Groups["tid"].Value;
                info.LogLevel = match.Groups["level"].Value;
                info.Tag = match.Groups["tag"].Value.Trim();
                info.Message = match.Groups["msg"].Value;
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

        private static LogInfo ParseLogBySplit(string line)
        {
            LogInfo info = default;

            info.Time = "";
            info.PId = "";
            info.TId = "";
            info.LogLevel = "";
            info.Tag = "";
            info.Message = line;

            int colonIndex = line.IndexOf(": ");
            if (colonIndex < 0)
            {
                Console.WriteLine("Invalid log line format.");
                return info;
            }

            string header = line.Substring(0, colonIndex);
            string message = line.Substring(colonIndex + 2); // 保留全部空格

            var parts = header.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 5)
            {
                return info;
            }

            info.Time = parts[0] + " " + parts[1];
            info.PId = parts[2];
            info.TId = parts[3];
            info.LogLevel = parts[4];
            info.Tag = parts[5];

            info.Message = message;
            return info;
        }
        public static LogInfo ParseLogLine(string logLine)
        {
            // 快速预判：日志头必以数字(日期)开头，非数字开头的续行/堆栈跟踪直接作为 Message-only 返回，跳过正则开销
            if (string.IsNullOrEmpty(logLine) || !char.IsDigit(logLine[0]))
            {
                LogInfo info = new LogInfo();
                info.Message = logLine;
                return info;
            }
            return ParseLogByRegex(logLine);
        }
    }
}
