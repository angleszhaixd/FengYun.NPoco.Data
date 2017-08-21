using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;
using NPoco.FluentMappings;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 数据库会话工厂类
    /// </summary>
    public class DefaultDbFactory
    {
        public static IDbFactory Factory { get { return LazyFactory.Value; } }
        private static readonly Lazy<IDbFactory> LazyFactory;
        static DefaultDbFactory()
        {
            LazyFactory = new Lazy<IDbFactory>(() =>
            {
                var dbFactory = new NPocoDbFactory();
                return dbFactory;
            });
        }
        /// <summary>
        /// 获取数据库回话
        /// </summary>
        /// <param name="keepAlive">是否保持连接(默认否)</param>
        /// <returns></returns>
        public static IDbSession GetDbSession(bool keepAlive=false)
        {
            var dbSession= Factory.GetDbSession(keepAlive);
            //if (startTrans)
            //    dbSession.BeginTranscation();
            return dbSession;
        }

        public static IDbSession GetDbSession(string connectionName,  bool keepAlive=false)
        {
            var dbSession = Factory.GetDbSession(connectionName,keepAlive);
          
            return dbSession;
        }

        /// <summary>
        /// 初始化DbFactory,网站启动时调用
        /// </summary>
        public static void Setup()
        {
            Factory.Setup();
        }
        
    }
}
