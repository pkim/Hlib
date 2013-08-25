using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handler.Item.Singleton
{
    /// <summary>
    /// SingletonBase
    /// </summary>
    /// <typeparam name="SingletonBase<T>">SingletonBase</typeparam>
    public abstract class SingletonBase<T>
                where T : SingletonBase<T>
    {

        public static T GetInstance(out bool New)
        {
            return SingletonProvider.GetInstance<T>(out New);
        }

        public static T GetInstance()
        {
            bool _Trash;
            return GetInstance(out _Trash);
        }

    }
}
