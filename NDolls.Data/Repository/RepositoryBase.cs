using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Util;
using NDolls.Data.Entity;
using NDolls.Data.Attribute;
using System.Reflection;
using System.Data.Common;

namespace NDolls.Data
{
    public class RepositoryBase<T> where T : EntityBase
    {
        protected static readonly string selectSQL = "SELECT {0} FROM {1} WHERE {2}";
        protected static readonly string insertSQL = "INSERT INTO {0}({1}) VALUES({2})";
        protected static readonly string updateSQL = "UPDATE {0} SET {1} WHERE {2}";
        protected static readonly string deleteSQL = "DELETE FROM {0} WHERE {1}";

        protected string tableName = "";//数据库表名
        protected string primaryKey = "";//数据库主键字段名
        protected string[] primaryKeys; //数据库主键字段名集合

        /// <summary>
        /// 构造函数，初始化实体信息(对应表名、表主键)
        /// </summary>
        public RepositoryBase()
        {
            tableName = EntityUtil.GetTableName(typeof(T));
            primaryKey = EntityUtil.GetPrimaryKey(tableName);
            primaryKeys = primaryKey.Split(',');
        }

        /// <summary>
        /// 构造函数，初始化实体信息(对应表名、表主键)
        /// </summary>
        public RepositoryBase(Type type)
        {
            tableName = EntityUtil.GetTableName(type);
            primaryKey = EntityUtil.GetPrimaryKey(tableName);
            primaryKeys = primaryKey.Split(',');
        }

        /// <summary>
        /// 数据库处理对象
        /// </summary>
        protected IDBHelper DBHelper
        {
            get
            {
                String key = "NDolls.Data." + Enum.GetName(typeof(DBType),DataConfig.DatabaseType);
                if (Storage.DBHelperDic.ContainsKey(key))
                {
                    return Storage.DBHelperDic[key];
                }
                else
                {
                    Type type = Type.GetType("NDolls.Data." + Enum.GetName(typeof(DBType), DataConfig.DatabaseType)+"Helper");
                    IDBHelper helper = Activator.CreateInstance(type) as IDBHelper;
                    Storage.DBHelperDic[key] = helper;

                    return helper;
                }
            }
        }

        /// <summary>
        /// 用户自定义特性信息
        /// </summary>
        public List<Attribute.CustomAttribute> CustomFields
        {
            get
            {
                return EntityUtil.GetFieldsByType(typeof(T)).CustomFields;
            }
        }

        /// <summary>
        /// 验证实体对象，并返回错误信息
        /// </summary>
        public String Validate(T entity)
        {
            return EntityUtil.ValidateEntity(entity);
        }

        #region 事务处理
        private DbTransaction tran = null;

        public void TransactionBegin()
        {
            tran = DBHelper.Connection.BeginTransaction();
        }

        public void TransactionCommit()
        {
            if (tran != null)
                tran.Commit();
        }

        public void TransactionRollback()
        {
            if (tran != null)
                tran.Rollback();
        }

        /// <summary>
        /// 持久化主对象及其的关联对象信息
        /// </summary>
        /// <param name="model">操作主对象</param>
        /// <param name="filedName">关联对象集合</param>
        public bool Persist(OptEntity model, List<AssociationAttribute> associations)
        {
            OptType optType = OptType.Save;

            List<OptEntity> entities = new List<OptEntity>();//实体对象集合
            entities.Add(model);//加入主对象

            if (associations != null)
            {
                Type type = model.Entity.GetType();
                PropertyInfo info;
                dynamic obj;
                dynamic repository;
                foreach (AssociationAttribute item in associations)
                {
                    info = type.GetProperty(item.FieldName);
                    repository =
                        RepositoryFactory<EntityBase>.CreateRepository(info.PropertyType.GetGenericArguments()[0]);//此处泛型T无实际作用

                    obj = info.GetValue(model.Entity, null);
                    if (obj == null)
                        continue;

                    switch (item.AssType)
                    {
                        case AssociationType.Association://关联关系
                            //控制级联类别
                            if (item.CasType == CascadeType.SAVE || item.CasType == CascadeType.UNDELETE || item.CasType == CascadeType.ALL)
                            {
                                if (repository.Exist(obj))
                                    optType = OptType.Update;
                                else
                                    optType = OptType.Create;
                            }
                            else if (item.CasType == CascadeType.UPDATE)
                                optType = OptType.Update;

                            entities.Add(new OptEntity(obj, optType));
                            break;
                        case AssociationType.Aggregation://聚合关系
                        case AssociationType.Composition://组合关系
                            foreach (dynamic entity in obj)
                            {
                                //控制级联类别
                                if (item.CasType == CascadeType.SAVE || item.CasType == CascadeType.UNDELETE || item.CasType == CascadeType.ALL)
                                {
                                    if (repository.Exist(entity))
                                        optType = OptType.Update;
                                    else
                                        optType = OptType.Create;
                                }
                                else if (item.CasType == CascadeType.UPDATE)
                                    optType = OptType.Update;

                                entities.Add(new OptEntity(entity, optType));
                            }
                            break;
                    }//end switch
                }//end foreach
            }//end if

            DBTransaction tran = new DBTransaction(DBHelper,entities);//此处需改进支持多数据库
            return tran.Excute();
        }

        #endregion

    }
}
