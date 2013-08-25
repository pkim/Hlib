using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handler.Interface.HLib.Network.IP.HTTP
{
    public class HTTPParameter
    {
        public String Name  { get; set; }
        public String Value { get; set; }

        public HTTPParameter(String _name, String _value)
        {
            this.Name  = _name;
            this.Value = _value;
        }

        public override String ToString()
        {
            return String.Format("{0}={1}", this.Name, this.Value);
        }
    }
}
