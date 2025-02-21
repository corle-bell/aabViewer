using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aabViewer.VersionUpdate
{
    public partial class VersionUpdateForm : Form
    {
        public VersionUpdateForm()
        {
            InitializeComponent();
        }

        //YES
        private void button1_Click(object sender, EventArgs e)
        {
            WinformTools.OpenUrl(VersionUpdateChecker.DownLoadURL);
            this.Close();
        }

        //NO
        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Ignored_Ver = VersionUpdateChecker.LastVersion.ToString();
            Properties.Settings.Default.Save();
            this.Close();
        }

        public static VersionUpdateForm Create(string _text)
        {
            VersionUpdateForm form = new VersionUpdateForm();
            form.richTextBox1.Text = _text;
            form.ShowDialog();
            return form;
        }
    }
}
