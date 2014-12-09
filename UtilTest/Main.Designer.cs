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
            this.btnDicToJson = new System.Windows.Forms.Button();
            this.varMsg = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnDicToJson
            // 
            this.btnDicToJson.Location = new System.Drawing.Point(418, 12);
            this.btnDicToJson.Name = "btnDicToJson";
            this.btnDicToJson.Size = new System.Drawing.Size(75, 23);
            this.btnDicToJson.TabIndex = 0;
            this.btnDicToJson.Text = "DicToJson";
            this.btnDicToJson.UseVisualStyleBackColor = true;
            this.btnDicToJson.Click += new System.EventHandler(this.btnDicToJson_Click);
            // 
            // varMsg
            // 
            this.varMsg.Location = new System.Drawing.Point(12, 12);
            this.varMsg.Name = "varMsg";
            this.varMsg.Size = new System.Drawing.Size(400, 96);
            this.varMsg.TabIndex = 1;
            this.varMsg.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 395);
            this.Controls.Add(this.varMsg);
            this.Controls.Add(this.btnDicToJson);
            this.Name = "Main";
            this.Text = "功能类测试";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDicToJson;
        private System.Windows.Forms.RichTextBox varMsg;
    }
}

