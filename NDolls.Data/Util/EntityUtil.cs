using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Attribute;
using System.Reflection;
using NDolls.Data.Entity;
using NDolls.Core.Util;

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
                    return item.FieldName + "," + item.FieldDesc + Messages.NullableError;
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
        /// 获取对象某属性的值
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="fieldName">属性名</param>
        /// <returns>该对象属性对应的值</returns>
        public static object GetValueByField(EntityBase entity, string fieldName)
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
        /// 为主对象的关联对象赋值
        /// </summary>
        /// <param name="model">要赋值的主对象</param>
        public static void SetAssociation(Object model)
        {
            Type type = model.GetType();
            Fields fields = GetFieldsByType(type);

            PropertyInfo info;
            foreach (AssociationAttribute aField in fields.AssociationFields)
            {
                if (aField.CasType != CascadeType.ALL && aField.CasType != CascadeType.SELECT && aField.CasType != CascadeType.UNDELETE)
                    continue;

                info = type.GetProperty(aField.FieldName);
                dynamic repository = RepositoryFactory<EntityBase>.CreateRepository(info.PropertyType.GetGenericArguments()[0]);//此处泛型T无实际作用
                switch (aField.AssType)
                {
                    case AssociationType.Association://关联关系
                        info.SetValue(model, repository.FindByPK(type.GetProperty(aField.RefField).GetValue(model, null).ToString()), null);
                        break;
                    case AssociationType.Aggregation://聚合关系
                    case AssociationType.Composition://组合关系
                        dynamic list =
                            repository.FindByCondition(new List<Item> { new ConditionItem(aField.RefField, GetValueByField((EntityBase)model, aField.RefField), SearchType.Accurate) });
                        info.SetValue(model, list, null);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 持久化主对象及其的关联对象信息
        /// </summary>
        /// <param name="model">操作主对象</param>
        /// <param name="filedName">关联对象集合</param>
        //public static bool Persist(OptEntity model,List<AssociationAttribute> associations)
        //{
        //    Type type = model.Entity.GetType();
        //    PropertyInfo info;
        //    dynamic obj;
        //    dynamic repository;
        //    OptType optType = OptType.Save;

        //    List<OptEntity> entities = new List<OptEntity>();//实体对象集合
        //    entities.Add(model);//加入主对象

        //    foreach (AssociationAttribute item in associations)
        //    {
        //        info = type.GetProperty(item.FieldName);
        //        repository =
        //            RepositoryFactory<EntityBase>.CreateRepository(info.PropertyType.GetGenericArguments()[0]);//此处泛型T无实际作用

        //        obj = info.GetValue(model.Entity, null);
        //        if (obj == null)
        //            continue;

        //        switch (item.AssType)
        //        {
        //            case AssociationType.Association://关联关系
        //                //控制级联类别
        //                if (item.CasType == CascadeType.SAVE || item.CasType == CascadeType.UNDELETE || item.CasType == CascadeType.ALL)
        //                {
        //                    if (repository.Exist(obj))
        //                        optType = OptType.Update;
        //                    else
        //                        optType = OptType.Create;
        //                }
        //                else if (item.CasType == CascadeType.UPDATE)
        //                    optType = OptType.Update;

        //                entities.Add(new OptEntity(obj, optType));
        //                break;
        //            case AssociationType.Aggregation://聚合关系
        //            case AssociationType.Composition://组合关系
        //                foreach (dynamic entity in obj)
        //                {
        //                    //控制级联类别
        //                    if (item.CasType == CascadeType.SAVE || item.CasType == CascadeType.UNDELETE || item.CasType == CascadeType.ALL)
        //                    {
        //                        if (repository.Exist(entity))
        //                            optType = OptType.Update;
        //                        else
        //                            optType = OptType.Create;
        //                    }
        //                    else if (item.CasType == CascadeType.UPDATE)
        //                        optType = OptType.Update;

        //                    entities.Add(new OptEntity(entity, optType));
        //                }
        //                break;
        //        }
                
        //    }

        //    DBTransaction tran = new DBTransaction(entities);//此处需改进支持多数据库
        //    return tran.Excute();
        //}

    }
}
