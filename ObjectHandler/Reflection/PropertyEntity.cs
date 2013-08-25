using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLib.Item.Reflection
{
    public class PropertyEntity
    {
        public String Name  { get; set; }

        public Object Value { get; set; }

        public Type   Type  { get; set; }
    }
}
