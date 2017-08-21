using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;
using System.Data.Common;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// db上下文接口
    /// </summary>
    public interface INPocoDataBase:IDatabase
    {
        string LastExcuteSQL { get; }
        Action<DbCommand> OnExecutingAction { get; set; }
        Action<Exception> OnExceptionAction { get; set; }
        
        Func<UpdateContext,bool> OnUpdatingFun { get; set; }
        Func<InsertContext, bool> OnInsertingFun { get; set; }
        Func<DeleteContext, bool> OnDeletingFun { get; set; }
    }
}
