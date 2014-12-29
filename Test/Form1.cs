using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NDolls.Core;
using NDolls.Data;
using NDolls.Data.Util;
using NDolls.Data.Entity;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(EntityUtil.GetTableName(typeof(Model.Sys_User)));

            MessageBox.Show(EntityUtil.GetPrimaryKey(EntityUtil.GetTableName(typeof(Model.Sys_User))));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Model.Sys_User m = new Model.Sys_User();
            m.CreateTime = DateTime.Now;
            m.Password = "123123";
            m.Status = true;
            m.UpdateTime = DateTime.Now;
            m.UserName = "test"+DateTime.Now.Ticks.ToString();
            m.UserRole = "testrole";

            Model.ECom_Work work = new Model.ECom_Work();
            work.Address = "zxc";
            work.City = "zxc";
            work.Province = "zxc";
            work.Classification = "sdfds";
            work.Contents = "zxc";
            work.CreateTime = DateTime.Now;
            work.District = "zxc";
            work.ExpirationDate = DateTime.Now;
            work.HTMLUrl = "zxc";
            work.Status = true;
            work.Title = "zxc";
            work.TitleColor = "zxc";
            work.UpdateTime = DateTime.Now;
            work.Province = "sdf";
            work.UserName = "zxcayumi";
            work.WorkID = Guid.NewGuid().ToString();
            work.WorkType = "放活";

            Model.ECom_Work work1 = new Model.ECom_Work();
            work1.Address = "zxc";
            work1.City = "zxc";
            work1.Classification = "sdfds";
            work1.Contents = "zxc";
            work1.CreateTime = DateTime.Now;
            work1.District = "zxc";
            work1.ExpirationDate = DateTime.Now;
            work1.HTMLUrl = "zxc";
            work1.Status = true;
            work1.Title = "zxc";
            work1.TitleColor = "zxc";
            work1.UpdateTime = DateTime.Now;
            work1.UserName = "zxcayumi";
            work1.Province = "sdf";
            work1.WorkID = Guid.NewGuid().ToString();
            work1.WorkType = "放活";
            m.Works = new List<Model.ECom_Work>() { work,work1};

            Repository<Model.Sys_User> r = new Repository<Model.Sys_User>();
            if (r.Add(m))
            {
                MessageBox.Show("添加成功");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r;
            r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            Model.Sys_User m = r.FindByPK("zxcayumi");
            String json = NDolls.Data.Util.DataConvert<Model.Sys_User>.EntityToJson(m);
            MessageBox.Show(json);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Repository<Model.Sys_User> r = new Repository<Model.Sys_User>();

            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("UserRole","testrol", SearchType.Unequal));

            List<Model.Sys_User> list = r.FindByCondition(conditions);
            dataGridView1.DataSource = list;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Model.Sys_User m = new Model.Sys_User();
            m.CreateTime = DateTime.Now;
            m.Password = "123123";
            m.Status = null;
            m.UpdateTime = DateTime.Now;
            m.UserName = "admin";
            m.UserRole = "update"+DateTime.Now.ToLongTimeString();

            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            if (r.Update(m))
            {
                MessageBox.Show("修改成功");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            if (r.Delete("test1"))
            {
                MessageBox.Show("删除成功");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Fields fs = NDolls.Data.Util.EntityUtil.GetFieldsByType(typeof(Test.Model.Sys_User));

            Model.Sys_User m = new Model.Sys_User();
            m.CreateTime = DateTime.Now;
            m.Password = "123123";
            m.Status = true;
            m.UpdateTime = DateTime.Now;
            m.UserName = "admin";
            m.UserRole = "update"+DateTime.Now.ToLongTimeString();

            MessageBox.Show(NDolls.Data.Util.EntityUtil.GetValueByField(m, "UserName").ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Model.Sys_User m = new Model.Sys_User();
            m.UserRole = "role";
            m.Status = true;

            Repository<Model.Sys_User> user = new Repository<Model.Sys_User>();
            List<Model.Sys_User> list = user.Find(m);
            dataGridView1.DataSource = list;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Model.Sys_User m = new Model.Sys_User();
            m.CreateTime = DateTime.Now;
            m.Password = "123123";
            m.Status = true;
            m.UpdateTime = DateTime.Now;
            m.UserName = "sdfsd";//验证不允许为空
            m.UserRole = "zDdk#11.com";//"update" + DateTime.Now.ToLongTimeString();//验证为邮箱格式

            Repository<Model.Sys_User> r = new Repository<Model.Sys_User>();
            String error = r.Validate(m);
            MessageBox.Show(error);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show(NDolls.Core.Util.ValidateUtil.GetPattern("EMail"));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Repository<Model.Sys_User> r = new Repository<Model.Sys_User>();
            dataGridView1.DataSource = r.FindByPage(10, 1, null);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Repository<Model.Sys_User> r = new Repository<Model.Sys_User>();
            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("UserName", "zxcayumi,admin", SearchType.ValuesIn));
            List<Model.Sys_User> list = r.FindByCondition(conditions);

            dataGridView1.DataSource = list;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Repository<Model.Sys_User> r = new Repository<Model.Sys_User>();
            List<Model.Sys_User> list = r.Find("UserName <> 'admin'");

            dataGridView1.DataSource = list;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Model.Test_Table m = new Model.Test_Table();
            m.key1 = "k11";
            m.key2 = "k22";
            m.memo = "测试下";

            Repository<Model.Test_Table> r = new Repository<Model.Test_Table>();
            if (r.Add(m))
            {
                MessageBox.Show("添加成功");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Model.Test_Table m = new Model.Test_Table();
            m.key1 = "k1";
            m.key2 = "k2";
            m.memo = "测试下"+ new Random().Next(100);

            Repository<Model.Test_Table> r = new Repository<Model.Test_Table>();
            if (r.Update(m))
            {
                MessageBox.Show("修改成功");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Repository<Model.Test_Table> r = new Repository<Model.Test_Table>();
            if (r.Delete(new string[] { "k1", "k2" }))
            {
                MessageBox.Show("删除成功");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Repository<Model.Test_Table> r = new Repository<Model.Test_Table>();
            Model.Test_Table m = r.FindByPK(new string[] { "k1", "k2" });
        }

        private void button18_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");

            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("CreateTime", "1990-1-1", SearchType.Greater));

            List<Model.Sys_User> list = r.FindByCondition(conditions);
            dataGridView1.DataSource = list;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");

            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("CreateTime", DateTime.Now.ToString(), SearchType.Lower));
            
            List<Model.Sys_User> list = r.FindByCondition(conditions);
            dataGridView1.DataSource = list;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");

            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("CreateTime", DateTime.Now.ToString(), SearchType.Lower));
            conditions.Add(new ConditionItem("CreateTime", "1990-1-1", SearchType.Greater));

            List<Model.Sys_User> list = r.FindByCondition(conditions);
            dataGridView1.DataSource = list;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            r.DeleteByCondition(new ConditionItem("UserName","test", SearchType.Fuzzy));
        }

        private void button21_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            dataGridView1.DataSource = r.FindAll();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");

            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("UserName", "abc", SearchType.Unequal));
            conditions.Add(new ConditionItem("UserName", "bcd", SearchType.Unequal));
            conditions.Add(new ConditionItem("UserName", "cde", SearchType.Unequal));

            List<Model.Sys_User> list = r.FindByCondition(conditions);
            dataGridView1.DataSource = list;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Repository<Model.Sys_User> r = new Repository<Model.Sys_User>();
            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("UserName", "abc,def", SearchType.ValuesNotIn));
            List<Model.Sys_User> list = r.FindByCondition(conditions);
            dataGridView1.DataSource = list;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("UserName", "admin", SearchType.Accurate));

            MessageBox.Show(r.Exist(conditions).ToString());
        }

        private void button26_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("CreateTime", "2015-01-01", SearchType.Lower));
            conditions.Add(new OrderItem("UserRole", OrderType.ASC));
            conditions.Add(new OrderItem("UserName", OrderType.DESC));

            dataGridView1.DataSource = r.FindByCondition(conditions);
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            List<NDolls.Data.Attribute.CustomAttribute> list = r.CustomFields;
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(NDolls.Data.DataConfig.ConnectionString);
            NDolls.Data.DataConfig.ConnectionString = "zhaoceshi";
            MessageBox.Show(NDolls.Data.DataConfig.ConnectionString);
        }
    }
}
