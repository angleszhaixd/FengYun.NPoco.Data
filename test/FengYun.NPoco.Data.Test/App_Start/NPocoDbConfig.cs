using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FengYun.NPoco.Data;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(FengYun.NPoco.Data.Test.NPocoDbConfig), "Register")]
namespace FengYun.NPoco.Data.Test
{
    /// <summary>
    /// 初始化NPoco-ORM
    /// </summary>
    public class NPocoDbConfig
    {
        /// <summary>
        /// 初始化NPoco-ORM数据库仓储
        /// </summary>
        public static void Register()
        {
            DefaultDbFactory.Setup();
        }
    }
}