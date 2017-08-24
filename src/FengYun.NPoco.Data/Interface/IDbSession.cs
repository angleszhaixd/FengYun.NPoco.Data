using NPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 基础数据仓储
    /// </summary>
    public interface IDbSession: IRepository,ITransaction
    {
        /// <summary>
        /// 对外公布数据库会话
        /// </summary>
        INPocoDataBase DbContext { get;}

        #region 执行普通SQL语句
        int ExcuteNoQuery(string sql, params object[] parameters);
        int ExcuteNoQuery(Sql sql);
        DataTable ExcuteQuery(string sql, params object[] parameters);
        DataTable ExcuteQuery(Sql sql);
        T ExcuteScalar<T>(string sql, params object[] parameters);
        T ExcuteScalar<T>(Sql sql);
        #endregion

        #region 存储过程调用
        DataTable QueryStoredProcedure(string procedureName, params StoreParameter[] parameters);
        IEnumerable<TEntity> QueryStoredProcedure<TEntity>(string procedureName, params StoreParameter[] parameters);
        TEntity SingleStoredProcedure<TEntity>(string procedureName, params StoreParameter[] parameters);
        int ExecuteStoredProcedure(string procedureName, params StoreParameter[] parameters);
        #endregion
    }
}
