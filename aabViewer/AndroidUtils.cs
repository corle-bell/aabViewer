
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aabViewer
{

    public class AndroidKeystoreCertificateData
    {
        public string FirstAndLastName;
        public string OrganizationalUnit;
        public string OrganizationName;
        public string CityOrLocality;
        public string StateOrProvince;
        public string CountryCode;
    }

    public class AndroidKeystoreData : AndroidKeystoreCertificateData
    {
        public string KeystorePath;
        public string Password;
        public string KeyAlias;
        public string KeyPassword;
        public int ValidityInYears;
    }

    internal class AndroidUtils
    {

        private static bool RunCommand(string command, string working_dir, bool show_window = true)
        {
            using (Process proc = new Process
            {
                StartInfo =
      {
        UseShellExecute = false,
        FileName = "cmd.exe",
        Arguments = command,
        CreateNoWindow = !show_window,
        WorkingDirectory = working_dir
      }
            })
            {
                try
                {
                    proc.Start();
                    proc.WaitForExit();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        private static string FilterString(string st)
        {
            return Regex.Replace(st, @"[^\w\d _]", "").Trim();
        }

        public static string GetKeystoreCertificateInputString(AndroidKeystoreCertificateData data)
        {
            string strCN = FilterString(data.FirstAndLastName);
            string strOU = FilterString(data.OrganizationalUnit);
            string strO = FilterString(data.OrganizationName);
            string strL = FilterString(data.CityOrLocality);
            string cnST = FilterString(data.StateOrProvince);
            string cnC = FilterString(data.CountryCode);

            string cert = "\"";
            if (!string.IsNullOrEmpty(strCN)) cert += "cn=" + strCN + ", ";
            if (!string.IsNullOrEmpty(strOU)) cert += "ou=" + strOU + ", ";
            if (!string.IsNullOrEmpty(strO)) cert += "o=" + strO + ", ";
            if (!string.IsNullOrEmpty(strL)) cert += "l=" + strL + ", ";
            if (!string.IsNullOrEmpty(cnST)) cert += "st=" + cnST + ", ";
            if (!string.IsNullOrEmpty(cnC)) cert += "c=" + cnC + "\"";

            if (cert.Length > 2) return cert;

            return string.Empty;
        }

        private static string GetKeytoolPath()
        {
            string javaHome = Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.User);
            return Path.Combine(javaHome, "bin\\keytool");
        }

        private static string GetKeystoreGenerationCommand(AndroidKeystoreData d)
        {
            string cert = GetKeystoreCertificateInputString(d);
            string keytool = GetKeytoolPath();
            string days = (d.ValidityInYears * 365).ToString();

            string dname = "-dname \"cn=" + d.KeyAlias + "\"";
            if (!string.IsNullOrEmpty(cert)) dname = "-dname " + cert;

            string cmd = "echo y | " + keytool + " -genkeypair " + dname +
                " -alias " + d.KeyAlias + " -keypass " + d.KeyPassword +
                " -keystore " + d.KeystorePath + " -storepass " + d.Password + " -validity " + days;

            return cmd;
        }

        public static bool RunGenerateKeystore(AndroidKeystoreData d)
        {
            string cmd = GetKeystoreGenerationCommand(d);
            string wdir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            return RunCommand(cmd, wdir, false);
        }
    }
}
