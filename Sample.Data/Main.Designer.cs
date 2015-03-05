namespace Sample.Data
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
            this.btnEntity = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnTransaction = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEntity
            // 
            this.btnEntity.Location = new System.Drawing.Point(32, 25);
            this.btnEntity.Name = "btnEntity";
            this.btnEntity.Size = new System.Drawing.Size(89, 23);
            this.btnEntity.TabIndex = 0;
            this.btnEntity.Text = "实体信息实例";
            this.btnEntity.UseVisualStyleBackColor = true;
            this.btnEntity.Click += new System.EventHandler(this.btnEntity_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(32, 64);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(89, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "查询示例";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(141, 64);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(89, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加示例";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(247, 64);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "修改示例";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(355, 64);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(89, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除示例";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(32, 225);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(89, 23);
            this.btnValidate.TabIndex = 5;
            this.btnValidate.Text = "验证示例";
            this.btnValidate.UseVisualStyleBackColor = true;
            // 
            // btnTransaction
            // 
            this.btnTransaction.Location = new System.Drawing.Point(32, 145);
            this.btnTransaction.Name = "btnTransaction";
            this.btnTransaction.Size = new System.Drawing.Size(89, 23);
            this.btnTransaction.TabIndex = 6;
            this.btnTransaction.Text = "事务处理示例";
            this.btnTransaction.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "多主键示例";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(32, 265);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "用户特性示例";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 381);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnTransaction);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnEntity);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库操作实例";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEntity;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnTransaction;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

