using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Auth
{
    public class AuthHandler : Handler
    {
        private Role role;

        public AuthHandler(Role role)
        {
            this.role = role;
        }

        /// <summary>
        /// 认证请求合法性
        /// </summary>
        /// <param name="module">模块名称</param>
        /// <param name="opt">操作名称</param>
        /// <returns>认证结果</returns>
        public override bool AuthRequest(string module,string opt)
        {
            bool ret = false;

            if (role.RoleAuth.ContainsKey(module) && role.RoleAuth[module].Contains(opt))
            {
                ret = true;
            }
            else if(successor != null)
            {
                ret = successor.AuthRequest(module, opt);
            }

            return ret;
        }
    }
}
