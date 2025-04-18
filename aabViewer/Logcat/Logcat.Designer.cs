using aabViewer.Logcat;

namespace aabViewer.Logcat
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonStart = new System.Windows.Forms.Button();
            this.comboBoxTypeFilter = new System.Windows.Forms.ComboBox();
            this.textBoxFullLog = new System.Windows.Forms.TextBox();
            this.buttonSaveSelected = new System.Windows.Forms.Button();
            this.buttonSaveAll = new System.Windows.Forms.Button();
            this.buttonClearLogs = new System.Windows.Forms.Button();
            this.comboBoxProcess = new System.Windows.Forms.ComboBox();
            this.buttonRefreshProcessList = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBoxLogs = new aabViewer.Logcat.DoubleBufferedListBox();
            this.textBoxStringFilter = new aabViewer.Logcat.DelayTextBox();
            this.tagFilterFilter = new aabViewer.UCheckComboBox(this.components);
            this.tagExludeFilter = new aabViewer.UCheckComboBox(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 7);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(79, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "开始读取";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // comboBoxTypeFilter
            // 
            this.comboBoxTypeFilter.FormattingEnabled = true;
            this.comboBoxTypeFilter.Items.AddRange(new object[] {
            "",
            "V",
            "D",
            "I",
            "W",
            "E"});
            this.comboBoxTypeFilter.Location = new System.Drawing.Point(538, 7);
            this.comboBoxTypeFilter.Name = "comboBoxTypeFilter";
            this.comboBoxTypeFilter.Size = new System.Drawing.Size(98, 20);
            this.comboBoxTypeFilter.TabIndex = 3;
            // 
            // textBoxFullLog
            // 
            this.textBoxFullLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFullLog.Location = new System.Drawing.Point(12, 386);
            this.textBoxFullLog.Multiline = true;
            this.textBoxFullLog.Name = "textBoxFullLog";
            this.textBoxFullLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFullLog.Size = new System.Drawing.Size(928, 91);
            this.textBoxFullLog.TabIndex = 5;
            // 
            // buttonSaveSelected
            // 
            this.buttonSaveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveSelected.Location = new System.Drawing.Point(114, 483);
            this.buttonSaveSelected.Name = "buttonSaveSelected";
            this.buttonSaveSelected.Size = new System.Drawing.Size(106, 23);
            this.buttonSaveSelected.TabIndex = 6;
            this.buttonSaveSelected.Text = "保存选中日志";
            this.buttonSaveSelected.UseVisualStyleBackColor = true;
            this.buttonSaveSelected.Click += new System.EventHandler(this.buttonSaveSelected_Click);
            // 
            // buttonSaveAll
            // 
            this.buttonSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveAll.Location = new System.Drawing.Point(226, 483);
            this.buttonSaveAll.Name = "buttonSaveAll";
            this.buttonSaveAll.Size = new System.Drawing.Size(101, 23);
            this.buttonSaveAll.TabIndex = 7;
            this.buttonSaveAll.Text = "保存所有日志";
            this.buttonSaveAll.UseVisualStyleBackColor = true;
            this.buttonSaveAll.Click += new System.EventHandler(this.buttonSaveAll_Click);
            // 
            // buttonClearLogs
            // 
            this.buttonClearLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClearLogs.Location = new System.Drawing.Point(333, 483);
            this.buttonClearLogs.Name = "buttonClearLogs";
            this.buttonClearLogs.Size = new System.Drawing.Size(96, 23);
            this.buttonClearLogs.TabIndex = 8;
            this.buttonClearLogs.Text = "清理日志";
            this.buttonClearLogs.UseVisualStyleBackColor = true;
            this.buttonClearLogs.Click += new System.EventHandler(this.buttonClearLogs_Click);
            // 
            // comboBoxProcess
            // 
            this.comboBoxProcess.FormattingEnabled = true;
            this.comboBoxProcess.Location = new System.Drawing.Point(642, 7);
            this.comboBoxProcess.Name = "comboBoxProcess";
            this.comboBoxProcess.Size = new System.Drawing.Size(192, 20);
            this.comboBoxProcess.TabIndex = 9;
            this.comboBoxProcess.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // buttonRefreshProcessList
            // 
            this.buttonRefreshProcessList.Location = new System.Drawing.Point(840, 7);
            this.buttonRefreshProcessList.Name = "buttonRefreshProcessList";
            this.buttonRefreshProcessList.Size = new System.Drawing.Size(100, 23);
            this.buttonRefreshProcessList.TabIndex = 5;
            this.buttonRefreshProcessList.Text = "刷新进程列表";
            this.buttonRefreshProcessList.UseVisualStyleBackColor = true;
            this.buttonRefreshProcessList.Click += new System.EventHandler(this.buttonRefreshProcessList_ClickAsync);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(868, 490);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "自动刷新";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(435, 483);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "保存TAG配置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBoxLogs
            // 
            this.listBoxLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLogs.FullRowSelect = true;
            this.listBoxLogs.GridLines = true;
            this.listBoxLogs.HideSelection = false;
            this.listBoxLogs.Location = new System.Drawing.Point(12, 36);
            this.listBoxLogs.Name = "listBoxLogs";
            this.listBoxLogs.Size = new System.Drawing.Size(928, 344);
            this.listBoxLogs.TabIndex = 4;
            this.listBoxLogs.UseCompatibleStateImageBehavior = false;
            this.listBoxLogs.View = System.Windows.Forms.View.Details;
            this.listBoxLogs.VirtualMode = true;
            this.listBoxLogs.SelectedIndexChanged += new System.EventHandler(this.listBoxLogs_SelectedIndexChanged);
            // 
            // textBoxStringFilter
            // 
            this.textBoxStringFilter.Location = new System.Drawing.Point(378, 6);
            this.textBoxStringFilter.Name = "textBoxStringFilter";
            this.textBoxStringFilter.Size = new System.Drawing.Size(154, 21);
            this.textBoxStringFilter.TabIndex = 2;
            // 
            // tagFilterFilter
            // 
            this.tagFilterFilter.DropDownHeight = 1;
            this.tagFilterFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tagFilterFilter.IntegralHeight = false;
            this.tagFilterFilter.Location = new System.Drawing.Point(106, 6);
            this.tagFilterFilter.Name = "tagFilterFilter";
            this.tagFilterFilter.Size = new System.Drawing.Size(133, 20);
            this.tagFilterFilter.TabIndex = 1;
            // 
            // tagExludeFilter
            // 
            this.tagExludeFilter.DropDownHeight = 1;
            this.tagExludeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tagExludeFilter.IntegralHeight = false;
            this.tagExludeFilter.Location = new System.Drawing.Point(245, 6);
            this.tagExludeFilter.Name = "tagExludeFilter";
            this.tagExludeFilter.Size = new System.Drawing.Size(127, 20);
            this.tagExludeFilter.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(773, 486);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "暂停";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(12, 483);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "加载日志";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 518);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttonSaveAll);
            this.Controls.Add(this.buttonSaveSelected);
            this.Controls.Add(this.textBoxFullLog);
            this.Controls.Add(this.listBoxLogs);
            this.Controls.Add(this.comboBoxTypeFilter);
            this.Controls.Add(this.textBoxStringFilter);
            this.Controls.Add(this.tagFilterFilter);
            this.Controls.Add(this.tagExludeFilter);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonClearLogs);
            this.Controls.Add(this.comboBoxProcess);
            this.Controls.Add(this.buttonRefreshProcessList);
            this.Name = "MainForm";
            this.Text = "Android Log Viewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonRefreshProcessList;
        private System.Windows.Forms.ComboBox comboBoxProcess;
        private System.Windows.Forms.Button buttonClearLogs;
        private System.Windows.Forms.Button buttonStart;
        private UCheckComboBox tagFilterFilter;
        private DelayTextBox textBoxStringFilter;
        private UCheckComboBox tagExludeFilter;
        private System.Windows.Forms.ComboBox comboBoxTypeFilter;
        private DoubleBufferedListBox listBoxLogs;
        private System.Windows.Forms.TextBox textBoxFullLog;
        private System.Windows.Forms.Button buttonSaveSelected;
        private System.Windows.Forms.Button buttonSaveAll;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}