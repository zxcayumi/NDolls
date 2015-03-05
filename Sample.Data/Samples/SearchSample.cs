using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NDolls.Data;
using NDolls.Data.Entity;

namespace Sample.Data.Samples
{
    public partial class SearchSample : Form
    {
        IRepository<Model.T_Class> r = RepositoryFactory<Model.T_Class>.CreateRepository("Sample.Data.Model.T_Class");
        IRepository<Sample.Data.Model.Sys_User> ur = RepositoryFactory<Sample.Data.Model.Sys_User>.CreateRepository("Sample.Data.Model.Sys_User");

        public SearchSample()
        {
            InitializeComponent();
        }

        private void btnFindAll_Click(object sender, EventArgs e)
        {            
            dataGridView1.DataSource = r.FindAll();
        }

        private void btnPSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = r.FindByPage(10, int.Parse(varPager.Text), null);
        }

        private void btnConSearch_Click(object sender, EventArgs e)
        {
            List<Item> list = new List<Item>();
            ConditionItem item = new ConditionItem("ClassType", "帅哥班", SearchType.Accurate);//精确查询
            dataGridView1.DataSource = r.FindByCondition(item);
        }
    }
}
