using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 单例依赖创建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SingletonDependency<T>
        where T:class,new()
    {
        public static T Instance { get { return LazyInstance.Value; } }

        private static readonly Lazy<T> LazyInstance;
        static SingletonDependency()
        {
            LazyInstance = new Lazy<T>(() => new T());
        }
    }
}
