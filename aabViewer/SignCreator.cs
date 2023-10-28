using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace aabViewer
{
    public partial class SignCreator : Form
    {
        public Form1 root;
        public SignCreator()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            //别名
            textBox1.Text = "sdf";

            //密码
            textBox2.Text = "000000";

            //有效期
            textBox3.Text = "25";

            //名称
            textBox4.Text = "sdf";

            //组织机构
            textBox5.Text = "badman";

            //组织名称
            textBox6.Text = "badman";

            //城市
            textBox7.Text = "sjz";

            //省份
            textBox8.Text = "hb";

            //国家代码
            textBox9.Text = "86";

            this.checkBox1.Checked = true;
        }

        private void SignCreator_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("参数错误!");
                return;
            }

            SaveFileDialog fileDialog = new SaveFileDialog();            
            fileDialog.Filter = "签名文件(*.keystore)|*.keystore;";

            if(fileDialog.ShowDialog()==DialogResult.OK)
            {
                string path = fileDialog.FileName;
                string dname = $"\"CN={textBox4.Text},OU={textBox5.Text},O={textBox6.Text},L={textBox7.Text},ST={textBox8.Text},C={textBox9.Text}\"";
                string cmd = $"keytool -genkey -alias {textBox1.Text} -keypass {textBox2.Text} -validity {textBox3.Text} -keystore {path} -storepass {textBox2.Text} -dname {dname}";

                WinformTools.Log(cmd);

                //执行指令
                string error = "";
                var ret = CmdTools.Exec(cmd, ref error);
                Console.WriteLine(cmd);

                if (!File.Exists(path))
                {
                    root.WriteLog("Create Error: " + error);
                    root.WriteLog("Create Ret: " + ret);

                    MessageBox.Show("生成失败!");
                    return;
                }
                else
                {
                    if(this.checkBox1.Checked)
                    {
                        root.AddKeys(textBox1.Text, path, textBox2.Text, textBox1.Text, textBox2.Text);
                        root.SaveKeyConfigs();
                    }

                    root.WriteLog("Create Ret: " + ret);
                    MessageBox.Show("生成结束!");                    
                }

            }

            
        }

        private void Writesdfsdf(System.Diagnostics.Process p, string _text)
        {
            p.StandardInput.Write(_text);
            p.StandardInput.AutoFlush = true;
            CmdTools.Enter(p);
        }

    }
}
