using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace NDolls.Forms
{
    public class WebUtil
    {
        /// <summary>
        /// 根据WinForm窗体自动组装Model实体类
        /// </summary>
        /// <param name="container">Winform窗体容器控件</param>
        /// <param name="model">实体类对象</param>
        /// <param name="prefix">获取数据控件的命名前缀（为空则默认var作为前缀）</param>
        public static void GetModel(HttpRequest container, Object model)
        {
            if (model == null)
                return;

            Type type = model.GetType();
            string val = "";
            foreach (PropertyInfo pi in type.GetProperties())
            {
                try
                {
                    if (pi.PropertyType.ToString().Contains("System.DateTime"))
                    {
                        DateTime d = DateTime.Parse(container[pi.Name]);
                        if (d.Year >= 1900)
                        {
                            try
                            {
                                pi.SetValue(model, (DateTime?)DateTime.ParseExact(container[pi.Name], "yyyy-MM-dd HH:mm:ss", null), null);
                            }
                            catch
                            {
                                pi.SetValue(model, d.ToString(), null);
                            }
                        }
                    }
                    else if (pi.PropertyType.ToString().ToLower().Contains("int"))
                    {
                        val = container[pi.Name];
                        pi.SetValue(model, Convert.ToInt32(val), null);
                    }
                    else if (pi.PropertyType.ToString().ToLower().Contains("decimal") || pi.PropertyType.ToString().ToLower().Contains("float"))
                    {
                        val = container[pi.Name];
                        pi.SetValue(model, Convert.ToDecimal(val), null);
                    }
                    else if (pi.PropertyType.ToString().ToLower().Contains("double"))
                    {
                        val = container[pi.Name];
                        pi.SetValue(model, Convert.ToDouble(val), null);
                    }
                    else if (pi.PropertyType.ToString().ToLower().Contains("bool"))
                    {
                        val = container[pi.Name];
                        pi.SetValue(model, Boolean.Parse(val), null);
                    }
                    else if (pi.PropertyType.ToString().ToLower().Contains("guid"))
                    {
                        val = container[pi.Name];
                        pi.SetValue(model, new Guid(val), null);
                    }
                    else
                    {
                        val = container[pi.Name];
                        if (val == "System.Data.DataRowView")
                            val = "";
                        if(!String.IsNullOrEmpty(val)) pi.SetValue(model, val, null);
                    }
                }
                catch
                { }
            }
        }
    }
}
