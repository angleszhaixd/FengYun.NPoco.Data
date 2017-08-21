using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FengYun.NPoco.Data
{
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTranscation();
        /// <summary>
        /// 提交
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();
    }
}
