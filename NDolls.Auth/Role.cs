using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Auth
{
    /// <summary>
    /// 角色类
    /// </summary>
    public class Role
    {
        private Dictionary<String, List<String>> roleAuth = new Dictionary<String, List<String>>();

        /// <summary>
        /// 角色权限集合<模块名称,操作集合>
        /// </summary>
        public Dictionary<String, List<String>> RoleAuth
        {
            get { return roleAuth; }
            set { roleAuth = value; }
        }
    }
}
