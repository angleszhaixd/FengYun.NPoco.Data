using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;
using System.Data;
using System.Dynamic;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 存储过程查询条件包裹器(目前支持sqlserver,orcale,mysql)
    /// author:zhaixd
    /// date:2016.08.11
    /// </summary>
    public class StoredProcedureBuilder
    {
        private readonly IDatabase _dataContext;
        private readonly Sql _sql;
        private bool _parameterAdded;
        private IList<StoreParameter> _parameters;
        /// <summary>
        /// 最终的参数对象
        /// </summary>
        private IDictionary<string,object> _argsObject;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="dataContext">当前DB对象</param>
        public StoredProcedureBuilder(string procedureName, IDatabase dataContext)
        {
            _dataContext = dataContext;
            var initalSql = getInitalSQL(procedureName);
            _sql = new Sql(initalSql);
            _parameters = new List<StoreParameter>();
            _argsObject = new Dictionary<string,object>();
        }

        /// <summary>
        /// 批量添加参数
        /// </summary>
        /// <param name="parameters"></param>
        public StoredProcedureBuilder AddParameters(IEnumerable<StoreParameter> parameters)
        {
            foreach (StoreParameter parameter in parameters)
            {
                AddParameter(parameter);
            }
            return this;
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameter"></param>
        public StoredProcedureBuilder AddParameter(StoreParameter parameter)
        {
            if (!parameter.Sort.HasValue)
                parameter.Sort = _parameters.Count;
            if (parameter.SpecialParameter != null) //特殊参数自动放到最后
                parameter.Sort = 9999;
            _parameters.Add(parameter);
            return this;
        }

        /// <summary>
        /// 构建最终的执行语句
        /// </summary>
        /// <returns></returns>
        public Sql Build()
        {
            StringBuilder sqlParameterFormat = new StringBuilder();
            //var dynamicDic = (IDictionary<string, object>)_argsObject;
            _parameters.OrderBy(o=>o.Sort).ToList().ForEach(pram =>
            {
                sqlParameterFormat.AppendFormat(_funSpecialParameterName(pram));
                _argsObject[pram.Name] = _funSpecialParameterValue(pram);
            });
            _sql.Append(sqlParameterFormat.ToString().TrimEnd(','), _argsObject);
            return _sql;
        }


        /// <summary>
        /// 获取调用存储过程的SQL语句
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        private string getInitalSQL(string procedureName)
        {
            string DEFAULT_PROCEDURE_FORMAT = "EXEC {0} ";
            string providerName = _dataContext.DatabaseType.GetProviderName();
            if (!string.IsNullOrEmpty(providerName))
            {
                if (providerName.IndexOf("MySql", StringComparison.OrdinalIgnoreCase) >= 0)
                    return string.Format(";CALL {0} ", procedureName);
                if (providerName.IndexOf("Oracle.DataAccess", StringComparison.OrdinalIgnoreCase) >= 0)
                    return string.Format(";CALL {0} ", procedureName);
                if (providerName.IndexOf("Oracle.ManagedDataAccess", StringComparison.OrdinalIgnoreCase) >= 0)
                    return string.Format(";CALL {0} ", procedureName);
                //if (providerName.IndexOf("pgsql", StringComparison.OrdinalIgnoreCase) >= 0)
                //    return "";
            }
            return string.Format(DEFAULT_PROCEDURE_FORMAT, procedureName);
        }

        /// <summary>
        /// 获取特殊参数类型名称
        /// </summary>
        private Func<StoreParameter, string> _funSpecialParameterName = (parameter) => {
            if (parameter.SpecialParameter != null)
            {
                if (parameter.SpecialParameter.Direction == ParameterDirection.Output)
                {
                    return string.Format(" @{0} Output,", parameter.Name);
                }
                //returnValue暂时有问题
                else if (parameter.SpecialParameter.Direction == ParameterDirection.ReturnValue)
                {
                    return string.Format(" @{0} ReturnValue,", parameter.Name);
                }
            }
            return string.Format(" @{0},", parameter.Name);
        };

        /// <summary>
        /// 获取特殊参数类型值
        /// </summary>
        private Func<StoreParameter, object> _funSpecialParameterValue = (parameter) => {
            if (parameter.SpecialParameter != null)
            {
                //parameter.SpecialParameter.ParameterName = parameter.Name;
                return parameter.SpecialParameter;
            }
            return parameter.Value;
        };
    }

    /// <summary>
    /// 存储过程参数实体(基本参数)
    /// author:zhaixd
    /// date:2016.08.11
    /// </summary>
    public class StoreParameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 参数序号
        /// </summary>
        public int? Sort { get; set; }
        /// <summary>
        /// 特殊参数(针对输出及返回值参数)
        /// </summary>
        public System.Data.Common.DbParameter SpecialParameter { get; set; }
        /// <summary>
        /// 默认构造
        /// </summary>
        public StoreParameter() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="sort">排序(不传按传入的输出排列)</param>
        public StoreParameter(string name, object value,int? sort=null)
            :this(name,value,sort,null)
        { 

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="dbParameter">特殊参数类型,现在仅支持输出参数Output</param>
        /// <param name="sort">排序</param>
        public StoreParameter(string name, System.Data.Common.DbParameter dbParameter, int? sort = null)
            : this(name, null, sort, dbParameter)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="sort">sort</param>
        /// <param name="dbParameter"></param>
        public StoreParameter(string name, object value,int? sort, System.Data.Common.DbParameter dbParameter)
        {
            name = (name ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(name))
                throw new InvalidSqlParameterSetup("参数名称不能为空:" + nameof(name));

            if (name.StartsWith("@"))
                throw new InvalidSqlParameterSetup("参数名称不能已@符号开头:" + nameof(name));
            this.Name = name;
            this.Value = value;
            this.Sort = sort;
            this.SpecialParameter = dbParameter;
        }
    }
}
