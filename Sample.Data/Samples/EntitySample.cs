using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NDolls.Data.Util;

namespace Sample.Data.Samples
{
    public partial class EntitySample : Form
    {
        public EntitySample()
        {
            InitializeComponent();
        }

        private void btnGetTabelName_Click(object sender, EventArgs e)
        {
            varTableName.Text = EntityUtil.GetTableName(typeof(Model.T_Class));
        }

        private void btnGetPrimaryKey_Click(object sender, EventArgs e)
        {
            varPrimaryKey.Text = EntityUtil.GetPrimaryKey(EntityUtil.GetTableName(typeof(Model.T_Class)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Model.T_Class m = new Model.T_Class();
            m.ID = Guid.NewGuid().ToString("N");
            m.ClassName = "班级名称";
            m.ClassType = "二年级一班";
            m.Teacher = "zxcayumi";
            m.CreatDate = DateTime.Now;

            varProperyValue.Text = EntityUtil.GetValueByField(m, "ClassType").ToString();
        }
    }
}
