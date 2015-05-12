using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilTest
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnDicToJson_Click(object sender, EventArgs e)
        {
            Dictionary<String, object> dic = new Dictionary<string, object>();
            dic.Add("Name", "zxcayumi");
            dic.Add("Birthday", "1983-10-15");
            dic.Add("Address","文化东路88号");
            dic.Add("Courses", new List<Course>() { new Course("英语",99),new Course("数学",98),new Course("计算机网络",100) });
            dic.Add("CModel", new Course("面试", 99));
            varMsg.Text = NDolls.Core.Util.JsonUtil.DicToJson(dic);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String input = textBox1.Text;
            String pattern = NDolls.Core.Util.ValidateUtil.GetPattern("NonNegativeInt");
            bool ret = NDolls.Core.Util.ValidateUtil.IsMatch(input, pattern);
            if (ret)
            {
                MessageBox.Show("验证通过");
            }
        }
    }

    public class Course
    {
        private String courseName;

        public String CourseName
        {
            get { return courseName; }
            set { courseName = value; }
        }
        private int courseScore;

        public int CourseScore
        {
            get { return courseScore; }
            set { courseScore = value; }
        }

        public Course(String courseName, int courseScore)
        {
            this.courseName = courseName;
            this.courseScore = courseScore;
        }


    }
}
