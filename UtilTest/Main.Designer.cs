namespace UtilTest
{
    partial class Main
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.varMsg1 = new System.Windows.Forms.RichTextBox();
            this.varMsg = new System.Windows.Forms.RichTextBox();
            this.btnDicToJson = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(722, 513);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.varMsg1);
            this.tabPage1.Controls.Add(this.varMsg);
            this.tabPage1.Controls.Add(this.btnDicToJson);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(714, 487);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "JsonUtil";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 307);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "整型验证";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // varMsg1
            // 
            this.varMsg1.Location = new System.Drawing.Point(29, 132);
            this.varMsg1.Name = "varMsg1";
            this.varMsg1.Size = new System.Drawing.Size(400, 96);
            this.varMsg1.TabIndex = 4;
            this.varMsg1.Text = "";
            // 
            // varMsg
            // 
            this.varMsg.Location = new System.Drawing.Point(29, 15);
            this.varMsg.Name = "varMsg";
            this.varMsg.Size = new System.Drawing.Size(400, 96);
            this.varMsg.TabIndex = 3;
            this.varMsg.Text = "";
            // 
            // btnDicToJson
            // 
            this.btnDicToJson.Location = new System.Drawing.Point(435, 15);
            this.btnDicToJson.Name = "btnDicToJson";
            this.btnDicToJson.Size = new System.Drawing.Size(75, 23);
            this.btnDicToJson.TabIndex = 2;
            this.btnDicToJson.Text = "DicToJson";
            this.btnDicToJson.UseVisualStyleBackColor = true;
            this.btnDicToJson.Click += new System.EventHandler(this.btnDicToJson_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(714, 487);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 280);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(125, 21);
            this.textBox1.TabIndex = 6;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 513);
            this.Controls.Add(this.tabControl1);
            this.Name = "Main";
            this.Text = "功能类测试";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox varMsg;
        private System.Windows.Forms.Button btnDicToJson;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox varMsg1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;

    }
}

