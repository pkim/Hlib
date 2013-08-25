using System;

namespace HLib.Settings.HotKey
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class HotKeyAttribute : Attribute
    {
        public Object HotKey { get; protected set; }

        public HotKeyAttribute(Object _hotKey)
        {
            this.HotKey = _hotKey;
        }
    }  

}
