using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NDolls.Data;

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
    }
}
