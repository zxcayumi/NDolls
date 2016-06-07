using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Attribute;
using System.Reflection;
using NDolls.Data.Entity;
using NDolls.Core.Util;
using System.Text.RegularExpressions;

namespace NDolls.Data.Util
{
    /// <summary>
    /// 反射功能类
    /// </summary>
    public class EntityUtil
    {
        /// <summary>
        /// 获取实体类的数据表名信息(从EntityAttribute中获取)
        /// </summary>
        /// <param name="objType">实体类类型</param>
        /// <returns>数据表名</returns>
        public static string GetTableName(Type objType)
        {
            String tableName = "";
            EntityAttribute atr;

            if (Storage.TableNameDic.ContainsKey(objType))
            {
                tableName = Storage.TableNameDic[objType];
            }

            if (String.IsNullOrEmpty(tableName))
            {
                atr = objType.GetCustomAttributes(typeof(EntityAttribute), false)[0] as EntityAttribute;
                if (atr != null)
                {
                    tableName = atr.TableName;
                }

                Storage.TableNameDic[objType] = tableName;

                if (!String.IsNullOrEmpty(tableName))
                {
                    Storage.PrimaryKeyDic[tableName] = atr.PK;
                }
                else
                {
                    throw new Exception(objType.ToString()+"未设置EntityAttribute的TableName信息。");
                }
            }

            return tableName;
        }

        /// <summary>
        /// 根据数据表名获取对应的主键字段名
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <returns>数据表主键</returns>
        public static string GetPrimaryKey(string tableName)
        {
            string primaryKey = "";

            if (Storage.PrimaryKeyDic.ContainsKey(tableName))
            {
                primaryKey = Storage.PrimaryKeyDic[tableName];
            }
            
            return primaryKey;
        }

        /// <summary>
        /// 根据实体类类型获取对应的主键字段名
        /// </summary>
        /// <param name="tableName">实体类类型</param>
        /// <returns>数据表主键</returns>
        public static string GetPrimaryKey(Type objType)
        {
            string primaryKey = "";
            string tableName = GetTableName(objType);

            if (Storage.PrimaryKeyDic.ContainsKey(tableName))
            {
                primaryKey = Storage.PrimaryKeyDic[tableName];
            }

            return primaryKey;
        }
        
