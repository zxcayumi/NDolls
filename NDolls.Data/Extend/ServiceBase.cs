using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;

namespace NDolls.Data
{
    /// <summary>
    /// 数据服务基类
    /// </summary>
    /// <typeparam name="T">Model类型</typeparam>
    public class ServiceBase<T> where T : Entity.EntityBase
    {
        public static IRepository<T> r =
            RepositoryFactory<T>.CreateRepository(typeof(T).ToString());

        /// <summary>
        /// 设置是否开启级联查询（设置完全局有效）
        /// </summary>
        /// <param name="isAllowed">是否开启</param>
        public void SetAllowAssociation(Boolean isAllowed)
        {
            Data.DataConfig.AllowAssociation = isAllowed;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        public bool Add(T model)
        {
            return r.Add(model);
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        public bool Update(T model)
        {
            return r.Update(model);
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        public bool Update(T model, OptType type)
        {
            return r.Update(model, type);
        }

        /// <summary>
        /// 保存对象（系统自动判断增或改）
        /// </summary>
        public bool Save(T model)
        {
            return r.AddOrUpdate(model);
        }

        /// <summary>
        /// 保存对象（系统自动判断增或改）
        /// </summary>
        public bool Save(T model, OptType type)
        {

            return r.AddOrUpdate(model);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        public bool Delete(String pk)
        {
            return r.Delete(pk);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        public bool Delete(String[] pks)
        {
            return r.Delete(pks);
        }

        /// <summary>
        /// 查找全部实体对象
        /// </summary>
        public List<T> GetAllModels()
        {
            return r.FindAll();
        }

        /// <summary>
        /// 按条件查询对象集合
        /// </summary>
        public List<T> GetModels(T model)
        {
            return r.Find(model);
        }

        /// <summary>
        /// 按条件查询对象集合
        /// </summary>
        public List<T> GetModels(Item item)
        {
            return r.FindByCondition(item);
        }

        /// <summary>
        /// 按条件查询对象集合
        /// </summary>
        public List<T> GetModels(List<Item> conditions)
        {
            return r.FindByCondition(conditions);
        }

        /// <summary>
        /// 按条件查询对象集合
        /// </summary>
        public List<T> GetModels(int top, String sqlCondition)
        {
            return r.Find(top, sqlCondition);
        }

        /// <summary>
        /// 按条件查询对象集合
        /// </summary>
        public List<T> GetModels(String sqlCondition)
        {
            return r.Find(sqlCondition);
        }

        /// <summary>
        /// 无条件分页查询,每页10条记录
        /// </summary>
        public List<T> GetModels(int pageIndex)
        {
            return r.FindByPage(10, pageIndex, null);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public List<T> GetModels(int pageIndex, int pageCount, List<Item> conditions)
        {
            return r.FindByPage(pageCount, pageIndex, conditions);
        }

        /// <summary>
        /// 查找符合条件的前top条对象
        /// </summary>
        public List<T> GetModels(int top, List<Item> conditions)
        {
            return r.FindByCondition(top, conditions);
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        public int GetCount(List<Item> items)
        {
            return r.GetCount(items);
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        public int GetCount()
        {
            return r.GetCount("");
        }

        /// <summary>
        /// 查询符合条件的总记录数
        /// </summary>
        public int GetCount(String sqlCondition)
        {
            return r.GetCount(sqlCondition);
        }

        /// <summary>
        /// 查找具体实体对象
        /// </summary>
        public T GetModel(String pk)
        {
            return r.FindByPK(pk);
        }

        /// <summary>
        /// 查找具体实体对象
        /// </summary>
        public T GetModel(String[] pks)
        {
            return r.FindByPK(pks);
        }

        /// <summary>
        /// 获取分页信息
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="items">查询条件集合</param>
        /// <returns>分页数据</returns>
        public Paper<T> GetPaper(int pageSize,int pageIndex,List<Item> items)
        {
            return r.FindPager(pageSize, pageIndex, items);
        }
    }
}
