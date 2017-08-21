using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 查询包裹器(用于构建查询语句)
    /// </summary>
    public class SearchSqlBuilder
    {
        /// <summary>
        /// 默认查询字段占位符
        /// </summary>
        private readonly Dictionary<string, string> defaultPlaceHolder = new Dictionary<string, string>() { { "select", "*" } };
        /// <summary>
        /// 默认分页查询实体
        /// </summary>
        public PageInfo DefaultPageInfo
        {
            get { return new PageInfo(1,10); }
        }
        public PageInfo Pageing { get; set; }
        public Action<SqlBuilder> QueryConditionAct { get; set; }
        /// <summary>
        /// SQL初始化模板
        /// </summary>
        public string SqlTemplate { get; set; }

        public SqlBuilder sqlBuilder;

        public SearchSqlBuilder()
        {
            sqlBuilder = new SqlBuilder(defaultPlaceHolder);
            sqlBuilder.Where("1=1");
        }

        public SearchSqlBuilder(string sqlTemplate)
        {
            sqlBuilder = new SqlBuilder(defaultPlaceHolder);
            this.SqlTemplate = sqlTemplate;
        }
        /// <summary>
        /// 获取查询sql模板
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public SqlBuilder.Template Build<TEntity>(IDatabase dataBase)
        {
            if (sqlBuilder == null)
                throw new ArgumentNullException("SearchSqlBuilder未初始化,获取查询模板失败："+nameof(sqlBuilder));
            SqlBuilder.Template _sqlTmpl = null;
            if (string.IsNullOrWhiteSpace(SqlTemplate))
            {
                _sqlTmpl = sqlBuilder.AddTemplate(SqlTemplate);
            }
            else
            {
                _sqlTmpl = sqlBuilder.DefaultTemplete<TEntity>(dataBase);
            }
            if (QueryConditionAct != null)
                QueryConditionAct(sqlBuilder);
            return _sqlTmpl;
        }

        public class PageInfo
        {
            public PageInfo(int index,int size)
            {
                this.PageIndex = index;
                this.PageSize = size;
            }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
    }
}

