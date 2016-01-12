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
using NDolls.Core.Util;
using NDolls.Data.Attribute;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

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

            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            if (r.Add(m))
            {
                MessageBox.Show("添加成功");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r;
            r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            NDolls.Data.DataConfig.AllowAssociation = true;
            Model.Sys_User m = r.FindByPK("zxcayumi");
            String json = NDolls.Data.Util.DataConvert<Model.Sys_User>.EntityToJson(m);
            MessageBox.Show(json);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");

            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("UserRole","testrol", SearchType.Unequal));

            List<Model.Sys_User> list = r.FindByCondition(conditions);
            dataGridView1.DataSource = list;
            //MessageBox.Show(JsonUtil.ListToJson<Model.Sys_User>(list));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Model.Sys_User m = new Model.Sys_User();
            m.CreateTime = DateTime.Now;
            m.Password = "123123";
            //m.Status = true;
            m.UpdateTime = DateTime.Now;
            m.UserName = "admin";
            m.UserRole = "update"+DateTime.Now.ToLongTimeString();

            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            if (r.Update(m, OptType.UpdateIgnoreNull))
            {
                MessageBox.Show("修改成功");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            if (r.Delete("test635568442097031250"))
            {
                MessageBox.Show("删除成功");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
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

            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            List<Model.Sys_User> list = r.Find(m);
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

            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            String error = r.Validate(m);
            MessageBox.Show(error);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show(NDolls.Core.Util.ValidateUtil.GetPattern("EMail"));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            dataGridView1.DataSource = r.FindByPage(10, 1, null);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            List<Item> conditions = new List<Item>();
            conditions.Add(new ConditionItem("UserName", "zxcayumi,admin", SearchType.ValuesIn));
            List<Model.Sys_User> list = r.FindByCondition(conditions);

            dataGridView1.DataSource = list;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            List<Model.Sys_User> list = r.Find("UserName <> 'admin'");

            dataGridView1.DataSource = list;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Model.Test_Table m = new Model.Test_Table();
            m.key1 = "k11";
            m.key2 = "k22";
            m.memo = "测试下";

            IRepository<Model.Test_Table> r = RepositoryFactory<Model.Test_Table>.CreateRepository("Model.Test_Table");
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

            IRepository<Model.Test_Table> r = RepositoryFactory<Model.Test_Table>.CreateRepository("Model.Test_Table");
            if (r.Update(m))
            {
                MessageBox.Show("修改成功");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            IRepository<Model.Test_Table> r = RepositoryFactory<Model.Test_Table>.CreateRepository("Model.Test_Table");
            if (r.Delete(new string[] { "k1", "k2" }))
            {
                MessageBox.Show("删除成功");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            IRepository<Model.Test_Table> r = RepositoryFactory<Model.Test_Table>.CreateRepository("Model.Test_Table");
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
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
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
            IEnumerable<NDolls.Data.Attribute.CustomAttribute> list = r.GetCustomFieldsByType("gridCol");
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(NDolls.Data.DataConfig.ConnectionString);
            NDolls.Data.DataConfig.ConnectionString = "zhaoceshi";
            MessageBox.Show(NDolls.Data.DataConfig.ConnectionString);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            IRepository<Model.ECom_Work> r = RepositoryFactory<Model.ECom_Work>.CreateRepository("Model.ECom_Work");
            dataGridView1.DataSource = r.FindAll();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            Model.Test_Table m1 = new Model.Test_Table();
            m1.key1 = "k0011";
            m1.key2 = "k0021";
            m1.memo = "测试下";

            Model.Test_Table m2 = new Model.Test_Table();
            m2.key1 = "k00111";
            m2.key2 = "k00221";
            m2.memo = "测试下2";

            Model.Sys_User m3 = new Model.Sys_User();
            m3.CreateTime = DateTime.Now;
            m3.Password = "123123";
            m3.Status = true;
            m3.UpdateTime = DateTime.Now;
            m3.UserName = "testtest";// +DateTime.Now.Ticks.ToString();
            m3.UserRole = "testrole111";

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
            m3.Works = new List<Model.ECom_Work>() { work, work1 };

            DBTransaction tran = new DBTransaction();
            tran.TransactionOpen();

            IRepository<Model.Test_Table> r = RepositoryFactory<Model.Test_Table>.CreateRepository(tran);
            IRepository<Model.Sys_User> r1 = RepositoryFactory<Model.Sys_User>.CreateRepository(tran);
            
            try
            {
                r.AddOrUpdate(m1);
                r.AddOrUpdate(m2);
                r1.AddOrUpdate(m3);
                Model.Sys_User mu = new Model.Sys_User();
                mu.UserRole = "rrole";
                r1.UpdateByCondition(mu, new ConditionItem("UserName", "testtest", SearchType.Accurate));
                r1.DeleteByCondition(new ConditionItem("UserName", "test", SearchType.RightFuzzy));
                tran.TransactionCommit();
                MessageBox.Show("处理成功");
            }
            catch
            {
                tran.TransactionRollback();
                MessageBox.Show("处理失败");
            }
        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            Model.Test_Table m1 = new Model.Test_Table();
            m1.key1 = "k001";
            m1.key2 = "k002";
            m1.memo = "测试下";

            Model.Test_Table m2 = new Model.Test_Table();
            m2.key1 = "k0011";
            m2.key2 = "k0022";
            m2.memo = "测试下2";

            Model.Sys_User m3 = new Model.Sys_User();
            m3.CreateTime = DateTime.Now;
            m3.Password = "123123";
            m3.Status = true;
            m3.UpdateTime = DateTime.Now;
            m3.UserName = "test" + DateTime.Now.Ticks.ToString();
            m3.UserRole = "testrole";

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
            m3.Works = new List<Model.ECom_Work>() { work, work1 };

            List<EntityBase> list = new List<EntityBase>();
            list.Add(m1);
            list.Add(m2);
            list.Add(m3);
            if (RepositoryBase<EntityBase>.BatchSave(list))
            {
                MessageBox.Show("保存成功");
            }
        }

        private void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            Test.Model.Test_Table m = new Model.Test_Table();
            m.id = 111;
            m.key1 = "121";
            m.key2 = "2312";
            m.memo = "sdfsdf";

            IRepository<Test.Model.Test_Table> r = RepositoryFactory<Test.Model.Test_Table>.CreateRepository("Model.Test_Table");
            if (r.AddOrUpdate(m))
            {
                MessageBox.Show("保存成功");
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            List<Item> list = new List<Item>();

            ConditionItem item = new ConditionItem("CreateTime", "2015-01-15", SearchType.Greater);
            list.Add(item);

            ConditionOrGroup g = new ConditionOrGroup();
            g.AddCondition(new ConditionItem("UserName", "test", SearchType.Fuzzy));
            g.AddCondition(new ConditionItem("UserName", "test", SearchType.Unequal));

            ConditionAndGroup g1 = new ConditionAndGroup();
            g1.AddCondition(new ConditionItem("UserName", "admin", SearchType.Accurate));
            g1.AddCondition(new ConditionItem("UserName", "test", SearchType.Unequal));

            g.AddCondition(g1);

            list.Add(g);
            //list.Add(item);

            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            dataGridView1.DataSource = r.FindByCondition(list);
        }

        private void button29_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            Paper<Model.Sys_User> p = r.FindPager(4, 1, null);
            dataGridView1.DataSource = p.Result;
            MessageBox.Show(p.PageCount.ToString());
        }

        private void button30_Click(object sender, EventArgs e)
        {
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            //Model.Sys_User m = new Model.Sys_User();
            //m.CreateTime = DateTime.Now;
            //m.UserRole = "testRole";
            Dictionary<String, Object> dic = new Dictionary<string, object>();
            dic.Add("CreateTime", DateTime.Now.ToString());
            dic.Add("UserRole", "ttRole");

            List<Item> items = new List<Item>();
            items.Add(new ConditionItem("UserName", "test", SearchType.RightFuzzy));

            if (r.UpdateByCondition(dic, new ConditionItem("UserName", "test", SearchType.RightFuzzy)))
            {
                MessageBox.Show("更新成功");
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            MessageBox.Show(NDolls.Data.Util.EntityUtil.GetValueByType("","System.DateTime.Now").ToString());
        }

        private void button32_Click(object sender, EventArgs e)
        {
            //Type type = Type.GetType("System.DateTime");

            //Assembly ab = Assembly.GetAssembly(type);
            //PropertyInfo pi = type.GetProperty("Now");
            //MessageBox.Show(pi.GetValue(null,null).ToString());
            IRepository<Model.Sys_User> r = RepositoryFactory<Model.Sys_User>.CreateRepository("Model.Sys_User");
            DataTable dt = r.FindByCustom("*", "UserName LIKE 'test%'");
            MessageBox.Show(NDolls.Core.Util.JsonUtil.TableToJson(dt));
        }
    }
}
