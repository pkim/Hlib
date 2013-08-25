using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NetworkHandler.IP
{
    public class SocketInfo
    {
        /// <summary>
        /// IPAddress of the socket
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// Port of the socket
        /// </summary>
        public Int32     Port      { get; set; }

        /// <summary>
        /// Generates a String which includes the IPAddress and the Port of the socket
        /// </summary>
        /// <returns>
        /// Returns the generated String
        /// </returns>
        public override string ToString()
        {
            if (this.IPAddress != null && this.Port != 0)
                return String.Format("IPAddress: {0} \nPort: {1}", this.IPAddress.ToString(), this.Port);

            return null;
        }

        /// <summary>
        /// Initializes a configured SocketInfo-Object and returns it.
        /// </summary>
        /// <param name="_tcpClient">
        /// the Object which contains the connected client-Object. This Object can be a TcpClient-Object
        /// if the TCPServer is StreamBased or a Socket-Object if the TCPServer is ByteBased.
        /// </param>
        /// <returns>Return a SocketInfo-Object witch setted variables from the _tcpClient Object</returns>
        public static SocketInfo getSocketInfo(Object _tcpClient)
        {
            // the string will save the EndO
            String remoteEndPoint = "";

            switch(_tcpClient.GetType().Name)
            {

                // if the TCPServer is StreamBased
                case "TcpClient":
            
                    // set _tcpClient as TcpClient
                    TcpClient tcpClient = _tcpClient as TcpClient;

                    // RemoteEndPoint
                    if(tcpClient != null)
                        remoteEndPoint = tcpClient.Client.RemoteEndPoint.ToString();

                    break;


                // if the TCPServer is ByteBased
                case "Socket":
                    
                    // set the _tcpClient as Socket
                    Socket client  = _tcpClient as Socket;
                    
                    // RemoteEndPoint
                    if(client != null)
                        remoteEndPoint = client.RemoteEndPoint.ToString();
                    
                    break;

                default:

                    // Falls ein nicht identifizierbares Object mit dieser methode aufgerufen wird
                    return null;

            }

            // TCP-CLient IPaddress/Port
            IPAddress remoteEndPoint_IPAddress = IPAddress.Parse(remoteEndPoint.Substring(0, remoteEndPoint.IndexOf(':')));
            Int32     remoteEndPoint_Port      = Int32.Parse(remoteEndPoint.Substring(remoteEndPoint.IndexOf(':') + 1));

            // SocketInfo
            SocketInfo socketInfo = new SocketInfo();

            //sets the variables of the SocketInfo-Object
            socketInfo.IPAddress = remoteEndPoint_IPAddress;    // Sets the IPAddress
            socketInfo.Port      = remoteEndPoint_Port;         // Sets the Port

            // return the SocketInfo which contains the RemoteEndPoint information
            return socketInfo;

        }     
        
    }
}
