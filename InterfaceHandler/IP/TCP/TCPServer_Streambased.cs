using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace HLib.Network.IP.TCP
{
    /// <summary>
    /// 
    /// </summary>
    public class TCPServer_Streambased : TCPServer
    {

        #region Objects

        private NetworkStream clientStream;

        #endregion

        #region Eventdeklaration

        /// <summary>
        /// This event accures if a message from a client recieved
        /// </summary>
        public event MessageRecieved_Handler MessageRecieved;

        /// <summary>
        /// This event accures if a client is connecting to the server
        /// </summary>
        public event ClientConnect_Handler ClientConnect;

        /// <summary>
        /// This event accures if a client is disconnecting from the server
        /// </summary>
        public event ClientDisconnect_Handler ClientDisconnect;

        /// <summary>
        /// This event accures if the connection of th client aborts suddenly
        /// </summary>
        public event ClientConnectionAborted_Handler ClientConnectionAborted;

        #endregion


        #region Constructor

        public TCPServer_Streambased() : base()
        { }

        public TCPServer_Streambased(IPAddress _IPAddress, Int32 _Port) : base(_IPAddress, _Port)
        { }

        #endregion


        ///<summary>
        /// Stops the TCPServer
        ///</summary>
        public override void Stop()
        {

            if (this.clientStream != null)
            {
                this.clientStream.Flush();
                this.clientStream.Close();
            }


            base.Stop();      
        }

        ///<summary>
        /// starts an endless loop which accepts new clients
        ///</summary>
        protected override void ListenForClients()
        {
            
            base.ListenForClients();

            while(this.Status != ServerStatus.STOPPED)
            {
                
                /* blocks until a client has connected to the server and another client is allowed
                   MaximalAllowedClients means no limit                                             */
                if (this.MaximalAllowedClients == 0 || this.clients.Count < this.MaximalAllowedClients)
                {
                
                    // Initialize a new TCPCLient if an client wants to connect
                    TcpClient tcpClient = this.tcpListener.AcceptTcpClient();
                    
                    TCPServer_StreamBasedClient_EventArgs_ClientConnect tcpServer_StreamBasedClient_EventArgs_Connect = new TCPServer_StreamBasedClient_EventArgs_ClientConnect(tcpClient);
                    this.ClientConnect(this, tcpServer_StreamBasedClient_EventArgs_Connect);

                    // Add this new tcpClient to the clients list which includes each connected client
                    this.clients.Add(tcpClient);

                    /* create a thread to handle communication
                     * with connected client                   */
                    Thread clientThread = new Thread(new ParameterizedThreadStart(this.HandleClientCommunication));

                    this.clientThreads.Add(clientThread);
                    this.clientThreads[this.clientThreads.IndexOf(clientThread)].Start(tcpClient);
                }

            }
        }


        ///<summary>
        /// Handles clientcommuniction
        /// <param name="_client"> _client which communication should be handled</param>
        ///</summary>
        private void HandleClientCommunication(Object _client)
        {

            /* TCPClient and Stream */
            TcpClient     tcpClient    = (TcpClient)_client;
            clientStream = tcpClient.GetStream();

            /* SocketInfo */  
            SocketInfo socketInfo = SocketInfo.getSocketInfo(tcpClient);

            /* Client Message */
            ClientMessage clientMessage = new ClientMessage();
            clientMessage.SocketInfo = SocketInfo.getSocketInfo(tcpClient);
            
            /* buffer and received_message */
            Byte[] message_byte = new byte[this.MaxMessageSize];
            Int32  bytesRead;


            while ( this.Status != ServerStatus.STOPPED )
            {
                if (this.Status == ServerStatus.RUNNING)
                {
                    bytesRead = new Int32();

                    try
                    {
                        // blocks until a client sends a message
                        bytesRead = clientStream.Read(message_byte, 0, this.MaxMessageSize);
                    }

                    catch
                    {
                        // a socket error has occured
                        // throw new Exception("Server: " + "a socket error has occured; Client: " + tcpClient.Client.LocalEndPoint.ToString() + " has interrupted the connection");
                        TCPServer_StreamBasedClient_EventArgs_ClienConnectionAbort clientConnectionAborted_Event = new TCPServer_StreamBasedClient_EventArgs_ClienConnectionAbort(tcpClient);
                        this.ClientConnectionAborted(this, clientConnectionAborted_Event);

                        System.Console.WriteLine("Server: " + "a socket error has occured; Client: " + "adresse" + " has interrupted the connection");
                        break;
                    }


                    if (bytesRead == 0)
                    {
                        // the client has disconnected from the server
                        // throw new Exception("Server: " + "a client " + tcpClient.Client.RemoteEndPoint.ToString() + " has disconnected from the server");
                        TCPServer_StreamBasedClient_EventArgs_ClientDisconnect clientDisconnect_Event = new TCPServer_StreamBasedClient_EventArgs_ClientDisconnect(tcpClient);
                        this.ClientDisconnect(this, clientDisconnect_Event);
                        
                        System.Console.WriteLine("Server: " + "a client " + "adresse" + " has disconnected from the server");
                        break;
                    }


                    /* recieved Message from Client */
                    clientMessage.Message = this.MessageEncoding.GetString(message_byte, 0, bytesRead);

                    /* Call the Event MessageRecieved */
                    TCPServer_EventArgs_MessageRecieved tcpServer_EventArgs_MessageRecieved = new TCPServer_EventArgs_MessageRecieved(clientMessage);
                    this.MessageRecieved(this, tcpServer_EventArgs_MessageRecieved);

                }
            }



            // Close open connections
            if (clientStream != null)
            {
                clientStream.Flush();
                clientStream.Close();
            }

            // if the tcpClient isn't null
            if (tcpClient != null)
            {
                // remove tcpClient from list clients
                this.clients.RemoveAt(this.clients.IndexOf(tcpClient));
            }

            // if the tcpClient is still connected
            if (tcpClient.Connected)
            {
                // close the tcpClient
                tcpClient.Close();
            }

            // remove this thread from list clientsThread
            this.clientThreads.RemoveAt(this.clientThreads.IndexOf(Thread.CurrentThread));
        }


        /// <summary>
        /// sends a message to the connected client
        /// </summary>
        /// <param name="_message">the message to send</param>
        public void Send(String _message)
        {
            Byte[] buffer = new byte[this.MaxMessageSize];
            buffer = Encoding.UTF8.GetBytes(_message);

            if (this.clientStream != null && this.clientStream.CanWrite)
            {
                // send
                this.clientStream.Write(buffer, 0, buffer.Length);
                this.clientStream.Flush();
            }

        }

    }
}
