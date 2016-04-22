using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Auth
{
    public abstract class Handler
    {
        protected Handler successor;
        /// <summary>
        /// 设置下一处理者
        /// </summary>
        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        public abstract bool AuthRequest(String form, String opt);
    }
}
