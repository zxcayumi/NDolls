using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NDolls.Data;

namespace Sample.Data
{
    public partial class Main : Form
    {
        IRepository<Sample.Data.Model.T_Class> r = RepositoryFactory<Sample.Data.Model.T_Class>.CreateRepository("Sample.Data.Model.T_Class");

        public Main()
        {
            InitializeComponent();
        }

        private void btnEntity_Click(object sender, EventArgs e)
        {
            Samples.EntitySample frm = new Samples.EntitySample();
            frm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Samples.SearchSample frm = new Samples.SearchSample();
            frm.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Model.T_Class m = new Model.T_Class();
            m.ID = Guid.NewGuid().ToString("N");
            m.ClassName = "99级6班";
            m.ClassType = "帅哥班";
            m.CreatDate = DateTime.Now;
            m.Teacher = "zxcayumi";
            m.StuCount = DateTime.Now.Second;

            if (r.Add(m))
            {
                MessageBox.Show("添加成功");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}
