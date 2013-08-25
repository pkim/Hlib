using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLib.Network.IP;

namespace HLib.Network.IP.TCP
{
    public class ClientMessage
    {
        public String     Message    { get; set; }
        public SocketInfo SocketInfo { get; set; }

        public override String ToString()
        {
            return String.Format("Message: {0} \nSocketInfo \n{1}", this.Message, this.SocketInfo);
        }

    }

    
}
