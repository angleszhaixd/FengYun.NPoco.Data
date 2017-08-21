using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FengYun.NPoco.Data;
using NPoco;
using System.Data;
using System.Text;
using System.Diagnostics;

namespace FengYun.NPoco.Data.Test
{
    public partial class DbFunctionTestForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        
        protected void test1_Click(object sender, EventArgs e)
        {
            StringBuilder msg = new StringBuilder();
            //测试查询
            using (var session = DefaultDbFactory.GetDbSession(true))
            {
                #region 一、Single 查询唯一实体(有且仅有一个,多于一个会报错)
                msg.AppendFormat("一、Single 查询唯一实体(有且仅有一个,多于一个会报错)：{0}", Environment.NewLine);
                //1、通过Id获取唯一实体
                var modelById = session.SingleById<User>(1);
                msg.AppendFormat("1、通过Id获取唯一实体:{0}{1}", modelById.name, Environment.NewLine);
                //2、自定义查询条件获取唯一实体
                var smodel = session.Single<User>("where id=@0 and name=@1", 1, "翟晓东");
                msg.AppendFormat("2、自定义查询条件获取唯一实体:{0}{1}", smodel.email, Environment.NewLine);
                //3、构建查询条件
                SearchSqlBuilder singleSql = new SearchSqlBuilder();
                singleSql.Create(cond => {
                    //自定义查询的列，不重写则默认为"*"
                    cond.Select("name", "email", "birthday");
                    cond.WhereIf(1 == 1, "id=@0", 2)
                        .WhereIf(1 == 12, "name=@0", "");
                    //指定排序
                    //cond.OrderBy("birthday DESC");
                });
                var smodel2 = session.Single<User>(singleSql);
                msg.AppendFormat("3、构建查询条件:{0}{1}", smodel2.name, Environment.NewLine);
                #endregion

                #region 二、First 查询匹配的第一个实体(有多个则返回集合中的第一条数据)
                msg.AppendFormat("二、First 查询匹配的第一个实体(有多个则返回集合中的第一条数据)：{0}", Environment.NewLine);
                //1、自定义查询条件获匹配的第一个实体
                var fmodel = session.First<User>("where name like @0", "费云帆%");
                msg.AppendFormat("1、自定义查询条件获匹配的第一个实体:{0}{1}", fmodel.name, Environment.NewLine);
                //2、构建查询条件
                SearchSqlBuilder firstSql = new SearchSqlBuilder();
                firstSql.Create(cond => {
                    //自定义查询的列，不重写则默认为"*"
                    //cond.Select("name", "email", "birthday");
                    cond.WhereIf(1 == 1, "convert(varchar(10),birthday,120)=@0", "2016-12-07")
                        .WhereIf(1 == 2, "name=@0", "fff");
                    //指定排序
                    cond.OrderBy("birthday DESC");
                });
                var fmodel2 = session.First<User>(firstSql);
                msg.AppendFormat("2、构建查询条件:{0}{1}", fmodel2.name, Environment.NewLine);
                #endregion

                #region 三、Query 获取实体集合
                msg.AppendFormat("三、Query 获取实体集合：{0}", Environment.NewLine);
                //1、自定义查询条件获取集合
                SearchSqlBuilder sqlList1 = new SearchSqlBuilder();
                sqlList1.Create(cond => {
                    //自定义查询的列，不重写则默认为"*"
                    //cond.Select("name", "email", "birthday");
                    cond.WhereIf(1 == 1, "convert(varchar(10),birthday,120)=@0", "2016-12-07")
                        .WhereIf(1 == 2, "name=@0", "fff");
                    //指定排序
                    cond.OrderBy("birthday DESC");
                });
                var modelList = session.Query<User>(sqlList1);
                msg.AppendFormat("1、自定义查询条件获取集合:{0}{1}", modelList.Count(), Environment.NewLine);
                //2、获取分页数据
                SearchSqlBuilder sqlList2 = new SearchSqlBuilder();
                sqlList2.CreatePage(cond => {
                    //自定义查询的列，不重写则默认为"*"
                    //cond.Select("name", "email", "birthday");
                    cond.WhereIf(1 == 12, "ISNULL(name,'')<>@0", "1")
                        .WhereIf(1 == 12, "id>@0", 0);
                    //指定排序
                    cond.OrderBy("id DESC");
                },1,3);
                var pageEntity = session.QueryPage<User>(sqlList2);
                msg.AppendFormat("2、获取分页数据:{0}{1}", pageEntity.TotalItems, Environment.NewLine);
                #endregion

                #region 四、执行Sql语句获取DataTable
                msg.AppendFormat("四、执行Sql语句获取DataTable：{0}", Environment.NewLine);
                //1、执行SQL语句
                var tb = session.ExcuteQuery("SELECT * FROM Users WHERE 1=1 AND id>@0", 1);
                msg.AppendFormat("1、执行SQL语句:{0}{1}", tb.Rows.Count, Environment.NewLine);
                //2、使用Sql类构建查询条件
                Sql sqlBuild = new Sql();
                sqlBuild.Select("*").From("NewsInfo").Where("Name<>@0", "").OrderBy("id DESC");
                var tb2 = session.ExcuteQuery(sqlBuild);
                msg.AppendFormat("2、使用Sql类构建查询条件:{0}{1}", tb2.Rows.Count, Environment.NewLine);
                #endregion

                #region 五、执行SQL命令
                msg.AppendFormat("五、执行SQL命令：{0}", Environment.NewLine);
                //1、执行SQL命令返回受影响的行数
                var result1 = session.ExcuteNoQuery("UPDATE Users SET Remark=@0 WHERE id=@1","更新测试",1);
                msg.AppendFormat("1、执行SQL命令返回受影响的行数:{0}{1}", result1, Environment.NewLine);
                //2、执行SQL命令返回第一行第一例的数据
                var result2 = session.ExcuteScalar<int>("SELECT COUNT(1) FROM NewsInfo WHERE 1=1");
                msg.AppendFormat("2、执行SQL命令返回第一行第一例的数据:{0}{1}", result2, Environment.NewLine);
                #endregion

                #region 六、获取动态类型
                var dynamic1 = session.Query<object[]>("SELECT * FROM Users where 1=1");
                var dynamic2 = session.DbContext.Fetch<dynamic>("SELECT * FROM Users where 1=1");
                var dynamic3 = session.DbContext.Fetch<Dictionary<string, object>>("SELECT * FROM NewsInfo where 1=1");
                #endregion
            }
            txtMessage.Value = msg.ToString();
        }

