using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FengYun.NPoco.Data
{
    public interface IRepository
    {
        /// <summary>
        /// 获取指定主键值得实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="primaryKey">主键值</param>
        /// <returns></returns>
        TEntity SingleById<TEntity>(object primaryKey);
        /// <summary>
        /// 通过指定条件获取单一实体
        /// 多于一条则报错,不确定有且仅有一条时请使用 First
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">查询条件或查询语句</param>
        /// <param name="keys"></param>
        /// <returns></returns>
        TEntity Single<TEntity>(string sql, params object[] keys);

        /// <summary>
        /// 通过指定条件获取单一实体
        /// 多于一条则报错,不确定有且仅有一条时请使用 First
        /// </summary>
        /// <typeparam name="TEntity">查询委托</typeparam>
        /// <param name="conditionAct"></param>
        /// <returns></returns>
        TEntity Single<TEntity>(SearchSqlBuilder search);
        /// <summary>
        /// 通过指定条件获取第一个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">查询条件或查询语句</param>
        /// <param name="keys"></param>
        /// <returns></returns>
        TEntity First<TEntity>(string sql, params object[] keys);
        /// <summary>
        /// 通过指定条件获取第一个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">查询条件</param>
        /// <returns></returns>
        TEntity First<TEntity>(SearchSqlBuilder search);

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Query<TEntity>(string sql, params object[] args);
        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <typeparam name="TEntity">查询的对象</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sql">sql语句或查询条件(where xx=@0)</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        Page<TEntity> QueryPage<TEntity>(int pageIndex, int pageSize, string sql, params object[] args);
        /// <summary>
        /// 查询数据列表
        /// </summary>
        IEnumerable<TEntity> Query<TEntity>(SearchSqlBuilder search);

        /// <summary>
        /// 分页查询列表
        /// </summary>
        Page<TEntity> QueryPage<TEntity>(SearchSqlBuilder search);
        object Insert<TEntity>(TEntity Model);
        int Update(object model);
        int Delete(object model);
        int Delete<TEntity>(object PrimaryKey);
    }
}
