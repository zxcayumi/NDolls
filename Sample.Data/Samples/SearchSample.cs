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
        IRepository<Model.T_Class> r = RepositoryFactory<Model.T_Class>.CreateRepository("Model.T_Class");

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
