using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data;
using NDolls.Data.Entity;

namespace NDolls.Data.EU
{
    public class CommandUpdate<T> : Command where T : EntityBase
    {
        private T model;
        private List<Item> conditions;//条件集合

        public CommandUpdate(T model)
        {
            this.model = model;
        }

        public CommandUpdate(T model,List<Item> conditions)
        {
            this.model = model;
            this.conditions = conditions;
        }

        public override void Execute(DBTransaction tran)
        {
            IRepository<T> r = RepositoryFactory<T>.CreateRepository(tran);
            if (conditions != null && conditions.Count > 0)//按条件
            {
                Result = r.UpdateByCondition(model, conditions);
            }
            else
            {
                Result = r.Update(model);
            }
        }

        public override string ToString()
        {
            return "UPDATE";
        }
    }
}
