using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handler.NetworkHandler.IP.TCP
{
    public class TCPServer_EventArgs_MessageRecieved : System.EventArgs
    {
        /// <summary>
        /// This object includs a string which is the message from the client and SocketInfo object which contains IP and Port of the client
        /// </summary>
        private ClientMessage clientMessage;

        public TCPServer_EventArgs_MessageRecieved(ClientMessage _clientMessage)
        {
            this.clientMessage = _clientMessage;
        }

        public ClientMessage getClientMessage()
        {
            return this.clientMessage;
        }

    }
}
