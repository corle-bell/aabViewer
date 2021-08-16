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
            this.btn_base_hash = new System.Windows.Forms.Button();
            this.btn_sha1 = new System.Windows.Forms.Button();
            this.btn_select_key = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.text_aab_path = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_file_select
            // 
            this.button_file_select.Location = new System.Drawing.Point(746, 414);
            this.button_file_select.Name = "button_file_select";
            this.button_file_select.Size = new System.Drawing.Size(104, 31);
            this.button_file_select.TabIndex = 0;
            this.button_file_select.Text = "选择文件";
            this.button_file_select.UseVisualStyleBackColor = true;
            this.button_file_select.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_install
            // 
            this.btn_install.Location = new System.Drawing.Point(12, 478);
            this.btn_install.Name = "btn_install";
            this.btn_install.Size = new System.Drawing.Size(104, 31);
            this.btn_install.TabIndex = 2;
            this.btn_install.Text = "安装";
            this.btn_install.UseVisualStyleBackColor = true;
            this.btn_install.Click += new System.EventHandler(this.btn_install_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.SeaShell;
            this.flowLayoutPanel2.Controls.Add(this.panel9);
            this.flowLayoutPanel2.Controls.Add(this.panel10);
            this.flowLayoutPanel2.Controls.Add(this.panel11);
            this.flowLayoutPanel2.Controls.Add(this.panel12);
            this.flowLayoutPanel2.Controls.Add(this.panel13);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(12, 515);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(848, 164);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // panel9
            // 
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
            this.label9.Location = new System.Drawing.Point(3, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 19);
            this.label9.TabIndex = 1;
            this.label9.Text = "别名";
            // 
            // panel10
            // 
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
            this.label10.Location = new System.Drawing.Point(3, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 19);
            this.label10.TabIndex = 1;
            this.label10.Text = "别名密码";
            // 
            // panel11
            // 
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
            this.label11.Location = new System.Drawing.Point(3, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 19);
            this.label11.TabIndex = 1;
            this.label11.Text = "密码";
            // 
            // panel12
            // 
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
            this.label12.Location = new System.Drawing.Point(3, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 19);
            this.label12.TabIndex = 1;
            this.label12.Text = "签名信息";
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.text_key_path);
            this.panel13.Controls.Add(this.label13);
            this.panel13.Location = new System.Drawing.Point(3, 99);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(773, 42);
            this.panel13.TabIndex = 4;
            // 
            // text_key_path
            // 
            this.text_key_path.Location = new System.Drawing.Point(94, 11);
            this.text_key_path.Name = "text_key_path";
            this.text_key_path.Size = new System.Drawing.Size(670, 21);
            this.text_key_path.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(3, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 19);
            this.label13.TabIndex = 1;
            this.label13.Text = "文件路径";
            // 
            // btn_base_hash
            // 
            this.btn_base_hash.Location = new System.Drawing.Point(132, 478);
            this.btn_base_hash.Name = "btn_base_hash";
            this.btn_base_hash.Size = new System.Drawing.Size(104, 31);
            this.btn_base_hash.TabIndex = 4;
            this.btn_base_hash.Text = "Base64哈希值";
            this.btn_base_hash.UseVisualStyleBackColor = true;
            this.btn_base_hash.Click += new System.EventHandler(this.btn_base_hash_Click);
            // 
            // btn_sha1
            // 
            this.btn_sha1.Location = new System.Drawing.Point(253, 478);
            this.btn_sha1.Name = "btn_sha1";
            this.btn_sha1.Size = new System.Drawing.Size(104, 31);
            this.btn_sha1.TabIndex = 5;
            this.btn_sha1.Text = "sha1指纹";
            this.btn_sha1.UseVisualStyleBackColor = true;
            this.btn_sha1.Click += new System.EventHandler(this.btn_sha1_Click);
            // 
            // btn_select_key
            // 
            this.btn_select_key.Location = new System.Drawing.Point(374, 478);
            this.btn_select_key.Name = "btn_select_key";
            this.btn_select_key.Size = new System.Drawing.Size(104, 31);
            this.btn_select_key.TabIndex = 6;
            this.btn_select_key.Text = "选择签名文件";
            this.btn_select_key.UseVisualStyleBackColor = true;
            this.btn_select_key.Click += new System.EventHandler(this.btn_select_key_Click);
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.SeaShell;
            this.panel14.Controls.Add(this.text_aab_path);
            this.panel14.Controls.Add(this.label14);
            this.panel14.Location = new System.Drawing.Point(12, 415);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(728, 42);
            this.panel14.TabIndex = 7;
            // 
            // text_aab_path
            // 
            this.text_aab_path.Location = new System.Drawing.Point(94, 11);
            this.text_aab_path.Name = "text_aab_path";
            this.text_aab_path.Size = new System.Drawing.Size(631, 21);
            this.text_aab_path.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(3, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 19);
            this.label14.TabIndex = 1;
            this.label14.Text = "aab路径";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.key,
            this.value});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(838, 397);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(876, 691);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel14);
            this.Controls.Add(this.btn_select_key);
            this.Controls.Add(this.btn_sha1);
            this.Controls.Add(this.btn_base_hash);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.btn_install);
            this.Controls.Add(this.button_file_select);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.ResumeLayout(false);

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
    }
}