        protected void test2_Click(object sender, EventArgs e)
        {
            StringBuilder msg = new StringBuilder();
            //测试增删改
            using (var session = DefaultDbFactory.GetDbSession())
            {
                session.BeginTranscation();
                #region 一、自增主键实体 CUD
                msg.AppendFormat("一、自增主键实体 CUD：{0}", Environment.NewLine);
                //1、新增实体(Id自增长)
                User u = new User() { name = "张飞", email = "werew23123@qq.com", birthday = DateTime.Now };
                var LastId = session.Insert(u);
                msg.AppendFormat("1、新增实体(Id自增长),新增后返回的Id：{0} {1}", LastId, Environment.NewLine);
                var LastUser = session.SingleById<User>(LastId);
                msg.AppendFormat("1、新增实体(Id自增长),新增后重新获取的实体信息：{0} {1}", LastUser.name, Environment.NewLine);
                //2、更新实体
                LastUser.name = "张飞-New";
                LastUser.email = "new-1234343@qq.com";
                var updateResult1 = session.Update(LastUser);
                msg.AppendFormat("1、新增实体(Id自增长),更新后的结果：{0} {1}", updateResult1, Environment.NewLine);
                var updateUser = session.First<User>("where email=@0", "new-1234343@qq.com");
                msg.AppendFormat("1、新增实体(Id自增长),更新后获取用户的信息：{0} {1}", updateUser.name, Environment.NewLine);
                #endregion

                #region 二、GUID主键实体 CUD
                msg.AppendFormat("二、GUID主键实体 CUD：{0}", Environment.NewLine);
                //1、新增实体(Guid类型主键)
                News newsInfo = new News() { Name = "翟晓东", Title = "风云科技", Flag = true, Remark = "gsfdsdf" };
                var LastGuidId=session.Insert(newsInfo);
                msg.AppendFormat("1、新增实体(Guid类型主键),新增后返回的Id：{0} {1}", LastGuidId, Environment.NewLine);
                var LastUser2 = session.SingleById<News>(LastGuidId);
                msg.AppendFormat("1、新增实体(Guid类型主键),新增后重新获取的实体信息：{0} {1}", LastUser2.Name, Environment.NewLine);
                //2、更新实体
                //LastUser2.Name = "翟晓东-New3333333";
                session.DbContext.UpdateMany<News>().Where(x => x.Id== LastUser2.Id)
                    //.ExcludeDefaults()
                    .OnlyFields(x => x.Name)
                    .Execute(new News() { Name = "翟晓东-New3333333" });
                //var upr = session.Update(LastUser2);
                //msg.AppendFormat("1、新增实体(Guid类型主键),更新后的结果：{0} {1}", upr, Environment.NewLine);
                var updateUser2 = session.First<News>("where Name=@0", "翟晓东-New3333333");
                msg.AppendFormat("1、新增实体(Guid类型主键),新增后重新获取的实体信息：{0} {1}", updateUser2.Name, Environment.NewLine);
                #endregion
                session.Commit();
            }
            txtMessage.Value = msg.ToString();
        }
        
