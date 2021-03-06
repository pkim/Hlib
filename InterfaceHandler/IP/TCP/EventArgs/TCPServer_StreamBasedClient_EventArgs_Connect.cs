﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Handler.NetworkHandler.IP.TCP
{
    public class TCPServer_StreamBasedClient_EventArgs_Connect : System.EventArgs
    {
        private TcpClient  tcpClient;
        private SocketInfo socketInfo;

        public TCPServer_StreamBasedClient_EventArgs_Connect(TcpClient _tcpClient)
        {
            this.tcpClient  = _tcpClient;
            this.socketInfo = SocketInfo.getSocketInfo(_tcpClient);      
        }

        public SocketInfo getSocketInfo()
        {
            return this.socketInfo;
        }

        public TcpClient getClient()
        {
            return this.tcpClient;
        }

    }
}
