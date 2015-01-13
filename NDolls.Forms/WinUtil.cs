using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Reflection;

namespace NDolls.Forms
{
    public class WinUtil
    {
        /// <summary>
        /// 将Control控件中所有的子控件初始化
        /// </summary>
        /// <param name="ctr">父控件</param>
        public static void Reset(Control ctr)
        {
            foreach (Control control in ctr.Controls)
            {
                if (control.GetType() == typeof(TextBox))
                {
                    TextBox txt = control as TextBox;
                    txt.Clear();
                }
                else if (control.GetType() == typeof(RichTextBox))
                {
                    RichTextBox rtx = control as RichTextBox;
                    rtx.Clear();
                }
                else if (control.GetType() == typeof(ComboBox))
                {
                    ComboBox cmb = control as ComboBox;
                    if (cmb.Items.Count > 0)
                        cmb.SelectedIndex = 0;
                }
                else if (control.GetType() == typeof(DateTimePicker))
                {
                    DateTimePicker dtp = control as DateTimePicker;
                    dtp.Text = DateTime.Now.ToShortDateString();
                }
                else if (control.GetType() == typeof(CheckedListBox))
                {
                    CheckedListBox list = control as CheckedListBox;
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        list.SetItemChecked(i, false);
                    }
                }
                else if (control.GetType() == typeof(CheckBox))
                {
                    CheckBox chbox = control as CheckBox;
                    chbox.Checked = false;
                }
            }
        }

        /// <summary>
        /// 初始化加载下拉框
        /// </summary>
        /// <param name="comboBox">要加载内容的下拉框</param>
        /// <param name="displayMember">显示值</param>
        /// <param name="valueMember">隐藏值</param>
        /// <param name="dataSource">数据源</param>
        public static void InitComboBox<T>(ComboBox comboBox, string displayMember, string valueMember, List<T> dataSource)
        {
            comboBox.DataSource = dataSource as List<T>;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
        }

        /// <summary>
        /// 初始化加载下拉框
        /// </summary>
        /// <param name="comboBox">要加载内容的下拉框</param>
        /// <param name="displayMember">显示值</param>
        /// <param name="valueMember">隐藏值</param>
        /// <param name="dataSource">数据源</param>
        public static void InitComboBox(ComboBox comboBox, string displayMember, string valueMember, DataTable dataSource)
        {
            comboBox.DataSource = dataSource;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
        }

        /// <summary>
        /// 根据WinForm窗体自动组装Model实体类
        /// </summary>
        /// <param name="container">Winform窗体容器控件</param>
        /// <param name="model">实体类对象</param>
        /// <param name="prefix">获取数据控件的命名前缀（为空则默认var作为前缀）</param>
        public static void GetModel(Control container, Object model,String prefix)
        {
            if (model == null)
                return;

            if (String.IsNullOrEmpty(prefix))
                prefix = "var";

            Type type = model.GetType();
            string val = "";
            foreach (PropertyInfo pi in type.GetProperties())
            {
                try
                {
                    if (pi.PropertyType.ToString().Contains("System.DateTime"))
                    {
                        DateTime d = DateTime.Parse(container.Controls.Find(prefix + pi.Name, true)[0].Text);
                        if (d.Year >= 1900)
                            pi.SetValue(model, d.ToShortDateString(), null);
                    }
                    else if (pi.PropertyType.ToString().ToLower().Contains("int"))
                    {
                        val = container.Controls.Find(prefix + pi.Name, true)[0].Text;
                        pi.SetValue(model, Convert.ToInt32(val), null);
                    }
                    else if (pi.PropertyType.ToString().ToLower().Contains("decimal") || pi.PropertyType.ToString().ToLower().Contains("float"))
                    {
                        val = container.Controls.Find(prefix + pi.Name, true)[0].Text;
                        pi.SetValue(model, Convert.ToDecimal(val), null);
                    }
                    else if (pi.PropertyType.ToString().ToLower().Contains("double"))
                    {
                        val = container.Controls.Find(prefix + pi.Name, true)[0].Text;
                        pi.SetValue(model, Convert.ToDouble(val), null);
                    }
                    else
                    {
                        val = container.Controls.Find(prefix + pi.Name, true)[0].Text;
                        if (val == "System.Data.DataRowView")
                            val = "";
                        pi.SetValue(model, val, null);
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 自动赋值，将实体类对象中的各个属性赋值到container容器中的对应控件
        /// </summary>
        /// <param name="container">赋值的父容器</param>
        /// <param name="model">实体对象</param>
        /// <param name="prefix">获取数据控件的命名前缀（为空则默认var作为前缀）</param>
        public static void SetObject(Control container, object model, String prefix)
        {
            if (model == null)
                return;

            Type type = model.GetType();
            foreach (PropertyInfo pi in type.GetProperties())
            {
                try
                {
                    if (pi.PropertyType.ToString().Contains("System.DateTime"))
                    {
                        DateTime d = DateTime.Parse(pi.GetValue(model, null).ToString());
                        if (d.Year >= 1900)
                            container.Controls.Find(prefix + pi.Name, true)[0].Text = d.ToShortDateString();
                    }
                    else
                        container.Controls.Find(prefix + pi.Name, true)[0].Text = pi.GetValue(model, null).ToString();
                }
                catch
                { }
            }
        }

    }
}
