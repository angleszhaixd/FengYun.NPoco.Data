using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;
using System.Data;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 仓储实现
    /// author:zhaixd
    /// date:2016.08.12
    /// </summary>
    public class DbSession : DbSessionBase,IDbSession
    {
        public INPocoDataBase DbContext
        {
            get { return this.DataContext; }
        }
        /// <summary>
        /// 默认构造
        /// </summary>
        /// <param name="dataBase"></param>
        public DbSession(INPocoDataBase dataBase)
        {

            this.DataContext = dataBase;
        }

        #region private_method
        /// <summary>
        /// 获取SQL查询模板（暂时停用）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="conditionAct"></param>
        /// <returns></returns>
        private SqlBuilder.Template GetSqlTemlate<TEntity>(Action<SqlBuilder> conditionAct)
        {
            var _sqlBuilder = new SqlBuilder();
            _sqlBuilder.Select("*"); //默认查询所有字段
            var _sqlTmpl = _sqlBuilder.DefaultTemplete<TEntity>(this.DataContext);
            
            if (conditionAct != null)
                conditionAct(_sqlBuilder);
            return _sqlTmpl;
        }
        #endregion
       
        /// <summary>
        /// 获取指定主键值得实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="primaryKey">主键值</param>
        /// <returns></returns>
        public TEntity SingleById<TEntity>(object primaryKey)
        {
            return DataContext.SingleOrDefaultById<TEntity>(primaryKey);
        }
        /// <summary>
        /// 通过指定条件获取单一实体
        /// 多于一条则报错,不确定有且仅有一条时请使用 First
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">查询条件或查询语句</param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public TEntity Single<TEntity>(string sql,params object[] keys)
        {
            return DataContext.SingleOrDefault<TEntity>(sql, keys);
        }

        /// <summary>
        /// 通过指定条件获取单一实体
        /// 多于一条则报错,不确定有且仅有一条时请使用 First
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public TEntity Single<TEntity>(SearchSqlBuilder search)
        {
            var _sqlTmpl = search.Build<TEntity>(this.DataContext);
            return DataContext.SingleOrDefault<TEntity>(_sqlTmpl);
        }

        /// <summary>
        /// 通过指定条件获取第一个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">查询条件或查询语句</param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public TEntity First<TEntity>(string sql, params object[] keys)
        {
            return DataContext.FirstOrDefault<TEntity>(sql, keys);
        }

        /// <summary>
        /// 通过指定条件获取第一个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">查询条件</param>
        /// <returns></returns>
        public TEntity First<TEntity>(SearchSqlBuilder search)
        {
            var _sqlTmpl = search.Build<TEntity>(this.DataContext);
            return DataContext.FirstOrDefault<TEntity>(_sqlTmpl);
        }
        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Query<TEntity>(string sql,params object[] args)
        {
            return DataContext.Query<TEntity>(sql, args);
        }
        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <typeparam name="TEntity">查询的对象</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sql">sql语句或查询条件(where xx=@0)</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public Page<TEntity> QueryPage<TEntity>(int pageIndex,int pageSize,string sql,params object[] args)
        {
            return DataContext.Page<TEntity>(pageIndex, pageSize, sql, args);
        }
        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Query<TEntity>(SearchSqlBuilder search)
        {
            //return default(TEntity);
            var _sqlTmpl = search.Build<TEntity>(this.DataContext);
            return DataContext.Query<TEntity>(_sqlTmpl);
        }
        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="search">查询条件中必须指定Order字段</param>
        /// <returns></returns>
        public Page<TEntity> QueryPage<TEntity>(SearchSqlBuilder search)
        {
            var _sqlTmpl = search.Build<TEntity>(this.DataContext);
            if (search.Pageing == null)
                search.Pageing = search.DefaultPageInfo;
            return DataContext.Page<TEntity>(search.Pageing.PageIndex, search.Pageing.PageSize, _sqlTmpl);
        }
        public object Insert<TEntity>(TEntity Model)
        {
            return DataContext.Insert(Model);
        }
        public int Update(object model)
        {
            return DataContext.Update(model);
        }
        public int Delete(object model)
        {
            return DataContext.Delete(model);
        }
        public int Delete<TEntity>(object PrimaryKey)
        {
            return DataContext.Delete<TEntity>(PrimaryKey);
        }

        #region 执行普通SQL语句
        public int ExcuteNoQuery(string sql, params object[] parameters)
        {
            return DataContext.Execute(sql, parameters);
        }
        public int ExcuteNoQuery(Sql sql)
        {
            return DataContext.Execute(sql.SQL, sql.Arguments);
        }
        public DataTable ExcuteQuery(string sql, params object[] parameters)
        {
            if (sql.ToLower().IndexOf("select") < 0)
                throw new ArgumentException("ExcuteQuery查询语句出错,不包含SELECT关键字:" + nameof(sql));
            var items = DataContext.Fetch<Dictionary<string, object>>(sql, parameters);
            return items.ToDataTable();
        }
        public DataTable ExcuteQuery(Sql sql)
        {
            return ExcuteQuery(sql.SQL, sql.Arguments);
        }
        public T ExcuteScalar<T>(string sql, params object[] parameters)
        {
            return DataContext.ExecuteScalar<T>(sql, parameters);
        }
        public T ExcuteScalar<T>(Sql sql)
        {
            return DataContext.ExecuteScalar<T>(sql.SQL, sql.Arguments);
        }
        #endregion


        #region 存储过程调用
        public DataTable QueryStoredProcedure(string procedureName, params StoreParameter[] parameters)
        {
            var procBuilder = new StoredProcedureBuilder(procedureName,DataContext);
            procBuilder.AddParameters(parameters);
            var sql = procBuilder.Build();
            var items = DataContext.Fetch<Dictionary<string,object>>(sql);
            return items.ToDataTable();
        }

        public IEnumerable<T> QueryStoredProcedure<T>(string procedureName, params StoreParameter[] parameters)
        {
            var procBuilder = new StoredProcedureBuilder(procedureName, DataContext);
            procBuilder.AddParameters(parameters);
            return DataContext.Query<T>(procBuilder.Build());
        }

        public T SingleStoredProcedure<T>(string procedureName, params StoreParameter[] parameters)
        {
            var procBuilder = new StoredProcedureBuilder(procedureName, DataContext);
            procBuilder.AddParameters(parameters);
            return DataContext.SingleOrDefault<T>(procBuilder.Build());
        }

        public int ExecuteStoredProcedure(string procedureName, params StoreParameter[] parameters)
        {
            var procBuilder = new StoredProcedureBuilder(procedureName, DataContext);
            procBuilder.AddParameters(parameters);
            var _sql = procBuilder.Build();
            return DataContext.Execute(_sql.SQL,CommandType.StoredProcedure,_sql.Arguments);
        }
        #endregion

    }
}
