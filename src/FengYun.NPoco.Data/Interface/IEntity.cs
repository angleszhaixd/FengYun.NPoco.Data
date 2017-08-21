using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FengYun.NPoco.Data
{
    public interface IEntity : IEntity<Guid>
    {

    }
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }

        bool IsTransient();
    }
}
