using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NDolls.Core
{
    public class Log
    {
        private String logPath;//日志路径
        private LogMod mod;//日志记录方式
        private static Queue<String> logs = new Queue<string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logPath">日志目录（文件夹）</param>
        /// <param name="mod">日志记录方式</param>
        public Log(String logPath,LogMod mod)
        {
            this.logPath = logPath;
            this.mod = mod;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(doWrite));
            t.IsBackground = true;
            t.Start(logPath);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public void WriteLog(string content)
        {
            logs.Enqueue(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + content + "\n");
        }

        private void doWrite(object logPath)
        {
            while (true)
            {
                DateTime dt = DateTime.Now;
                String filePath = "";
                try
                {

                    if (!Directory.Exists(logPath.ToString()))  //工程目录下 Log目录 '目录是否存在,为true则没有此目录
                    {
                        Directory.CreateDirectory(logPath.ToString()); //建立目录　Directory为目录对象
                    }

                    filePath = logPath + "\\log";
                    switch (mod)
                    {
                        case LogMod.Year:
                            filePath += dt.Year;
                            break;
                        case LogMod.Month:
                            filePath += dt.ToString("yyyyMM");
                            break;
                        case LogMod.Day:
                            filePath += dt.ToString("yyyyMMdd");
                            break;
                        default:
                            filePath += dt.ToString("yyyyMM");
                            break;
                    }
                    filePath += ".txt";

                    StreamWriter FileWriter = new StreamWriter(filePath, true); //创建日志文件
                    while (logs.Count > 0)
                    {
                        FileWriter.WriteLine(logs.Dequeue());
                    }
                    FileWriter.Close(); //关闭StreamWriter对象
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                System.Threading.Thread.Sleep(2000);
            }
        }
    }

    /// <summary>
    /// 日志记录方式
    /// </summary>
    public enum LogMod
    {
        Year,
        Month,
        Day
    }
}
