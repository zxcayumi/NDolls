using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;
using System.Data;

namespace NDolls.Data
{
    public interface IRepository<T> where T:EntityBase
    {
        List<Attribute.CustomAttribute> GetCustomFieldsByType(String type);

        List<T> FindAll();
        List<T> Find(T model);
        List<T> Find(int top, T model);
        List<T> FindByCondition(List<Item> items);
        List<T> FindByCondition(Item item);
        List<T> FindByCondition(int top, List<Item> items);
        List<T> FindByPage(int pageSize, int current, List<Item> items);
        Paper<T> FindPager(int pageSize, int current, List<Item> items);
        T FindByPK(string keyValue);
        T FindByPK(string[] keyValues);
        List<T> Find(String sqlCondition);
        List<T> Find(int top, String sqlCondition);

        int GetCount(List<Item> items);
        int GetCount(String customCondition);
        DataTable FindByCustom(String fields,List<Item> items);
        DataTable FindByCustom(String fields, String sqlCondition);

        String Validate(T entity);

        bool Add(T model);
        bool Update(T model);
        bool Update(T model, OptType type);
        bool UpdateByCondition(T model, List<Item> items);
        bool UpdateByCondition(T model, Item item);
        bool UpdateByCondition(T model, String sqlCondition);
        bool UpdateByCondition(Dictionary<String, Object> model, List<Item> items);
        bool UpdateByCondition(Dictionary<String, Object> model, Item item);
        bool UpdateByCondition(Dictionary<String, Object> model, String sqlCondition);

        bool AddOrUpdate(T model);
        bool Delete(string keyValue);
        bool Delete(string[] keyValues);
        bool DeleteByCondition(String sqlCondition);
        bool DeleteByCondition(Item item);
        bool DeleteByCondition(List<Item> items);
        bool Exist(T model);
        bool Exist(List<Item> items);
    }
}
