using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aabViewer
{
    public class UCheckComboBoxItem : Object
    {
        public int Id;
        public string Name;

        public override string ToString() 
        {
            return Name;
        }
    }
    public partial class UCheckComboBox : ComboBox
    {
        public event EventHandler OnLeave;

        private string valueProperty;
        private string displayProperty;
        //全选
        private LinkLabel selectAll;
        //全清
        private LinkLabel selectClean;
        //用于模拟键盘输入
        [DllImport("user32.dll")]
        private static extern void keybd_event(
            byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //显示控件
        //Control Control;
        //下拉面板
        private Panel panel;

        //绘制面板用变量
        //光标前一位置
        private Point pPoint;
        //光标当前位置
        private Point cPoint;
        //鼠标是否已按下
        private bool isMouseDown = false;

        //关闭下拉时光标是否在ComboBox上
        private bool isCursorOnComboBox = false;

        [Browsable(false)]
        private CheckedListBox CheckedListBox { get; set; }

        public CheckedListBox ListBox
        {
            get => CheckedListBox;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="valueMemberName"></param>
        /// <param name="displayMemberName"></param>
        public void BindingDataList<T>(List<T> t, string valueMemberName, string displayMemberName) where T : new()
        {
            valueProperty = valueMemberName;
            displayProperty = displayMemberName;
            //BindingSource bs = new BindingSource();
            //bs.DataSource = t;
            //this.CheckedListBox.DataSource = bs;
            ShowText("");
            this.CheckedListBox.Items.Clear();
            if (t != null)
            {
                foreach (var item in t)
                {
                    this.CheckedListBox.Items.Add(item, false);
                }
            }
            this.CheckedListBox.DisplayMember = displayMemberName;
            this.CheckedListBox.ValueMember = valueMemberName;
            //int height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;//这是主屏幕的，不是当前软件运行的屏幕
            int sheight = Screen.GetBounds(this).Height;//当前软件运行的屏幕，但没有减任务栏
            if (t != null)
            {
                this.panel.Size = new System.Drawing.Size(this.panel.Width, Math.Min(t.Count * 18 + 31, sheight - 60));
            }
        }

        public bool AddItem(string _item, bool isChecked=true, string valueMemberName="Id", string displayMemberName="Name")
        {
            valueProperty = valueMemberName;
            displayProperty = displayMemberName;

            ShowText("");
            foreach (var item in this.CheckedListBox.Items)
            {
                var t = (UCheckComboBoxItem)item;
                if (t.Name.Equals(_item))
                {
                    return false;
                }
            }
            this.CheckedListBox.Items.Add(new UCheckComboBoxItem { Id = CheckedListBox.Items.Count-1, Name = _item }, isChecked);
            this.CheckedListBox.DisplayMember = displayMemberName;
            this.CheckedListBox.ValueMember = valueMemberName;
            //int height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;//这是主屏幕的，不是当前软件运行的屏幕
            int sheight = Screen.GetBounds(this).Height;//当前软件运行的屏幕，但没有减任务栏
            this.panel.Size = new System.Drawing.Size(this.panel.Width, Math.Min(CheckedListBox.Items.Count * 18 + 31, sheight - 60));
            return true;
        }

        /// <summary>
        /// 获取选中项的文本列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedTexts()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < CheckedListBox.Items.Count; i++)
            {
                if (CheckedListBox.GetItemChecked(i))
                {
                    string text = CheckedListBox.Items[i].GetType().GetProperty(displayProperty).GetValue(CheckedListBox.Items[i]).ToString();
                    result.Add(text);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取选中项的值列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedValues()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < CheckedListBox.Items.Count; i++)
            {
                if (CheckedListBox.GetItemChecked(i))
                {
                    string value = CheckedListBox.Items[i].GetType().GetProperty(valueProperty).GetValue(CheckedListBox.Items[i]).ToString();
                    result.Add(value);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取选中项的指定字段列表
        /// </summary>
        /// <returns></returns>
        public List<object> GetSelectedObject()
        {
            List<object> result = new List<object>();
            for (int i = 0; i < CheckedListBox.Items.Count; i++)
            {
                if (CheckedListBox.GetItemChecked(i))
                {
                    object value = CheckedListBox.Items[i];
                    result.Add(value);
                }
            }
            return result;
        }

        /// <summary>
        /// 全选
        /// </summary>
        public void CheckAll()
        {
            SelectAll_LinkClicked(null, null);
        }

        /// <summary>
        /// 全清
        /// </summary>
        public void UnCheckAll()
        {
            SelectClean_LinkClicked(null, null);
        }

        public UCheckComboBox()
        {
            InitializeComponent();

            //设置下拉样式为DropDownList，不能手动输入
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            //绘制下拉面板
            this.DrawPanel();
        }

        public UCheckComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            //设置下拉样式为DropDownList，不能手动输入
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            //绘制下拉面板
            this.DrawPanel();
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (isCursorOnComboBox)
            {
                isCursorOnComboBox = false;
                //模拟Enter键，取消掉下拉状态
                //keybd_event(0xD, 0, 0, 0);
                //keybd_event(0xD, 0, 0x0002, 0);
            }
            else
            {
                //创建下拉窗
                ToolStripControlHost toolStripControlHost = new ToolStripControlHost(this.panel);
                toolStripControlHost.BackColor = Color.White;
                HsToolStripDropDown toolStripDropDown = new HsToolStripDropDown();
                //设置边框
                toolStripDropDown.BackColor = Color.White;
                toolStripControlHost.Margin = Padding.Empty;
                toolStripControlHost.Padding = Padding.Empty;
                toolStripControlHost.AutoSize = false;
                toolStripDropDown.Padding = Padding.Empty;
                //添加
                toolStripDropDown.Items.Add(toolStripControlHost);
                toolStripDropDown.Show(this, 0, this.Height);
                //设置宽度最小值
                if (this.panel.Width < this.Width)
                {
                    this.panel.Size = new System.Drawing.Size(this.Width, this.panel.Height);
                }
                //判断关闭时光标在ComboBox组件内
                toolStripDropDown.Closed += delegate (object sender, ToolStripDropDownClosedEventArgs e1)
                {
                    Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
                    this.isCursorOnComboBox = rec.Contains(this.PointToClient(Cursor.Position));

                    OnLeave?.Invoke(this, null);
                };
                //设置焦点
                toolStripDropDown.Focus();
            }
        }

        /// <summary>
        /// 绘制下拉面板
        /// </summary>
        public void DrawPanel()
        {
            this.panel = new Panel();
            this.panel.Size = new System.Drawing.Size(this.Width, 100);
            this.panel.Padding = new Padding(1, 1, 1, 13);
            this.panel.BackColor = Color.White;
            //绘制边线
            this.panel.Paint += delegate (object sender, PaintEventArgs e)
            {
                ControlPaint.DrawBorder(e.Graphics,
                               this.panel.ClientRectangle,
                               Color.DarkGray,
                               1,
                               ButtonBorderStyle.Solid,
                               Color.DarkGray,
                               1,
                               ButtonBorderStyle.Solid,
                               Color.DarkGray,
                               1,
                               ButtonBorderStyle.Solid,
                               Color.DarkGray,
                               1,
                               ButtonBorderStyle.Solid);
            };
            //使用Label实现右下角拖动按钮
            Label label = new Label();
            label.Text = "◢";
            label.Font = new Font("宋体", 9);
            label.Parent = this.panel;
            label.AutoSize = true;
            label.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            label.Location = new Point(this.panel.Location.X + this.panel.Size.Width - label.Width + 3,
                this.panel.Location.Y + this.panel.Size.Height - label.Height - 1);
            //实现缩放功能
            label.MouseDown += delegate (object sender, MouseEventArgs e1)
            {
                this.pPoint = Cursor.Position;
                this.isMouseDown = true;
            };
            label.MouseLeave += delegate (object sender, EventArgs e1)
            {
                this.isMouseDown = false;
            };
            label.MouseMove += delegate (object sender, MouseEventArgs e1)
            {
                this.cPoint = Cursor.Position;
                if (e1.Button == MouseButtons.Left && isMouseDown)
                {
                    this.panel.Height = Math.Max(this.panel.Height + cPoint.Y - pPoint.Y, 23);
                    this.panel.Width = Math.Max(this.panel.Width + cPoint.X - pPoint.X, this.Width);
                    pPoint = Cursor.Position;
                }
                else
                {
                    label.Cursor = Cursors.SizeNWSE;
                }
            };
            //全选
            selectAll = new LinkLabel();
            selectAll.Text = "全选";
            selectAll.Parent = this.panel;
            selectAll.AutoSize = true;
            selectAll.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            selectAll.Location = new Point(this.panel.Location.X + 1, 1);
            selectAll.LinkClicked += SelectAll_LinkClicked;
            //全清
            selectClean = new LinkLabel();
            selectClean.Text = "全清";
            selectClean.Parent = this.panel;
            selectClean.AutoSize = true;
            selectClean.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            selectClean.Location = new Point(this.panel.Location.X + selectAll.Width + 1, 1);
            selectClean.LinkClicked += SelectClean_LinkClicked;
            //必须要是Dock=Fill的，否则第一次拉出来是透明的，只能再嵌套一层panel
            Panel checkPanel = new Panel();
            checkPanel.BackColor = Color.White;
            //checkPanel.Location = new Point(0, 0);
            checkPanel.Dock = DockStyle.Fill;
            checkPanel.Parent = this.panel;

            CheckedListBox = new CheckedListBox();
            CheckedListBox.CheckOnClick = true;
            CheckedListBox.BorderStyle = BorderStyle.None;
            CheckedListBox.IntegralHeight = true;
            CheckedListBox.ItemCheck += delegate (object sender, ItemCheckEventArgs e)
            {
                String text = "";
                for (int i = 0; i < CheckedListBox.Items.Count; i++)
                {
                    //使用异或特殊处理当前正在check的条目
                    if ((i == e.Index) != CheckedListBox.GetItemChecked(i))
                    {
                        //text += CheckedListBox.Items[i].ToString() + ",";
                        text += (CheckedListBox.Items[i] as UCheckComboBoxItem).Name + ",";
                    }
                }
                text = text.Substring(0, Math.Max(text.Length - 1, 0));
                //显示所有内容
                ShowText(text);
            };
            CheckedListBox.Location = new Point(0, 20);
            CheckedListBox.Size = new Size(this.Width, 100 - 15);
            CheckedListBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);
            CheckedListBox.Parent = checkPanel;
            //将下拉高度设为1,实现隐藏效果
            this.DropDownHeight = 1;
        }
        //全清
        private void SelectClean_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < CheckedListBox.Items.Count; i++)
            {
                CheckedListBox.SetItemChecked(i, false);
            }
        }
        //全选
        private void SelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < CheckedListBox.Items.Count; i++)
            {
                CheckedListBox.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="Text">信息内容</param>
        private void ShowText(String Text)
        {
            //当DropDownStyle = DropDownList时不能直接对Text赋值
            this.Items.Clear();
            this.Items.Add(Text);
            this.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// 重写ToolStripDropDown
    /// 使用双缓存减少闪烁
    /// </summary>
    class HsToolStripDropDown : ToolStripDropDown
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;//双缓存
                return cp;
            }
        }
    }
}
