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
        private ContextMenuStrip contextMenu;
        private ListViewItem contextMenuItem;
        private List<LogInfo> logInfos = new List<LogInfo>();

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

            // 启用虚拟模式
            this.VirtualMode = true;
            this.RetrieveVirtualItem += DoubleBufferedListView_RetrieveVirtualItem;
            this.CacheVirtualItems += DoubleBufferedListView_CacheVirtualItems;

            contextMenu = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem("添加此TAG");
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);
            

            menuItem = new ToolStripMenuItem("剔除此TAG");
            menuItem.Click += MenuItem_ClickExclue;
            contextMenu.Items.Add(menuItem);


            MouseDown += ListView1_MouseDown;
        }

        private void DoubleBufferedListView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            // 缓存虚拟项
        }

        private void DoubleBufferedListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex < logInfos.Count)
            {
                LogInfo log = logInfos[e.ItemIndex];
                ListViewItem item1 = new ListViewItem(log.Time);
                item1.ForeColor = LogcatTools.GetLogTextColor(log.LogLevel);
                item1.SubItems.Add(log.PId);
                item1.SubItems.Add(log.TId);
                item1.SubItems.Add(log.LogLevel);
                item1.SubItems.Add(log.Tag);
                item1.SubItems.Add(log.Message);
                e.Item = item1;
            }
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

        private void MenuItem_ClickExclue(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(contextMenuItem.SubItems[4].Text)) return;

            var root = this.Parent as MainForm;
            if(root.TagExlude.AddItem(contextMenuItem.SubItems[4].Text))
            {
                root.ApplyFilter();
            }            
        }

        public void AddLog(LogInfo log)
        {
            logInfos.Add(log);
            this.VirtualListSize = logInfos.Count;
        }

        public void EnsureVisibleLast()
        {
            // 确保最后一项可见
            if (this.VirtualListSize > 0)
            {
                this.EnsureVisible(this.VirtualListSize - 1);
            }
        }

        public void UpdateLog(LogInfo log, int id)
        {
            if (id < logInfos.Count)
            {
                logInfos[id] = log;
                this.RedrawItems(id, id, true);
            }
        }

        public void UpdateLogs(List<LogInfo> newLogInfos)
        {
            logInfos = newLogInfos;
            this.VirtualListSize = logInfos.Count;
            if (this.VirtualListSize > 0)
            {
                this.RedrawItems(0, logInfos.Count - 1, true);
            }
        }

        public string SelectToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < SelectedIndices.Count; i++)
            {
                int index = SelectedIndices[i];
                if (index < logInfos.Count)
                {
                    LogInfo log = logInfos[index];
                    sb.Append(log.Time);
                    sb.Append(" ");
                    sb.Append(log.PId);
                    sb.Append(" ");
                    sb.Append(log.TId);
                    sb.Append(" ");
                    sb.Append(log.LogLevel);
                    sb.Append(" ");
                    sb.Append(log.Tag);
                    sb.Append(" ");
                    sb.Append(log.Message);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        public string AllToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < logInfos.Count; i++)
            {
                LogInfo log = logInfos[i];
                sb.Append(log.Time);
                sb.Append(" ");
                sb.Append(log.PId);
                sb.Append(" ");
                sb.Append(log.TId);
                sb.Append(" ");
                sb.Append(log.LogLevel);
                sb.Append(" ");
                sb.Append(log.Tag);
                sb.Append(" ");
                sb.Append(log.Message);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 清空整个视图
        /// </summary>
        public void ClearAll()
        {
            logInfos.Clear();
            this.VirtualListSize = 0;
            if (this.VirtualListSize > 0)
            {
                this.RedrawItems(0, this.VirtualListSize - 1, true);
            }
        }
    }
}