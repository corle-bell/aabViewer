﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;

namespace aabViewer
{
    public class Decoder_XAPK : Decoder
    {
        public Task installTask;
        public Task parseTask;

        public List<string> apksPath = new List<string>();
        public string CachePath;
        public string manifestPath;
        public string BaseAPKPath;

        public string ObbPath = "";
        public string ObbInstallPath = "";
        public override void Init(Form1 view)
        {
            CachePath = Path.Combine(WinformTools.GetCurrentPath(), "Temp/XAPK/");
            manifestPath = Path.Combine(CachePath, "manifest.json");

            if (!Directory.Exists(CachePath))
            {
                Directory.CreateDirectory(CachePath);
            }

        }

        public override void SwitchUI(Form1 view)
        {
            view.ApkDecoder.SwitchUI(view);
        }

        public override void Decode(Form1 view)
        {
            string filePath = view.FilePath;

            TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
            parseTask = Task.Run(() => {

                LoadingForm.PerformStep("开始解析数据~~~");


                WinformTools.DeleteFilesInDirectory(CachePath);


                ZipFile.ExtractToDirectory(filePath, CachePath);
        
                ParseManifest();


                parseTask = null;
            }).ContinueWith(m =>
            {
                LoadingForm.HideLoading();

                (view.ApkDecoder as Decoder_APK).DecodeByFile(view, BaseAPKPath, () => {

                    PackageName = view.ApkDecoder.PackageName;
                    LauncherActivity = view.ApkDecoder.LauncherActivity;
                });

            }, ui);
        }



        public override void Install(bool isRun, Form1 view)
        {
            if (installTask == null)
            {
                LoadingForm.ShowLoading(view);

                bool _isRun = false;
                TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();
                installTask = Task.Run(() => {

                    LoadingForm.PerformStep("正在安装!");

                    _isRun = Install(view, isRun);
                    installTask = null;
                }).ContinueWith(m =>
                {

                    LoadingForm.HideLoading();

                    if (_isRun)
                    {
                        Run(view);
                    }
                }, ui);

            }

        }

        private void CopyObb()
        {
            if(string.IsNullOrEmpty(ObbPath))
            {
                return;
            }

            LoadingForm.PerformStep("拷贝OBB!");

            var cmd = $"adb push \"{ObbPath}\" /sdcard/{ObbInstallPath}";
            var error = "";
            var result = CmdTools.Exec(cmd, ref error);

            MessageBox.Show(error);
        }


        private bool Install(Form1 view, bool isRun)
        {
            var cmd = "";
            if(apksPath.Count==1)
            {
                cmd = $"install \"{apksPath[0]}\"";
            }
            else
            {
                cmd = $"install-multiple";
                for (int i = 0; i < apksPath.Count; i++)
                {
                    cmd += $" \"{apksPath[i]}\"";
                }
            }
            var error = "";
            var result = CmdTools.ExecAdb(cmd, ref error);

            if (result.Contains("Success"))
            {
                CopyObb();

                if (!isRun)
                {
                    MessageBox.Show("安装成功!");
                }
                return isRun;
            }
            else
            {
                MessageBox.Show(result);
            }
            return false;
        }

        private void ParseManifest()
        {
            apksPath.Clear();

            var jsonData = LitJson.JsonMapper.ToObject(File.ReadAllText(manifestPath));
            var jsonDataApks = jsonData["split_apks"];
            for (int i = 0; i < jsonDataApks.Count; i++)
            {
                var t = jsonDataApks[i];
                string path = Path.Combine(CachePath, t["file"].ToString());
                apksPath.Add(path);

                if (t["id"].ToString().Equals("base"))
                {
                    BaseAPKPath = path;
                }
            }

            ObbInstallPath = ObbPath = "";
            if (jsonData.ContainsKey("expansions"))
            {
                var obbs = jsonData["expansions"];
                obbs = obbs[0];

                ObbPath = Path.Combine(CachePath, obbs["file"].ToString());
                ObbInstallPath = obbs["install_path"].ToString();
            }
            
        }
    }
}
