using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;
using System.Data.Common;
using System.Data;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 自实现DataBase类型，重载相应方法
    /// </summary>
    public class NPocoDataBase : Database,INPocoDataBase
    {
        /// <summary>
        /// 最后执行的sql语句
        /// </summary>
        public string LastExcuteSQL
        {
            get
            {
                return string.Format("sql语句:{0},参数:{1}", this.LastSQL, string.Join(",",this.LastArgs.ToArray())); 
            }
        }
        public NPocoDataBase(string connectionStringName) : base(connectionStringName) { }
        public NPocoDataBase(DbConnection conn) : base(conn) { }

        #region Override Method
        public Action<DbCommand> OnExecutingAction { get; set; }
        public Action<Exception> OnExceptionAction { get; set; }
        public Func<UpdateContext, bool> OnUpdatingFun { get; set; }
        public Func<InsertContext, bool> OnInsertingFun { get; set; }
        public Func<DeleteContext, bool> OnDeletingFun { get; set; }

        protected override void OnExecutingCommand(DbCommand cmd)
        {
            if (OnExecutingAction != null)
                OnExecutingAction(cmd);
        }
        protected override void OnException(Exception e)
        {
            e.Data["LastSQL"] = this.LastSQL; //记录最后一条sql语句
            e.Data["LastArgs"] = this.LastArgs;
            if (OnExceptionAction != null)
                OnExceptionAction(e);
        }
        protected override bool OnUpdating(UpdateContext updateContext)
        {
            //var entity = updateContext.Poco as CommonEntity;
            //if (entity != null)
            //{
            //    entity.DateModified = DateTime.UtcNow;
            //}
            if (OnUpdatingFun != null) {
                var flag=OnUpdatingFun(updateContext);
            }
            return base.OnUpdating(updateContext);
        }
        protected override bool OnInserting(InsertContext insertContext)
        {
            var PocoEntity = insertContext.Poco;
            if (PocoEntity is IEntity<Guid>)
            {
                //GUID类型默认赋值
                var entity = PocoEntity as IEntity<Guid>;
                if (entity.IsTransient())
                {
                    entity.Id = Guid.NewGuid();
                }
            }
            if (OnInsertingFun != null)
            {
                var flag = OnInsertingFun(insertContext);
            }
            return base.OnInserting(insertContext);
        }
        protected override bool OnDeleting(DeleteContext deleteContext)
        {
            if (OnDeletingFun != null)
            {
                var flag = OnDeletingFun(deleteContext);
            }
            return base.OnDeleting(deleteContext);
        }
        #endregion
    }
}
