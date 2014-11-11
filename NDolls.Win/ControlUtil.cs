using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace NDolls.Win
{
    public class ControlUtil
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
    }
}
