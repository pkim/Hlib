using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace NetworkHandler.IP.TCP
{
    /// <summary>
    /// 
    /// </summary>
    public class TCPServer_ByteBased : TCPServer
    {

       #region Constructor

        /// <summary>
        /// Constructor of a ByteBased TCPServer.
        /// Initializes new a TCPServer_ByteBased-Object
        /// </summary>
        public TCPServer_ByteBased() : base()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ipAddress">The IPAddress of the TCPServer on which it is listening</param>
        /// <param name="_port">The Port of the TCPServer on which it is listening</param>
        public TCPServer_ByteBased(IPAddress _ipAddress, Int32 _port) : base(_ipAddress, _port)
        { }

        #endregion

        ///<summary>
        /// starts an endless loop which accepts new clients
        ///</summary>
        protected override void ListenForClients()
        {
            // calls the ListenForClients method from the base class
            base.ListenForClients();

            // While the Server isn't crashed or STOPPED
            while (this.status != ServerStatus.STOPPED)
            {

                //blocks until a client has connected to the server
                if (this.MaximalAllowedClients == 0 || this.clients.Count < this.MaximalAllowedClients)
                {

                    // blocks until incomming connection is detected
                    Socket socket = this.tcpListener.AcceptSocket();

                    // Add new Client to the list of clients
                    this.clients.Add(socket);

                    // create a thread to handle the communication with the connected client
                    this.clientThreads.Add(new Thread(new ParameterizedThreadStart(this.HandleClientCommunication)));
                    this.clientThreads[this.clientThreads.Count - 1].Start(socket);
                }
            }
        }

        private void HandleClientCommunication(Object _socket)
        {
            // Socket
            Socket socket = (Socket)_socket;

            // Buffer
            byte[] buffer;

            // Client Message
            ClientMessage clientMessage = new ClientMessage();
            clientMessage.SocketInfo = SocketInfo.getSocketInfo(socket);
            
            // Buffer
            buffer = new byte[this.MaxMessageSize];     // initializes a buffer; the MaxMessageSize is a globla Varibale in the TCPServer-Class

            // while the isn't STOPPED
            while (this.Status != ServerStatus.STOPPED)
            {
                // if the server is RUNNING
                if (this.Status == ServerStatus.RUNNING)
                {

                    // Wait until a message recieved
                    socket.Receive(buffer);                     // save the recieved message  
                    
                    clientMessage.Message = this.MessageEncoding.GetString(buffer);     // save the Message in in the clientMessage-Object

                    /* Call the Event MessageRecieved */
                    //this.MessageRecieved(this, clientMessage);
                }
            }

        }


    }
}
