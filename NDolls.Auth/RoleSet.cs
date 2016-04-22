using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Auth
{
    /// <summary>
    /// 角色集合
    /// </summary>
    public class RoleSet
    {
        Handler handler;//职责链初始处理者

        private List<Role> roles = new List<Role>();
        /// <summary>
        /// 角色集合
        /// </summary>
        public List<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        public void AddRole(Role role)
        {
            if (role != null)
            {
                roles.Add(role);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        public void RemoveRole(Role role)
        {
            if (role != null)
            {
                roles.Remove(role);
            }
        }

        /// <summary>
        /// 初始化职责链
        /// </summary>
        public void Reset()
        {
            handler = null;
        }

        /// <summary>
        /// 验证是否有权限
        /// </summary>
        /// <param name="module">模块名称</param>
        /// <param name="opt">操作名称</param>
        public bool HasAuth(String module,String opt)
        {
            if (roles != null && roles.Count > 0)
                return false;

            if (handler == null)
            {
                int i = 0;
                handler = new AuthHandler(roles[0]);//职责链初始处理者
                foreach (Role role in roles)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    handler.SetSuccessor(new AuthHandler(role));//职责链后续处理者
                }
            }

            return handler.AuthRequest(module, opt);
        }
    }
}
