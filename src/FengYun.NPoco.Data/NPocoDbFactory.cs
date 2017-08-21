using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace FengYun.NPoco.Data
{
    public class NPocoDbFactory : IDbFactory
    { 
        private static DatabaseFactory _DbFactory { get; set; }
        private static string _defaultConnectionName;
        /// <summary>
        /// 当前链接字符串名称
        /// </summary>
        public string DefaultConnectionName
        {
            get
            {
                return _defaultConnectionName;
            }
        }
        static NPocoDbFactory()
        {
            _defaultConnectionName = GetDefaultConnectionName();
        }
         
        public IDbSession GetDbSession(bool keepAlive = false)
        {
            return GetDbSession(DefaultConnectionName, keepAlive);
        }

        public IDbSession GetDbSession(string connectionName, bool keepAlive = false)
        {
            NPocoDataBase dataBase= _DbFactory == null ? new NPocoDataBase(DefaultConnectionName):_DbFactory.GetDatabase() as NPocoDataBase;
            if (keepAlive)
                dataBase.KeepConnectionAlive = true;
            //DbConnection conn = new SqlConnection(DefaultConnectionName);
            //dataBase = new NPocoDataBase(conn);
            return new DbSession(dataBase);
        }

        public static void Init()
        {
            _DbFactory = DatabaseFactory.Config(x =>
            {
                x.UsingDatabase(() => new NPocoDataBase(_defaultConnectionName));
            });
        }
        /// <summary>
        /// 获取默认数据库连接字符串名
        /// </summary>
        /// <returns></returns>
        private static string GetDefaultConnectionName()
        {
            string contextName = ConfigurationManager.AppSettings["DefaultConn"];
            if (string.IsNullOrEmpty(contextName)) {
                //获取最后配置的链接字符串
                int lastIndex = ConfigurationManager.ConnectionStrings.Count - 1;
                lastIndex = lastIndex < 0 ? 0 : lastIndex;
                contextName = ConfigurationManager.ConnectionStrings[lastIndex].Name;
            }
            return contextName;
        }

        public void Setup()
        {
            Init();
        }
    }
}
