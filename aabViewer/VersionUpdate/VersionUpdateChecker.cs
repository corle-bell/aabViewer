using System;
using System.Net.Http;
using LitJson;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;

namespace aabViewer.VersionUpdate
{
    public class VersionUpdateChecker
    {
        // GitHub 仓库信息
        private const string GitHubApiUrl = "https://api.github.com/repos/corle-bell/aabViewer/releases/latest";

        public static string UpdateInfo;

        public static string DownLoadURL;

        public static Version LastVersion;
        public static async Task<bool> CheckForUpdatesAsync(string platform, bool isForce=false)
        {            
            try
            {
                // 获取本地应用程序的版本号
                Version Ignored_Ver = Version.Parse(Properties.Settings.Default.Ignored_Ver);
                Version localVersion = GetLocalVersion();
                Version latestVersion;

                if (platform.Equals(Define.GitHub, StringComparison.OrdinalIgnoreCase))
                {
                    // 从 GitHub 获取最新 Release 的版本号
                    latestVersion = await GetLatestReleaseVersionFromGitHubAsync();
                }
                else
                {
                    throw new ArgumentException("Invalid platform. Please choose either 'GitHub' or 'Gitee'.");
                }

                if(!isForce && latestVersion == Ignored_Ver)
                {
                    return false;
                }
                LastVersion = latestVersion;
                // 比较本地版本和最新版本
                return latestVersion > localVersion;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking for updates: {ex.Message}");
                return false;
            }
        }

        public static Version GetLocalVersion()
        {
            // 获取当前应用程序的程序集
            Assembly assembly = Assembly.GetExecutingAssembly();
            // 获取程序集的版本信息
            AssemblyName assemblyName = assembly.GetName();
            return assemblyName.Version;
        }

        private static async Task<Version> GetLatestReleaseVersionFromGitHubAsync()
        {
            // 指定支持的 SSL/TLS 版本
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 设置请求头，模拟浏览器请求，GitHub API 要求必须设置 User - Agent
                    client.DefaultRequestHeaders.Add("User-Agent", value: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36");

                    // 发送 GET 请求到 GitHub API
                    HttpResponseMessage response = await client.GetAsync(GitHubApiUrl);
                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string json = await response.Content.ReadAsStringAsync();
                    // 使用 LitJson 解析 JSON 数据
                    JsonData data = JsonMapper.ToObject(json);
                    var assets = data["assets"];
                    string tagName = data["tag_name"].ToString();                    
                    UpdateInfo = $"{tagName}\r\n{data["body"].ToString()}";
                    DownLoadURL = assets[0]["browser_download_url"].ToString();
                    // 移除版本号前面可能存在的 "v" 字符
                    if (tagName.StartsWith("v"))
                    {
                        tagName = tagName.Substring(1);
                    }
                    // 将标签名转换为 Version 对象
                    return Version.Parse(tagName);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error checking for updates: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking for updates: {ex.Message}");
                    return null;
                }
                
            }
        }
    }

}
