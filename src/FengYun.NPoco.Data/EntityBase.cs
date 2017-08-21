using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FengYun.NPoco.Data
{

    [Serializable]
    public abstract class Entity<Tkey> : IEntity<Tkey>
    {
        public virtual Tkey Id { get; set; }

        /// <summary>
        /// 判断是否有ID值
        /// </summary>
        /// <returns>如果是临时对象,则为True</returns>
        public virtual bool IsTransient()
        {
            return EqualityComparer<Tkey>.Default.Equals(Id, default(Tkey));
        }

    }
    /// <summary>
    /// 默认基类 GUID主键
    /// </summary>
    [Serializable]
    [PrimaryKey("Id", AutoIncrement = false)]
    public abstract class Entity : Entity<Guid>, IEntity
    {

    }
}
