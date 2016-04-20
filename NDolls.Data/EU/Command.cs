using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data;

namespace NDolls.Data.EU
{
    public abstract class Command
    {
        public bool IsTran = true;
        public abstract void Execute(DBTransaction tran);
        public object Result{get;set;}
    }
}
