using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NPoco;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 查询条件扩展
    /// author：zhaixd
    /// datetime:2016.12.06
    /// </summary>
    public static class SqlBuilderExtensions
    {
        /// <summary>
        /// 获取默认的Sql查询语句 zxd 2016.12.06
        /// 格式:select /**select**/ from test where /**where**/ and id = @0 /**orderby**/
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlBuilder"></param>
        /// <param name="database">DB对象</param>
        /// <param name="sqlWhere">默认查询条件（注意:初始化时where 条件最后必须是/**where**/,用于替换后续添加的条件 ）</param>
        /// <returns></returns>
        public static SqlBuilder.Template DefaultTemplete<TEntity>(this SqlBuilder sqlBuilder,IDatabase database, string sqlWhere= "where /**where**/")
        {
            var pd = database.PocoDataFactory.ForType(typeof(TEntity));
            var tableName = database.DatabaseType.EscapeTableName(pd.TableInfo.TableName);
            /*
            var columns = pd.QueryColumns.Select(c =>
            {
                return database.DatabaseType.EscapeSqlIdentifier(c.Value.ColumnName) +
                       (!string.IsNullOrEmpty(c.Value.ColumnAlias)
                            ? " AS " + database.DatabaseType.EscapeSqlIdentifier(c.Value.ColumnAlias)
                            : " AS " + database.DatabaseType.EscapeSqlIdentifier(c.Value.MemberInfoKey));
            });
            string cols = String.Join(", ", columns.ToArray());
            */
            //var selectSql = AutoSelectHelper.AddSelectClause(database, typeof(TEntity), sql);
            string selectSql = string.Format("SELECT /**select**/ FROM {0} {1} /**orderby**/", tableName, sqlWhere);
            return sqlBuilder.AddTemplate(selectSql);
        }

        /// <summary>
        /// 拼接查询条件 eg:SqlBuilder.WhereIf(1==1,"id=@0",1)
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="Clause">判断条件</param>
        /// <param name="strWhere">查询条件 eg: id=@0/ id=@0 OR name=@1</param>
        /// <param name="args">条件字段对应的值</param>
        /// <returns></returns>
        public static SqlBuilder WhereIf(this SqlBuilder sqlBuilder,bool Clause, string strWhere,params object[] args)
        {
            return Clause ? sqlBuilder.Where(strWhere, args) : sqlBuilder;
        }
        /// <summary>
        /// 构建常规查询
        /// </summary>
        /// <param name="search"></param>
        /// <param name="act">查询委托</param>
        /// <returns></returns>
        public static SearchSqlBuilder Create(this SearchSqlBuilder search, Action<SqlBuilder> act)
        {
            search.QueryConditionAct = act;
            return search;
        }
        /// <summary>
        /// 构建分页查询(act参数中必须调用order指定排序)
        /// </summary>
        /// <param name="search"></param>
        /// <param name="act">查询条件构建</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数(默认10)</param>
        /// <returns></returns>
        public static SearchSqlBuilder CreatePage(this SearchSqlBuilder search, Action<SqlBuilder> act, int pageIndex, int pageSize=10)
        {
            search.QueryConditionAct = act;
            search.Pageing = new SearchSqlBuilder.PageInfo(pageIndex, pageSize);
            return search;
        }
    }
}
