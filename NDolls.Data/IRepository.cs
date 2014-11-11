using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;

namespace NDolls.Data
{
    public interface IRepository<T> where T:EntityBase
    {
        List<T> FindAll();
        List<T> Find(T model);
        List<T> Find(int top, T model);
        List<T> FindByCondition(List<ConditionItem> items);
        List<T> FindByCondition(ConditionItem item);
        List<T> FindByCondition(int top,List<ConditionItem> items);
        List<T> FindByPage(int pageCount, int index, List<ConditionItem> items);
        T FindByPK(string keyValue);
        T FindByPK(string[] keyValues);
        List<T> Find(String customCondition);

        String Validate(T entity);

        bool Add(T model);
        bool Update(T model);
        bool Delete(string keyValue);
        bool Delete(string[] keyValues);
        bool DeleteByCondition(ConditionItem item);
        bool DeleteByCondition(List<ConditionItem> items);
        bool Exist(T model);
    }
}
