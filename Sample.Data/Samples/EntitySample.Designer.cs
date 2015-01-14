namespace Sample.Data.Samples
{
    partial class EntitySample
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGetTabelName = new System.Windows.Forms.Button();
            this.varTableName = new System.Windows.Forms.TextBox();
            this.varPrimaryKey = new System.Windows.Forms.TextBox();
            this.btnGetPrimaryKey = new System.Windows.Forms.Button();
            this.varProperyValue = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetTabelName
            // 
            this.btnGetTabelName.Location = new System.Drawing.Point(34, 27);
            this.btnGetTabelName.Name = "btnGetTabelName";
            this.btnGetTabelName.Size = new System.Drawing.Size(98, 23);
            this.btnGetTabelName.TabIndex = 0;
            this.btnGetTabelName.Text = "获取表名";
            this.btnGetTabelName.UseVisualStyleBackColor = true;
            this.btnGetTabelName.Click += new System.EventHandler(this.btnGetTabelName_Click);
            // 
            // varTableName
            // 
            this.varTableName.Location = new System.Drawing.Point(138, 27);
            this.varTableName.Name = "varTableName";
            this.varTableName.Size = new System.Drawing.Size(387, 21);
            this.varTableName.TabIndex = 1;
            // 
            // varPrimaryKey
            // 
            this.varPrimaryKey.Location = new System.Drawing.Point(138, 69);
            this.varPrimaryKey.Name = "varPrimaryKey";
            this.varPrimaryKey.Size = new System.Drawing.Size(387, 21);
            this.varPrimaryKey.TabIndex = 3;
            // 
            // btnGetPrimaryKey
            // 
            this.btnGetPrimaryKey.Location = new System.Drawing.Point(34, 69);
            this.btnGetPrimaryKey.Name = "btnGetPrimaryKey";
            this.btnGetPrimaryKey.Size = new System.Drawing.Size(98, 23);
            this.btnGetPrimaryKey.TabIndex = 2;
            this.btnGetPrimaryKey.Text = "获取主键";
            this.btnGetPrimaryKey.UseVisualStyleBackColor = true;
            this.btnGetPrimaryKey.Click += new System.EventHandler(this.btnGetPrimaryKey_Click);
            // 
            // varProperyValue
            // 
            this.varProperyValue.Location = new System.Drawing.Point(138, 111);
            this.varProperyValue.Name = "varProperyValue";
            this.varProperyValue.Size = new System.Drawing.Size(387, 21);
            this.varProperyValue.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(34, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "获取对象属性值";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EntitySample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 165);
            this.Controls.Add(this.varProperyValue);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.varPrimaryKey);
            this.Controls.Add(this.btnGetPrimaryKey);
            this.Controls.Add(this.varTableName);
            this.Controls.Add(this.btnGetTabelName);
            this.MaximizeBox = false;
            this.Name = "EntitySample";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "实体对象示例";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetTabelName;
        private System.Windows.Forms.TextBox varTableName;
        private System.Windows.Forms.TextBox varPrimaryKey;
        private System.Windows.Forms.Button btnGetPrimaryKey;
        private System.Windows.Forms.TextBox varProperyValue;
        private System.Windows.Forms.Button button1;
    }
}