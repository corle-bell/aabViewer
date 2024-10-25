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

        }

        public SignCreator Init(Form1 _root)
        {
            root = _root;

            if(root.SignTemplete.Count>0)
            {
                //别名
                textBox1.Text = root.SignTemplete[0];

                //密码
                textBox2.Text = root.SignTemplete[1];

                //有效期
                textBox3.Text = root.SignTemplete[2];

                //名称
                textBox4.Text = root.SignTemplete[3];

                //组织机构
                textBox5.Text = root.SignTemplete[4];

                //组织名称
                textBox6.Text = root.SignTemplete[5];

                //城市
                textBox7.Text = root.SignTemplete[6];

                //省份
                textBox8.Text = root.SignTemplete[7];

                //国家代码
                textBox9.Text = root.SignTemplete[8];
            }

            this.checkBox1.Checked = true;

            return this;
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


            int day = int.Parse(textBox3.Text);
            int year = day * 365;

            SaveFileDialog fileDialog = new SaveFileDialog();            
            fileDialog.Filter = "签名文件(*.keystore)|*.keystore;";

            if(fileDialog.ShowDialog()==DialogResult.OK)
            {

                string path = fileDialog.FileName;
                string dname = $"\"CN={textBox4.Text},OU={textBox5.Text},O={textBox6.Text},L={textBox7.Text},ST={textBox8.Text},C={textBox9.Text}\"";
                string cmd = $"keytool -genkey -sigalg SHA1withRSA -keyalg RSA -alias {textBox1.Text} -keypass {textBox2.Text} -validity {year} -keystore {path} -storepass {textBox2.Text} -dname {dname}";


                WinformTools.Log(cmd);

                //执行指令
                string error = "";
                var ret = CmdTools.Exec(cmd, ref error);
                Console.WriteLine(cmd);

                if (!File.Exists(path))
                {
                    WinformTools.WriteLog("Create Error: " + error);
                    WinformTools.WriteLog("Create Ret: " + ret);

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

                    WinformTools.WriteLog("Create Ret: " + ret);
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
