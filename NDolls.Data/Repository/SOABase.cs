using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using NDolls.Data.Entity;

namespace NDolls.Data
{
    public class SOABase<T> : IHttpHandler, IRequiresSessionState where T : EntityBase
    {
        private String _SOAPath;
        protected ServiceBase<T> s = new ServiceBase<T>();
        
        // 用户验证代理
        public delegate Boolean AuthenticateDelegate();
        public AuthenticateDelegate authenticate;

        /// <summary>
        /// 构造函数，初始化模板引擎
        /// </summary>
        /// <param name="_SOAPath">SOA程序集路径</param>
        public SOABase(String _SOAPath)
        {
            this._SOAPath = _SOAPath;
        }

        /// <summary>
        /// 一般处理程序调用开始
        /// </summary>
        public void ProcessRequest(HttpContext context)
        {
            if (authenticate != null && !authenticate.Invoke())
            {
                throw new Exception("用户验证失败!");
            }

            String controller = context.Request["c"];//Controller名称
            String action = context.Request["a"];//Action名称
            String param = context.Request["p"];//参数

            if (String.IsNullOrEmpty(controller) || String.IsNullOrEmpty(action))
            {
                throw new Exception("调用参数不完整!");
            }

            invokeMethod(controller, action, param);
        }

        /// <summary>
        /// 处理方法调用(可通过缓存优化)
        /// </summary>
        /// <param name="controller">控制器名称</param>
        /// <param name="action">控制器中的方法名称</param>
        /// <param name="p">默认参数值</param>
        private void invokeMethod(String controller, String action, String p)
        {
            if (String.IsNullOrEmpty(controller))
            {
                return;
            }

            Type type = Type.GetType(_SOAPath + "." + controller);
            object obj = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(action);
            if (method.GetParameters().Length == 0)//调用无参方法
            {
                method.Invoke(obj, null);
            }
            else//调用有参方法
            {
                method.Invoke(obj, new object[] { p });
            }
        }

        public virtual void HAdd()
        { }

        public virtual void HUpdate()
        { }

        public virtual void HGet(String id)
        { }

        public String JsonP(String json)
        {
            String cb = HttpContext.Current.Request["callback"];
            if (!String.IsNullOrEmpty(cb))
            {
                json = cb + "(" + json + ")";
            }
            return json;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
