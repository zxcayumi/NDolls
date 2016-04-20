using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;
using NDolls.Data;

namespace NDolls.Data.EU
{
    public class CommandAdd<T> : Command where T:EntityBase
    {
        private T model;

        public CommandAdd(T model)
        {
            this.model = model;
        }

        public override void Execute(DBTransaction tran)
        {
            IRepository<T> r = RepositoryFactory<T>.CreateRepository(tran);
            Result = r.Add(model);
        }
        
        public override string ToString()
        {
            return "ADD";
        }
    }
}
