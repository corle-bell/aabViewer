using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aabViewer
{
    public partial class ScrollTextBox : Form
    {
        public ScrollTextBox()
        {
            InitializeComponent();

            // 创建一个新的RichTextBox控件
            RichTextBox richTextBox = richTextBox1;

            // 设置位置和大小
            richTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // 设置属性
            richTextBox.Multiline = true;
            richTextBox.ReadOnly = true;
            richTextBox.ScrollBars = RichTextBoxScrollBars.ForcedVertical;

            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            richTextBox1.Width = this.ClientRectangle.Width;
            richTextBox1.Height = this.ClientRectangle.Height;
        }

        public void Show(string _text)
        {
            richTextBox1.Text = _text;
            this.Show();
        }
    }
}
