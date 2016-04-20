using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data;
using NDolls.Data.Entity;

namespace NDolls.Data.EU
{
    public class CommandDelete<T> : Command where T : EntityBase
    {
        private List<Item> conditions;//条件集合
        private String[] keys;//主键

        public CommandDelete(List<Item> conditions)
        {
            this.conditions = conditions;
        }

        public CommandDelete(String[] keys)
        {
            this.keys = keys;
        }

        public override void Execute(DBTransaction tran)
        {
            IRepository<T> r = RepositoryFactory<T>.CreateRepository(tran);
            if (conditions != null && conditions.Count > 0)//按条件删除
            {
                Result = r.DeleteByCondition(conditions);
            }
            else//按主键删除
            {
                Result = r.Delete(keys);
            }
        }

        public override string ToString()
        {
            return "DELETE";
        }
    }
}
