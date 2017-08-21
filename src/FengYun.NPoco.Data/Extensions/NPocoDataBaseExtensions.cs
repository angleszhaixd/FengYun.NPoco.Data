using NPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;


namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 数据库回话事件监听
    /// author:zhaixd
    /// datetime:2016.12.05
    /// </summary>
    public static class NPocoDataBaseExtensions
    {
        public static INPocoDataBase OnExecutingCommand(this INPocoDataBase database, Action<DbCommand> action)
        {
            database.OnExecutingAction = action;
            
            return database;
        }

        public static INPocoDataBase OnException(this INPocoDataBase database, Action<Exception> action)
        {
            database.OnExceptionAction = action;
            return database;
        }

        public static INPocoDataBase OnUpdating(this INPocoDataBase database, Func<UpdateContext, bool> func)
        {
            database.OnUpdatingFun = func;
            return database;
        }

        public static INPocoDataBase OnInserting(this INPocoDataBase database, Func<InsertContext, bool> func)
        {
            database.OnInsertingFun = func;
            return database;
        }

        public static INPocoDataBase OnDeleting(this INPocoDataBase database, Func<DeleteContext, bool> func)
        {
            database.OnDeletingFun = func;
            return database;
        }
         
    }
}
