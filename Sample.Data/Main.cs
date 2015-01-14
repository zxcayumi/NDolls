using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sample.Data
{
    public partial class Main : Form
    {
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
    }
}
