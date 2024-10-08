namespace aabViewer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_file_select = new System.Windows.Forms.Button();
            this.btn_install = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.text_alias = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.text_key_pass = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.text_pass = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.text_hash_result = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.text_key_path = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btn_select_key = new System.Windows.Forms.Button();
            this.btn_base_hash = new System.Windows.Forms.Button();
            this.btn_sha1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.text_aab_path = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_install_part = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.text_model = new System.Windows.Forms.TextBox();
            this.label_status = new System.Windows.Forms.Label();
            this.text_version = new System.Windows.Forms.TextBox();
            this.菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.环境监测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.配置说明ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.连接手机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存Key配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清理缓存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.查看LogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看缓存目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_file_select
            // 
            this.button_file_select.Location = new System.Drawing.Point(535, 8);
            this.button_file_select.Name = "button_file_select";
            this.button_file_select.Size = new System.Drawing.Size(64, 31);
            this.button_file_select.TabIndex = 0;
            this.button_file_select.Text = "选择文件";
            this.button_file_select.UseVisualStyleBackColor = true;
            this.button_file_select.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_install
            // 
            this.btn_install.Location = new System.Drawing.Point(605, 8);
            this.btn_install.Name = "btn_install";
            this.btn_install.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_install.Size = new System.Drawing.Size(104, 31);
            this.btn_install.TabIndex = 2;
            this.btn_install.Text = "device模式安装";
            this.btn_install.UseVisualStyleBackColor = true;
            this.btn_install.Click += new System.EventHandler(this.btn_install_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AllowDrop = true;
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.SeaShell;
            this.flowLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel2.Controls.Add(this.panel9);
            this.flowLayoutPanel2.Controls.Add(this.panel10);
            this.flowLayoutPanel2.Controls.Add(this.panel11);
            this.flowLayoutPanel2.Controls.Add(this.panel12);
            this.flowLayoutPanel2.Controls.Add(this.panel13);
            this.flowLayoutPanel2.Controls.Add(this.btn_base_hash);
            this.flowLayoutPanel2.Controls.Add(this.btn_sha1);
            this.flowLayoutPanel2.Controls.Add(this.button1);
            this.flowLayoutPanel2.Controls.Add(this.button4);
            this.flowLayoutPanel2.Controls.Add(this.comboBox1);
            this.flowLayoutPanel2.Controls.Add(this.button2);
            this.flowLayoutPanel2.Controls.Add(this.button3);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(12, 469);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(848, 235);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.text_alias);
            this.panel9.Controls.Add(this.label9);
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(241, 42);
            this.panel9.TabIndex = 2;
            // 
            // text_alias
            // 
            this.text_alias.Location = new System.Drawing.Point(56, 11);
            this.text_alias.Name = "text_alias";
            this.text_alias.Size = new System.Drawing.Size(176, 21);
            this.text_alias.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(3, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 19);
            this.label9.TabIndex = 1;
            this.label9.Text = "别名";
            // 
            // panel10
            // 
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Controls.Add(this.text_key_pass);
            this.panel10.Controls.Add(this.label10);
            this.panel10.Location = new System.Drawing.Point(250, 3);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(279, 42);
            this.panel10.TabIndex = 3;
            // 
            // text_key_pass
            // 
            this.text_key_pass.Location = new System.Drawing.Point(94, 11);
            this.text_key_pass.Name = "text_key_pass";
            this.text_key_pass.Size = new System.Drawing.Size(176, 21);
            this.text_key_pass.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(3, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 19);
            this.label10.TabIndex = 1;
            this.label10.Text = "别名密码";
            // 
            // panel11
            // 
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Controls.Add(this.text_pass);
            this.panel11.Controls.Add(this.label11);
            this.panel11.Location = new System.Drawing.Point(535, 3);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(241, 42);
            this.panel11.TabIndex = 3;
            // 
            // text_pass
            // 
            this.text_pass.Location = new System.Drawing.Point(56, 11);
            this.text_pass.Name = "text_pass";
            this.text_pass.Size = new System.Drawing.Size(176, 21);
            this.text_pass.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(3, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 19);
            this.label11.TabIndex = 1;
            this.label11.Text = "密码";
            // 
            // panel12
            // 
            this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel12.Controls.Add(this.text_hash_result);
            this.panel12.Controls.Add(this.label12);
            this.panel12.Location = new System.Drawing.Point(3, 51);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(773, 42);
            this.panel12.TabIndex = 4;
            // 
            // text_hash_result
            // 
            this.text_hash_result.Location = new System.Drawing.Point(94, 11);
            this.text_hash_result.Name = "text_hash_result";
            this.text_hash_result.Size = new System.Drawing.Size(670, 21);
            this.text_hash_result.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(3, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 19);
            this.label12.TabIndex = 1;
            this.label12.Text = "签名信息";
            // 
            // panel13
            // 
            this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel13.Controls.Add(this.text_key_path);
            this.panel13.Controls.Add(this.label13);
            this.panel13.Controls.Add(this.btn_select_key);
            this.panel13.Location = new System.Drawing.Point(3, 99);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(773, 42);
            this.panel13.TabIndex = 4;
            // 
            // text_key_path
            // 
            this.text_key_path.Location = new System.Drawing.Point(94, 11);
            this.text_key_path.Name = "text_key_path";
            this.text_key_path.Size = new System.Drawing.Size(560, 21);
            this.text_key_path.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(3, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 19);
            this.label13.TabIndex = 1;
            this.label13.Text = "文件路径";
            // 
            // btn_select_key
            // 
            this.btn_select_key.Location = new System.Drawing.Point(660, 5);
            this.btn_select_key.Name = "btn_select_key";
            this.btn_select_key.Size = new System.Drawing.Size(104, 31);
            this.btn_select_key.TabIndex = 6;
            this.btn_select_key.Text = "选择签名文件";
            this.btn_select_key.UseVisualStyleBackColor = true;
            this.btn_select_key.Click += new System.EventHandler(this.btn_select_key_Click);
            // 
            // btn_base_hash
            // 
            this.btn_base_hash.Location = new System.Drawing.Point(3, 147);
            this.btn_base_hash.Name = "btn_base_hash";
            this.btn_base_hash.Size = new System.Drawing.Size(104, 31);
            this.btn_base_hash.TabIndex = 4;
            this.btn_base_hash.Text = "Base64哈希值";
            this.btn_base_hash.UseVisualStyleBackColor = true;
            this.btn_base_hash.Click += new System.EventHandler(this.btn_base_hash_Click);
            // 
            // btn_sha1
            // 
            this.btn_sha1.Location = new System.Drawing.Point(113, 147);
            this.btn_sha1.Name = "btn_sha1";
            this.btn_sha1.Size = new System.Drawing.Size(104, 31);
            this.btn_sha1.TabIndex = 5;
            this.btn_sha1.Text = "sha1指纹";
            this.btn_sha1.UseVisualStyleBackColor = true;
            this.btn_sha1.Click += new System.EventHandler(this.btn_sha1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(223, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 31);
            this.button1.TabIndex = 19;
            this.button1.Text = "md5";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(333, 147);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(104, 31);
            this.button4.TabIndex = 25;
            this.button4.Text = "查看信息";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(443, 147);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(135, 20);
            this.comboBox1.TabIndex = 21;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.YellowGreen;
            this.button2.Location = new System.Drawing.Point(584, 147);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 31);
            this.button2.TabIndex = 23;
            this.button2.Text = "创建签名";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.OrangeRed;
            this.button3.Location = new System.Drawing.Point(677, 147);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(107, 31);
            this.button3.TabIndex = 24;
            this.button3.Text = "从列表移除签名";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.SeaShell;
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.text_aab_path);
            this.panel14.Controls.Add(this.label14);
            this.panel14.Controls.Add(this.button_file_select);
            this.panel14.Controls.Add(this.btn_install_part);
            this.panel14.Controls.Add(this.btn_install);
            this.panel14.Location = new System.Drawing.Point(12, 415);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(838, 48);
            this.panel14.TabIndex = 7;
            // 
            // text_aab_path
            // 
            this.text_aab_path.Location = new System.Drawing.Point(107, 14);
            this.text_aab_path.Name = "text_aab_path";
            this.text_aab_path.Size = new System.Drawing.Size(422, 21);
            this.text_aab_path.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(-1, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 19);
            this.label14.TabIndex = 1;
            this.label14.Text = "aab路径";
            // 
            // btn_install_part
            // 
            this.btn_install_part.Location = new System.Drawing.Point(715, 8);
            this.btn_install_part.Name = "btn_install_part";
            this.btn_install_part.Size = new System.Drawing.Size(118, 31);
            this.btn_install_part.TabIndex = 17;
            this.btn_install_part.Text = "universal模式安装";
            this.btn_install_part.UseVisualStyleBackColor = true;
            this.btn_install_part.Click += new System.EventHandler(this.btn_install_part_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.key,
            this.value});
            this.dataGridView1.Location = new System.Drawing.Point(12, 63);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(838, 340);
            this.dataGridView1.TabIndex = 8;
            // 
            // key
            // 
            this.key.HeaderText = "名称";
            this.key.Name = "key";
            this.key.Width = 300;
            // 
            // value
            // 
            this.value.HeaderText = "数值";
            this.value.Name = "value";
            this.value.Width = 300;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 14);
            this.label1.TabIndex = 12;
            this.label1.Text = "设备信息:";
            // 
            // text_model
            // 
            this.text_model.Location = new System.Drawing.Point(84, 7);
            this.text_model.Name = "text_model";
            this.text_model.Size = new System.Drawing.Size(145, 21);
            this.text_model.TabIndex = 13;
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_status.ForeColor = System.Drawing.Color.Red;
            this.label_status.Location = new System.Drawing.Point(623, 8);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(67, 14);
            this.label_status.TabIndex = 14;
            this.label_status.Text = "连接状态";
            // 
            // text_version
            // 
            this.text_version.Location = new System.Drawing.Point(236, 7);
            this.text_version.Name = "text_version";
            this.text_version.Size = new System.Drawing.Size(145, 21);
            this.text_version.TabIndex = 15;
            // 
            // 菜单ToolStripMenuItem
            // 
            this.菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.环境监测ToolStripMenuItem,
            this.配置说明ToolStripMenuItem,
            this.连接手机ToolStripMenuItem,
            this.保存Key配置ToolStripMenuItem,
            this.清理缓存ToolStripMenuItem,
            this.查看LogToolStripMenuItem,
            this.查看缓存目录ToolStripMenuItem});
            this.菜单ToolStripMenuItem.Name = "菜单ToolStripMenuItem";
            this.菜单ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.菜单ToolStripMenuItem.Text = "菜单";
            // 
            // 环境监测ToolStripMenuItem
            // 
            this.环境监测ToolStripMenuItem.Name = "环境监测ToolStripMenuItem";
            this.环境监测ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.环境监测ToolStripMenuItem.Text = "环境监测";
            this.环境监测ToolStripMenuItem.Click += new System.EventHandler(this.环境监测ToolStripMenuItem_Click);
            // 
            // 配置说明ToolStripMenuItem
            // 
            this.配置说明ToolStripMenuItem.Name = "配置说明ToolStripMenuItem";
            this.配置说明ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.配置说明ToolStripMenuItem.Text = "配置说明";
            this.配置说明ToolStripMenuItem.Click += new System.EventHandler(this.配置说明ToolStripMenuItem_Click);
            // 
            // 连接手机ToolStripMenuItem
            // 
            this.连接手机ToolStripMenuItem.Name = "连接手机ToolStripMenuItem";
            this.连接手机ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.连接手机ToolStripMenuItem.Text = "连接手机";
            this.连接手机ToolStripMenuItem.Click += new System.EventHandler(this.连接手机ToolStripMenuItem_Click);
            // 
            // 保存Key配置ToolStripMenuItem
            // 
            this.保存Key配置ToolStripMenuItem.Name = "保存Key配置ToolStripMenuItem";
            this.保存Key配置ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.保存Key配置ToolStripMenuItem.Text = "保存Key配置";
            this.保存Key配置ToolStripMenuItem.Click += new System.EventHandler(this.保存Key配置ToolStripMenuItem_Click);
            // 
            // 清理缓存ToolStripMenuItem
            // 
            this.清理缓存ToolStripMenuItem.Name = "清理缓存ToolStripMenuItem";
            this.清理缓存ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.清理缓存ToolStripMenuItem.Text = "清理缓存";
            this.清理缓存ToolStripMenuItem.Click += new System.EventHandler(this.清理缓存ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.菜单ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(876, 25);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label_status);
            this.panel1.Controls.Add(this.text_version);
            this.panel1.Controls.Add(this.text_model);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(838, 35);
            this.panel1.TabIndex = 16;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(697, 217);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 35);
            this.button5.TabIndex = 18;
            this.button5.Text = "查看manifest";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(697, 82);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 116);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // 查看LogToolStripMenuItem
            // 
            this.查看LogToolStripMenuItem.Name = "查看LogToolStripMenuItem";
            this.查看LogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.查看LogToolStripMenuItem.Text = "查看Log";
            this.查看LogToolStripMenuItem.Click += new System.EventHandler(this.查看LogToolStripMenuItem_Click);
            // 
            // 查看缓存目录ToolStripMenuItem
            // 
            this.查看缓存目录ToolStripMenuItem.Name = "查看缓存目录ToolStripMenuItem";
            this.查看缓存目录ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.查看缓存目录ToolStripMenuItem.Text = "查看缓存目录";
            this.查看缓存目录ToolStripMenuItem.Click += new System.EventHandler(this.查看缓存目录ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(876, 716);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel14);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "AABViewer";
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_file_select;
        private System.Windows.Forms.Button btn_install;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.TextBox text_alias;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.TextBox text_key_pass;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.TextBox text_pass;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.TextBox text_hash_result;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.TextBox text_key_path;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btn_base_hash;
        private System.Windows.Forms.Button btn_sha1;
        private System.Windows.Forms.Button btn_select_key;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.TextBox text_aab_path;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn key;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox text_model;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.TextBox text_version;
        private System.Windows.Forms.ToolStripMenuItem 菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 环境监测ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 配置说明ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 连接手机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_install_part;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripMenuItem 保存Key配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清理缓存ToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ToolStripMenuItem 查看LogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看缓存目录ToolStripMenuItem;
    }
}

