using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class UtilTest : Form
    {
        public UtilTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Name", "赵学臣");
            dic.Add("Age", "25");
            dic.Add("Sex","男");
            dic.Add("Height","1.78米");
            String json = NDolls.Core.Util.JsonUtil.ToJson(dic);
            MessageBox.Show(json);
        }
    }
}
