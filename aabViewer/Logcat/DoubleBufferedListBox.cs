using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aabViewer.Logcat
{

    public class DoubleBufferedListBox : ListView
    {
        public int TopIndex;

        private ContextMenuStrip contextMenu;
        private ListViewItem contextMenuItem;
        public DoubleBufferedListBox()
        {
            View = View.Details;
            FullRowSelect = true;
            GridLines = true;

            // 添加列（若使用 View.Details 模式）
            Columns.Add("Time", 100);
            Columns.Add("PID", 100);
            Columns.Add("TID", 100);
            Columns.Add("LogLevel", 50);
            Columns.Add("TAG", 100);
            Columns.Add("Message", 600);

            // 启用双缓冲
            this.SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, true);
            // 更新样式
            this.UpdateStyles();


            contextMenu = new ContextMenuStrip();
            ToolStripMenuItem menuItem = new ToolStripMenuItem("筛选此TAG");
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);
            MouseDown += ListView1_MouseDown;

        }

        private void ListView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo hitTestInfo = HitTest(e.X, e.Y);
                if (hitTestInfo.Item != null)
                {
                    // 右键点击了某个 ListViewItem，显示上下文菜单
                    contextMenu.Show(this, e.Location);
                    contextMenuItem = hitTestInfo.Item;
                }
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(contextMenuItem.SubItems[4].Text)) return;

            var root = this.Parent as MainForm;
            root.FilterByTag(contextMenuItem.SubItems[4].Text);
        }

        public void AddLog(LogInfo log)
        {
            ListViewItem item1 = new ListViewItem(log.Time);
            item1.ForeColor = LogcatTools.GetLogTextColor(log.LogLevel);            
            item1.SubItems.Add(log.PId);
            item1.SubItems.Add(log.TId);
            item1.SubItems.Add(log.LogLevel);
            item1.SubItems.Add(log.Tag);
            item1.SubItems.Add(log.Message);
            this.Items.Add(item1);

            
        }

        public void EnsureVisibleLast()
        {
            // 确保最后一项可见
            if (this.Items.Count > 0)
            {
                this.Items[this.Items.Count - 1].EnsureVisible();
            }
        }

        public void UpdateLog(LogInfo log, int id)
        {
            ListViewItem item1 = this.Items[id];
            item1.ForeColor = LogcatTools.GetLogTextColor(log.LogLevel);
            item1.SubItems[0].Text = log.Time;
            item1.SubItems[1].Text = log.PId;
            item1.SubItems[2].Text = log.TId;
            item1.SubItems[3].Text = log.LogLevel;
            item1.SubItems[4].Text = log.Tag;
            item1.SubItems[5].Text = log.Message;
            
        }

        public void UpdateLogs(List<LogInfo> logInfos)
        {
            int len = this.Items.Count;
            
            if(Items.Count>=logInfos.Count)
            {
                int i = 0;
                for (i = 0; i < logInfos.Count; i++)
                {
                    UpdateLog(logInfos[i], i);
                }

                for (int q = Items.Count - 1; q >= i; q--)
                {
                    Items.RemoveAt(q);
                }
            }
            else
            {
                int i = 0;
                for (i = 0; i < len; i++)
                {
                    UpdateLog(logInfos[i], i);
                }

                for (int q = i; q< logInfos.Count; q++)
                {
                    AddLog(logInfos[q]);
                }
            }
        }

        public string SelectToString()
        {
            StringBuilder sb = new StringBuilder();

            for(int i=0; i<SelectedItems.Count; i++)
            {
                var item = SelectedItems[i];
                for(int q=0; q<item.SubItems.Count; q++)
                {
                    sb.Append(item.SubItems[q].Text);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
