namespace Sample.Data.Samples
{
    partial class SearchSample
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConSearch1 = new System.Windows.Forms.Button();
            this.btnConSearch = new System.Windows.Forms.Button();
            this.btnFindAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearchEx = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.varPager = new System.Windows.Forms.TextBox();
            this.btnPSearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.varClassName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.varTeacher = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.varTeacher);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.varClassName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnConSearch1);
            this.panel1.Controls.Add(this.btnConSearch);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnFindAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 84);
            this.panel1.TabIndex = 0;
            // 
            // btnConSearch1
            // 
            this.btnConSearch1.Location = new System.Drawing.Point(510, 48);
            this.btnConSearch1.Name = "btnConSearch1";
            this.btnConSearch1.Size = new System.Drawing.Size(75, 23);
            this.btnConSearch1.TabIndex = 7;
            this.btnConSearch1.Text = "不等于查询";
            this.btnConSearch1.UseVisualStyleBackColor = true;
            this.btnConSearch1.Click += new System.EventHandler(this.btnConSearch1_Click);
            // 
            // btnConSearch
            // 
            this.btnConSearch.Location = new System.Drawing.Point(386, 12);
            this.btnConSearch.Name = "btnConSearch";
            this.btnConSearch.Size = new System.Drawing.Size(75, 23);
            this.btnConSearch.TabIndex = 5;
            this.btnConSearch.Text = "精确查询";
            this.btnConSearch.UseVisualStyleBackColor = true;
            this.btnConSearch.Click += new System.EventHandler(this.btnConSearch_Click);
            // 
            // btnFindAll
            // 
            this.btnFindAll.Location = new System.Drawing.Point(429, 48);
            this.btnFindAll.Name = "btnFindAll";
            this.btnFindAll.Size = new System.Drawing.Size(75, 23);
            this.btnFindAll.TabIndex = 0;
            this.btnFindAll.Text = "查询全部";
            this.btnFindAll.UseVisualStyleBackColor = true;
            this.btnFindAll.Click += new System.EventHandler(this.btnFindAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.varPager);
            this.groupBox1.Controls.Add(this.btnPSearch);
            this.groupBox1.Location = new System.Drawing.Point(591, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 43);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // btnSearchEx
            // 
            this.btnSearchEx.Location = new System.Drawing.Point(696, 426);
            this.btnSearchEx.Name = "btnSearchEx";
            this.btnSearchEx.Size = new System.Drawing.Size(75, 23);
            this.btnSearchEx.TabIndex = 1;
            this.btnSearchEx.Text = "跨库查询";
            this.btnSearchEx.UseVisualStyleBackColor = true;
            this.btnSearchEx.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSearchEx);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 84);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(774, 452);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(774, 452);
            this.dataGridView1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "页";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "第";
            // 
            // varPager
            // 
            this.varPager.Location = new System.Drawing.Point(29, 15);
            this.varPager.Name = "varPager";
            this.varPager.Size = new System.Drawing.Size(47, 21);
            this.varPager.TabIndex = 6;
            this.varPager.Text = "1";
            // 
            // btnPSearch
            // 
            this.btnPSearch.Location = new System.Drawing.Point(95, 13);
            this.btnPSearch.Name = "btnPSearch";
            this.btnPSearch.Size = new System.Drawing.Size(75, 23);
            this.btnPSearch.TabIndex = 5;
            this.btnPSearch.Text = "分页查询";
            this.btnPSearch.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "班级名称:";
            // 
            // varClassName
            // 
            this.varClassName.Location = new System.Drawing.Point(92, 14);
            this.varClassName.Name = "varClassName";
            this.varClassName.Size = new System.Drawing.Size(100, 21);
            this.varClassName.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(215, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "班主任:";
            // 
            // varTeacher
            // 
            this.varTeacher.Location = new System.Drawing.Point(268, 14);
            this.varTeacher.Name = "varTeacher";
            this.varTeacher.Size = new System.Drawing.Size(100, 21);
            this.varTeacher.TabIndex = 11;
            // 
            // SearchSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 536);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "SearchSample";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询示例";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnFindAll;
        private System.Windows.Forms.Button btnSearchEx;
        private System.Windows.Forms.Button btnConSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConSearch1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox varPager;
        private System.Windows.Forms.Button btnPSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox varClassName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox varTeacher;
    }
}