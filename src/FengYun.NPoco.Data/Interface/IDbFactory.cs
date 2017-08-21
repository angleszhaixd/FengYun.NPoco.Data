using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;
namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 数据回话工厂接口
    /// </summary>
    public interface IDbFactory
    {
        string DefaultConnectionName { get; }
        IDbSession GetDbSession(bool keepAlive = false);
        IDbSession GetDbSession(string connectionName,bool keepAlive = false);
        void Setup();
    }
}