        /// <summary>
        /// 获取实体的数据字段集合
        /// </summary>
        /// <param name="obj">数据实体</param>
        /// <returns>数据实体对应的字段集合</returns>
        public static List<DataField> GetDataFields(Object obj)
        {
            List<DataField> list = new List<DataField>();
            
            PropertyInfo[] infos = obj.GetType().GetProperties();
            DataFieldAttribute atr ;
            object[] objs;
            foreach (PropertyInfo info in infos)
            {
                objs = info.GetCustomAttributes(typeof(DataFieldAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    atr = objs[0] as DataFieldAttribute;
                    list.Add(new DataField(atr.FieldName, atr.FieldType, info.GetValue(obj, null),atr.IsIdentity));
                }
            }
            
            return list;
        }

        /// <summary>
        /// 根据实体类型，获取实体字段信息（数据字段+关联对象字段）
        /// </summary>
        /// <param name="type">实体类类型</param>
        /// <returns>实体类字段信息</returns>
        public static Fields GetFieldsByType(Type type)
        {
            Fields fields = new Fields();

            if (!Storage.ClassFieldsDic.ContainsKey(type))
            {
                PropertyInfo[] infos = type.GetProperties();
                
                object[] objs;
                foreach (PropertyInfo info in infos)
                {
                    //获取数据字段
                    DataFieldAttribute atr = null;
                    objs = info.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (objs != null && objs.Length > 0)
                    {
                        atr = objs[0] as DataFieldAttribute;
                        fields.DataFields.Add(atr);
                    }

                    //获取关联对象字段
                    AssociationAttribute atr1;
                    objs = info.GetCustomAttributes(typeof(AssociationAttribute), false);
                    if (objs != null && objs.Length > 0)
                    {
                        atr1 = objs[0] as AssociationAttribute;
                        atr1.FieldName = info.Name;
                        fields.AssociationFields.Add(atr1);
                    }

                    //获取验证字段
                    objs = info.GetCustomAttributes(typeof(ValidateAttribute), false);
                    if (objs != null && objs.Length > 0)
                    {
                        foreach (ValidateAttribute obj in objs)
                        {
                            obj.FieldName = info.Name;//单独赋值(对应属性的变量名)
                            fields.ValidateFields.Add(obj);
                        }
                    }

                    //获取排序字段
                    objs = info.GetCustomAttributes(typeof(OrderAttribute), false);
                    if (objs != null && objs.Length > 0)
                    {
                        foreach (OrderAttribute obj in objs)
                        {
                            obj.FieldName = info.Name;//单独赋值(对应属性的变量名)
                            fields.OrderFields.Add(obj);
                        }
                    }

                    //获取用户自定义字段
                    objs = info.GetCustomAttributes(typeof(CustomAttribute), false);
                    if (objs != null && objs.Length > 0)
                    {
                        foreach (CustomAttribute obj in objs)
                        {
                            obj.FieldName = info.Name;//单独赋值(对应属性的变量名)
                            fields.CustomFields.Add(obj);
                        }
                    }
                }

                Storage.ClassFieldsDic.Add(type, fields);
            }
            else
            {
                fields = Storage.ClassFieldsDic[type];
            }

            return fields;
        }

        /// <summary>
        /// 将Model实体类对象转换为查询条件集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<Item> ModelToCondition<T>(T model)
        {
            List<Item> conditions = new List<Item>();

            List<DataField> fields = EntityUtil.GetDataFields(model);
            foreach (DataField field in fields)
            {
                if (field.FieldValue == null || field.FieldValue.ToString() == ""
                    || field.FieldType.ToLower().Contains("date")
                    || (field.FieldType.ToLower().Contains("uniqueidentifier") && field.FieldValue.ToString() == "00000000-0000-0000-0000-000000000000"))
                    continue;

                if (field.FieldType.ToLower().Contains("varchar"))
                {
                    conditions.Add(new ConditionItem(field.FieldName, field.FieldValue, SearchType.Fuzzy));
                }
                else if ("int,float,decimal,double,number".Contains(field.FieldType.ToLower()))
                {
                    continue;
                    //if ((int)field.FieldValue > 0)
                    //    conditions.Add(new ConditionItem(field.FieldName, field.FieldValue, SearchType.Lower));
                }
                else
                {
                    conditions.Add(new ConditionItem(field.FieldName, field.FieldValue, SearchType.Accurate));
                }
            }

            return conditions;
        }

        /// <summary>
        /// 实体对象验证
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>若有不符合要求的返回错误信息；若验证通过返回空字符串</returns>
        public static string ValidateEntity(EntityBase entity)
        {
            List<ValidateAttribute> list = GetFieldsByType(entity.GetType()).ValidateFields;//获取验证字段集合
            object fieldValue;
            string pattern;

            foreach (ValidateAttribute item in list)
            {
                fieldValue = EntityUtil.GetValueByField(entity, item.FieldName);

                if (!item.Nullable && (fieldValue == null || fieldValue.ToString() == ""))
                {
                    return item.FieldName + "," + item.FieldDesc + (item.FieldDesc == "" ? Messages.NullableError : "");
                }

                pattern = ValidateUtil.GetPattern(item.Expression);//尝试获取内置已存在正则表达式
                if (String.IsNullOrEmpty(pattern))//未获取到认为Entity配置的信息则为正则表达式
                    pattern = item.Expression;

                if (!String.IsNullOrEmpty(item.Expression) && 
                    !ValidateUtil.IsMatch(fieldValue.ToString(), pattern))
                {
                    return item.FieldName + "," + item.FieldDesc + "," + Messages.ExpressionError;
                }
            }

            return "";
        }

        /// <summary>
        /// 获取程序集中静态属性值
        /// </summary>
        /// <param name="sysAssembley">系统程序集</param>
        /// <param name="fieldName">静态变量名</param>
        /// <returns>静态变量值</returns>
        public static object GetValueByField(String sysAssembley, String fieldName)
        {
            Type type = Type.GetType(sysAssembley);
            PropertyInfo info = type.GetProperty(fieldName);
            if (info != null)
            {
                return info.GetValue(null, null);
            }
            else
                return null;
        }

        /// <summary>
        /// 获取对象某属性的值
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="fieldName">属性名</param>
        /// <returns>该对象属性对应的值</returns>
        public static object GetValueByField(EntityBase entity, String fieldName)
        {
            Type type = entity.GetType();
            PropertyInfo info = type.GetProperty(fieldName);
            if (info != null)
            {
                return info.GetValue(entity, null);
            }
            else
                return null;
        }

        /// <summary>
        /// 设置对象某属性的值
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="fieldName">属性名</param>
        /// <param name="fieldValue">属性需要设置的值</param>
        /// <returns>该对象属性对应的值</returns>
        public static void SetValueByField(EntityBase entity, String fieldName,Object fieldValue)
        {
            Type type = entity.GetType();
            PropertyInfo info = type.GetProperty(fieldName);
            if (info != null)
            {
                info.SetValue(entity, fieldValue, null);
            }
        }

        /// <summary>
        /// 获取类静态属性值
        /// </summary>
        /// <param name="assembleName">程序集名称</param>
        /// <param name="clsName">类FullName</param>
        /// <param name="clsProperty">属性名称</param>
        /// <returns>静态属性值</returns>
        public static object GetValueByType(String assembleName, String clsName, String clsProperty)
        {
            PropertyInfo pi;
            if (!String.IsNullOrEmpty(assembleName))
            {
                Assembly a = Assembly.Load(assembleName);
                pi = a.CreateInstance(clsName).GetType().GetProperty(clsProperty);
            }
            else
            {
                pi = Type.GetType(clsName).GetProperty(clsProperty);
            }
            
            return pi.GetValue(null, null).ToString(); 
        }

        /// <summary>
        /// 获取方法
        /// </summary>
        /// <param name="assembleName">程序集名称</param>
        /// <param name="clsName">类FullName</param>
        /// <param name="methodName">方法名称</param>
        /// <returns>方法对象</returns>
        public static MethodInfo GetMethod(String assembleName, String clsName, String methodName)
        {
            Assembly assembly = Assembly.Load(assembleName);
            Type type = assembly.GetType(clsName);
            MethodInfo method = type.GetMethod(methodName);

            return method;
        }

        /// <summary>
        /// 获取类静态属性值
        /// </summary>
        /// <param name="assembleName">程序集名称</param>
        /// <param name="fullPropertyName">包含类路径的属性名</param>
        /// <returns>静态属性值</returns>
        public static object GetValueByType(String assembleName,String fullPropertyName)
        {
            String clsName = fullPropertyName.Substring(0, fullPropertyName.LastIndexOf('.'));
            String clsProperty = fullPropertyName.Substring(fullPropertyName.LastIndexOf('.') + 1);
            return GetValueByType(assembleName, clsName, clsProperty);
        }

        /// <summary>
        /// 获取某字段所属的特性集合
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="fieldName">对象字段</param>
        /// <returns>特性集合</returns>
        public static object[] GetAttributesByField<T>(EntityBase entity, String fieldName)
        {
            Type type = entity.GetType();
            PropertyInfo info = type.GetProperty(fieldName);
            if (info != null)
            {
                return info.GetCustomAttributes(typeof(T), false);
            }
            else
                return null;
        }

        /// <summary>
        /// 为主对象的关联对象赋值
        /// </summary>
        /// <param name="model">要赋值的主对象</param>
        public static void SetAssociation(Object model)
        {
            if (!NDolls.Data.DataConfig.AllowAssociation)//若未开启级联查询，返回
            {
                return;
            }

            Type type = model.GetType();
            Fields fields = GetFieldsByType(type);

            PropertyInfo info;
            Type refType;
            foreach (AssociationAttribute aField in fields.AssociationFields)
            {
                if (aField.CasType != CascadeType.ALL && aField.CasType != CascadeType.SELECT && aField.CasType != CascadeType.UNDELETE)
                    continue;

                info = type.GetProperty(aField.FieldName);
                if (aField.AssType == AssociationType.Association)//关联关系
                {
                    refType = info.PropertyType;
                }
                else//组合或聚合关系
                {
                    refType = info.PropertyType.GetGenericArguments()[0];
                }

                dynamic repository = RepositoryFactory<EntityBase>.CreateRepository(refType);//此处泛型T无实际作用
                String[] curFields = aField.CurField.Split(',');//主对象字段名集合
                String[] vals = new String[curFields.Length];
                for (int i = 0; i < curFields.Length; i++)
                {
                    try
                    {
                        vals[i] = type.GetProperty(curFields[i]).GetValue(model, null).ToString();
                    }
                    catch 
                    {}
                }

                switch (aField.AssType)
                {
                    case AssociationType.Association://关联关系(1vs1)
                        info.SetValue(model, repository.FindByPK(vals), null);
                        break;
                    case AssociationType.Aggregation://聚合关系
                    case AssociationType.Composition://组合关系
                        String[] objFields = aField.ObjField.Split(',');//目标对象字段名集合
                        List<Item> conditions = new List<Item>();
                        for (int i = 0; i < objFields.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(objFields[i]))
                            {
                                conditions.Add(new ConditionItem(objFields[i], vals[i], SearchType.Accurate));
                            }
                        }

                        //加载特殊查询项（条件项）
                        object[] objs = GetAttributesByField<AssocConditionAttribute>(model as EntityBase, aField.FieldName);
                        if (objs != null && objs.Length > 0)
                        {
                            foreach (AssocConditionAttribute obj in objs)
                            {
                                //匹配变量替换
                                String pattern = @"[\{（][\s\S]*[\}）]";

                                List<string> mlist = Regex.Matches(obj.FieldName, pattern).Cast<Match>().Select(a => a.Value).ToList();
                                foreach (String v in mlist)
                                {
                                    LoadVars(obj, (EntityBase)model, v, true);
                                }

                                if (obj.FieldValue != null)
                                {
                                    mlist = Regex.Matches(obj.FieldValue.ToString(), pattern).Cast<Match>().Select(a => a.Value).ToList();
                                    foreach (String v in mlist)
                                    {
                                        LoadVars(obj, (EntityBase)model, v, false);
                                    }
                                }

                                conditions.Add(new ConditionItem(obj.FieldName, obj.FieldValue, obj.SearchType));
                            }
                        }

                        //加载特殊查询项（排序项）
                        objs = GetAttributesByField<AssocOrderAttribute>(model as EntityBase, aField.FieldName);
                        if (objs != null && objs.Length > 0)
                        {
                            foreach (AssocOrderAttribute obj in objs)
                            {
                                conditions.Add(new OrderItem(obj.FieldName, obj.OrderType));
                            }
                        }

                        dynamic list = aField.Top <= 0 ?
                            repository.FindByCondition(conditions) : repository.FindByCondition(aField.Top, conditions);
                        info.SetValue(model, list, null);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void LoadVars(AssocConditionAttribute obj, EntityBase model, String val,Boolean isFieldName)
        {
            String[] pars = val.Trim(new char[] { '{', '}' }).Split(',');
            String vv = "";
            if (isFieldName)
            {
                vv = obj.FieldName;
            }
            else
            {
                vv = obj.FieldValue.ToString();
            }

            if (pars.Length == 1)
            {
                if (pars[0].Contains("."))//系统全局静态变量，如{System.DateTime.Now}
                {
                    vv = vv.Replace(val, GetValueByField(pars[0].Substring(0, pars[0].LastIndexOf('.')),
                        pars[0].Substring(pars[0].LastIndexOf('.') + 1)).ToString());
                }
                else//{FieldName}:当前主对象字段值替换
                {
                    vv = vv.Replace(val,
                        GetValueByField((EntityBase)model, pars[0]).ToString());
                }
            }
            else//{Assembly,Assembly.FieldName}:全局静态变量替换
            {
                vv = vv.Replace(val,
                    NDolls.Data.Util.EntityUtil.GetValueByType(pars[0], pars[1]).ToString());
            }

            if (isFieldName)
            {
                obj.FieldName = vv;
            }
            else
            {
                obj.FieldValue = vv;
            }
        }

    }
}