        protected void text3_Click(object sender, EventArgs e)
        {
            /*
            Debug.Assert(1 == 0, "error message", "参数不能为空！");
            System.Diagnostics.Trace.Assert(1==0,"错误：","个方法士大夫十分");
            TextWriterTraceListener _DebugLog = new TextWriterTraceListener(System.IO.File.CreateText("Debug_Output.txt"));
            Debug.Listeners.Add(_DebugLog);
            Debug.AutoFlush = true;
            Debug.WriteLine("Debug 余小章");
            System.Diagnostics.Trace.WriteLine("Release 余小章");
            */

            //测试存储过程
            StringBuilder msg = new StringBuilder();
            using (var session = DefaultDbFactory.GetDbSession(true))
            {
                msg.AppendFormat("五、调用存储过程：{0}", Environment.NewLine);
                var outputParam = new System.Data.SqlClient.SqlParameter("rtnStr", SqlDbType.VarChar ,2000);
                outputParam.Direction = ParameterDirection.Output;
                var dt=session.QueryStoredProcedure("SP_GetUser", new StoreParameter("id", 3), new StoreParameter("name", "翟晓东"),new StoreParameter("rtnStr", outputParam));
                msg.AppendFormat("1、调用存储过程，获取DataTable：{0} {1}",dt.Rows.Count, Environment.NewLine);

                var outputParam2 = new System.Data.SqlClient.SqlParameter("rtnStr", SqlDbType.VarChar, 2000);
                outputParam2.Direction = ParameterDirection.Output;
                var suserList = session.QueryStoredProcedure<User>("SP_GetUser", new StoreParameter("id", 3), new StoreParameter("name", ""), new StoreParameter("rtnStr", outputParam2));
                msg.AppendFormat("2、调用存储过程，获取List<T>：{0} {1}", suserList.Count(), Environment.NewLine);

                var outputParam3 = new System.Data.SqlClient.SqlParameter("rtnStr", SqlDbType.VarChar, 2000);
                outputParam3.Direction = ParameterDirection.Output;
                var singleDD = session.SingleStoredProcedure<User>("SP_GetUser", new StoreParameter("id", 2), new StoreParameter("name", ""), new StoreParameter("rtnStr", outputParam3));
                msg.AppendFormat("3、调用存储过程，获取单个实体：{0} {1}", singleDD.name, Environment.NewLine);
                
            }
            txtMessage.Value = msg.ToString();
        }

        protected void text4_Click(object sender, EventArgs e)
        {
            //测试删除
            StringBuilder msg = new StringBuilder();
            using (var session = DefaultDbFactory.GetDbSession(true))
            {
                msg.AppendFormat("六、测试删除：{0}", Environment.NewLine);
                //1、删除指定实体
                var user = session.SingleById<User>(1);
                int delFlag = session.Delete(user);
                msg.AppendFormat("1、删除指定实体，返回结果：{0} {1}", delFlag, Environment.NewLine);
                //2、通过主键删除
                var delFlag2=session.Delete<User>(3);
                msg.AppendFormat("2、通过主键删除，返回结果：{0} {1}", delFlag2, Environment.NewLine);
                session.Commit();
            }
            txtMessage.Value = msg.ToString();
        }
    }
}