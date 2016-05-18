using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data;
using System.Diagnostics;
using System.Reflection;

namespace NDolls.Data.EU
{
    /// <summary>
    /// 执行单元执行类
    /// </summary>
    public class Invoker
    {
        private String form;//功能窗体
        private String opt;//功能操作
        List<Command> commands = new List<Command>();//命令集合

        /// <summary>
        /// 构造函数
        /// </summary>
        public Invoker() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="opt">功能操作类别</param>
        public Invoker(String opt)
        {
            StackTrace trace = new StackTrace();
            this.form = trace.GetFrame(1).GetMethod().ReflectedType.FullName;
            this.opt = opt;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="form">功能窗体</param>
        /// <param name="opt">功能操作类别</param>
        public Invoker(String form,String opt)
        {
            this.form = form;
            this.opt = opt;
        }

        /// <summary>
        /// 添加命令
        /// </summary>
        public void AddCommand(Command cmd)
        {
            commands.Add(cmd);
        }

        /// <summary>
        /// 取消命令
        /// </summary>
        public void CancleCommand(Command cmd)
        {
            commands.Remove(cmd);
        }

        private bool hasAuth()
        {
            //若未配置验证，默认权限开放
            if (String.IsNullOrEmpty(DataConfig.AuthAssembleName)
                || String.IsNullOrEmpty(DataConfig.AuthClassName)
                || String.IsNullOrEmpty(DataConfig.AuthMethodName))
            {
                return true;
            }

            try
            {
                MethodInfo mi = NDolls.Data.Util.EntityUtil.
                    GetMethod(DataConfig.AuthAssembleName, DataConfig.AuthClassName, DataConfig.AuthMethodName);
                if (mi.Invoke(null, new object[] { form, opt }).ToString().ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行命令集（事务处理）
        /// </summary>
        public bool ExecuteCommands()
        {
            if (!hasAuth())
            {
                //throw new Exception("没有该功能权限！");
                return false;
            }

            DBTransaction tran = new DBTransaction();
            tran.TransactionOpen();

            List<Command> tCommands;
            try
            {
                tCommands = commands.FindAll(c=>c.IsTran == true);
                foreach (Command cmd in tCommands)
                {
                    cmd.Execute(tran);
                }
                tran.TransactionCommit();

                tCommands = commands.FindAll(c => c.IsTran == false);
                foreach (Command cmd in tCommands)
                {
                    cmd.Execute(tran);
                }

                return true;
            }
            catch(Exception ex)
            {
                tran.TransactionRollback();
                throw ex;
            }
        }

        public IList<Command> Commands
        {
            get { return commands; }
        }

        public Command Command
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
