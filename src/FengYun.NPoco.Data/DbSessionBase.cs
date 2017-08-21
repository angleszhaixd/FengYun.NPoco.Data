using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;

namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 仓储基类
    /// author：zhaixd
    /// date:2016.08.12
    /// </summary>
    public class DbSessionBase : ITransaction
    {
        /// <summary>
        /// 是否开启事物
        /// </summary>
        protected bool IsTranscation { get; set; }
        private INPocoDataBase _DataContext = null;
        protected INPocoDataBase DataContext
        {
            get { return _DataContext; }
            set
            {
                _DataContext = value;
            }
        }

        public void BeginTranscation()
        {
            IsTranscation = true;
            DataContext.BeginTransaction();
        }

        public void Commit()
        {
            DataContext.CompleteTransaction();
            IsTranscation = false;
        }

        public void Rollback()
        {
            DataContext.AbortTransaction();
            IsTranscation = false;
        }

        public void Dispose()
        {
            if (DataContext == null)
                return;
            if (IsTranscation)
                DataContext.AbortTransaction();
            DataContext.Dispose();
        }

        #region Custom_Method
        public void DebugInfo()
        {
            _DataContext.OnException(arg =>
            {
                //出错时错误处理
            });

            _DataContext.OnExecutingCommand(arg => 
            {
                //准备执行sql命令时执行
            });
        }
        #endregion
    }
}
