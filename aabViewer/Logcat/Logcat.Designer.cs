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
            this.buttonStart = new System.Windows.Forms.Button();
            this.tagFilterFilter = new DelayTextBox();
            this.textBoxStringFilter = new DelayTextBox();
            this.comboBoxTypeFilter = new System.Windows.Forms.ComboBox();
            this.textBoxFullLog = new System.Windows.Forms.TextBox();
            this.buttonSaveSelected = new System.Windows.Forms.Button();
            this.buttonSaveAll = new System.Windows.Forms.Button();
            this.buttonClearLogs = new System.Windows.Forms.Button();
            this.comboBoxProcess = new System.Windows.Forms.ComboBox();
            this.buttonRefreshProcessList = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.listBoxLogs = new aabViewer.Logcat.DoubleBufferedListBox();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(79, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "开始读取";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // tagFilterFilter
            // 
            this.tagFilterFilter.Location = new System.Drawing.Point(97, 12);
            this.tagFilterFilter.Name = "tagFilterFilter";
            this.tagFilterFilter.Size = new System.Drawing.Size(127, 21);
            this.tagFilterFilter.TabIndex = 1;
            // 
            // textBoxStringFilter
            // 
            this.textBoxStringFilter.Location = new System.Drawing.Point(230, 12);
            this.textBoxStringFilter.Name = "textBoxStringFilter";
            this.textBoxStringFilter.Size = new System.Drawing.Size(154, 21);
            this.textBoxStringFilter.TabIndex = 2;
            // 
            // comboBoxTypeFilter
            // 
            this.comboBoxTypeFilter.FormattingEnabled = true;
            this.comboBoxTypeFilter.Items.AddRange(new object[] {
            "",
            "D",
            "I",
            "W",
            "E"});
            this.comboBoxTypeFilter.Location = new System.Drawing.Point(390, 12);
            this.comboBoxTypeFilter.Name = "comboBoxTypeFilter";
            this.comboBoxTypeFilter.Size = new System.Drawing.Size(98, 20);
            this.comboBoxTypeFilter.TabIndex = 3;
            // 
            // textBoxFullLog
            // 
            this.textBoxFullLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFullLog.Location = new System.Drawing.Point(12, 323);
            this.textBoxFullLog.Multiline = true;
            this.textBoxFullLog.Name = "textBoxFullLog";
            this.textBoxFullLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFullLog.Size = new System.Drawing.Size(816, 100);
            this.textBoxFullLog.TabIndex = 5;
            // 
            // buttonSaveSelected
            // 
            this.buttonSaveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveSelected.Location = new System.Drawing.Point(12, 429);
            this.buttonSaveSelected.Name = "buttonSaveSelected";
            this.buttonSaveSelected.Size = new System.Drawing.Size(120, 23);
            this.buttonSaveSelected.TabIndex = 6;
            this.buttonSaveSelected.Text = "保存选中日志";
            this.buttonSaveSelected.UseVisualStyleBackColor = true;
            this.buttonSaveSelected.Click += new System.EventHandler(this.buttonSaveSelected_Click);
            // 
            // buttonSaveAll
            // 
            this.buttonSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveAll.Location = new System.Drawing.Point(138, 429);
            this.buttonSaveAll.Name = "buttonSaveAll";
            this.buttonSaveAll.Size = new System.Drawing.Size(120, 23);
            this.buttonSaveAll.TabIndex = 7;
            this.buttonSaveAll.Text = "保存所有日志";
            this.buttonSaveAll.UseVisualStyleBackColor = true;
            this.buttonSaveAll.Click += new System.EventHandler(this.buttonSaveAll_Click);
            // 
            // buttonClearLogs
            // 
            this.buttonClearLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClearLogs.Location = new System.Drawing.Point(264, 429);
            this.buttonClearLogs.Name = "buttonClearLogs";
            this.buttonClearLogs.Size = new System.Drawing.Size(120, 23);
            this.buttonClearLogs.TabIndex = 8;
            this.buttonClearLogs.Text = "清理日志";
            this.buttonClearLogs.UseVisualStyleBackColor = true;
            this.buttonClearLogs.Click += new System.EventHandler(this.buttonClearLogs_Click);
            // 
            // comboBoxProcess
            // 
            this.comboBoxProcess.FormattingEnabled = true;
            this.comboBoxProcess.Location = new System.Drawing.Point(494, 12);
            this.comboBoxProcess.Name = "comboBoxProcess";
            this.comboBoxProcess.Size = new System.Drawing.Size(192, 20);
            this.comboBoxProcess.TabIndex = 9;
            this.comboBoxProcess.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // buttonRefreshProcessList
            // 
            this.buttonRefreshProcessList.Location = new System.Drawing.Point(692, 12);
            this.buttonRefreshProcessList.Name = "buttonRefreshProcessList";
            this.buttonRefreshProcessList.Size = new System.Drawing.Size(100, 23);
            this.buttonRefreshProcessList.TabIndex = 5;
            this.buttonRefreshProcessList.Text = "刷新进程列表";
            this.buttonRefreshProcessList.UseVisualStyleBackColor = true;
            this.buttonRefreshProcessList.Click += new System.EventHandler(this.buttonRefreshProcessList_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(390, 433);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "自动刷新";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // listBoxLogs
            // 
            this.listBoxLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLogs.Location = new System.Drawing.Point(12, 41);
            this.listBoxLogs.Name = "listBoxLogs";
            this.listBoxLogs.Size = new System.Drawing.Size(816, 268);
            this.listBoxLogs.TabIndex = 4;
            this.listBoxLogs.SelectedIndexChanged += new System.EventHandler(this.listBoxLogs_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 464);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttonSaveAll);
            this.Controls.Add(this.buttonSaveSelected);
            this.Controls.Add(this.textBoxFullLog);
            this.Controls.Add(this.listBoxLogs);
            this.Controls.Add(this.comboBoxTypeFilter);
            this.Controls.Add(this.textBoxStringFilter);
            this.Controls.Add(this.tagFilterFilter);
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
        private DelayTextBox tagFilterFilter;
        private DelayTextBox textBoxStringFilter;
        private System.Windows.Forms.ComboBox comboBoxTypeFilter;
        private DoubleBufferedListBox listBoxLogs;
        private System.Windows.Forms.TextBox textBoxFullLog;
        private System.Windows.Forms.Button buttonSaveSelected;
        private System.Windows.Forms.Button buttonSaveAll;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}