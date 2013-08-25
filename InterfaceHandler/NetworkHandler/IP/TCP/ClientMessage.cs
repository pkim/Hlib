using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkHandler.IP;

namespace NetworkHandler.IP.TCP
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
