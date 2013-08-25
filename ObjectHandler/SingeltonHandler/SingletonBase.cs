using System;

namespace HLib.Item.Singleton
{
    /// <summary>
    /// SingletonBase
    /// </summary>
    /// <typeparam name="SingletonBase<T>">SingletonBase</typeparam>
    public abstract class SingletonBase<T>
                where T : SingletonBase<T>
    {

        public static T GetInstance(out Boolean New)
        {
            return SingletonProvider.GetInstance<T>(out New);
        }

        public static T GetInstance()
        {
            Boolean _Trash;
            return GetInstance(out _Trash);
        }

    }
}
