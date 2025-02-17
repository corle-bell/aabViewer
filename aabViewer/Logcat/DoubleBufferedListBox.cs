using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aabViewer.Logcat
{
    public class DoubleBufferedListBox : ListBox
    {
        public DoubleBufferedListBox()
        {
            // 启用双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            // 更新样式
            this.UpdateStyles();

            this.DrawItem += ListBoxLogs_DrawItem;
        }

        private void ListBoxLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                Color c = Color.LightGray;
                if (e.Index % 2 == 0)
                {
                    c = Color.FromArgb(200, c);
                }
                using (SolidBrush brush = new SolidBrush(c))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }

                string log = Items[e.Index].ToString();
                Color textColor = LogcatTools.GetLogTextColor(log);
                using (SolidBrush textBrush = new SolidBrush(textColor))
                {
                    e.Graphics.DrawString(log, e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                }
            }
            e.DrawFocusRectangle();
        }
    }
}
