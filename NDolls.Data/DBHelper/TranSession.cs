using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace NDolls.Data
{
    class TranSession
    {
        public Guid SID { get; set; }
        public DbConnection SConnection { get; set; }
        public DbTransaction STransaction { get; set; }
    }
}
