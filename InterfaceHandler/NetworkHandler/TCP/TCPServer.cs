/*
 * Name: TCPServer
 * Date: 20 Februar 2011
 * Author: Patrik Kimmeswenger
 * Description: initializes a TCPServer
*/


using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using Handler.FileHandler;
using System.Collections.Generic;
using System.Diagnostics;


namespace Handler.NetworkHandler.TCP
{
   
    public class TCPServer
    {
     
        #region objects

        /* server information */
        private int       port;
        private IPAddress ipAddress;


        /* Status */
        public enum ServerStatus { running, stopped, paused };
        private ServerStatus status;


        /* server Listener, clients and Threads */
        private TcpListener tcpListener;
        private Thread      listenThread;
        private List<Thread> clientThreads;
        private List<TcpClient> clients;

        #endregion


        #region proberties

        public ServerStatus Status
        {
            get { return this.status; }
        }

        public IPAddress IPAddress {
            
            get
            {
                return this.ipAddress;
            }

            set
            {
                this.ipAddress = value;
            }
        }

        public Int32 Port
        {
            get
            {
                return this.port;
            }

            set
            {
                if (value > 0 && value <= 65536)
                    this.port = value;
            }
        }

        public Int32 MaxMessageSize { get; set; }

        #endregion


        #region delagetes

        public delegate void MessageRecieved();

        #endregion


        #region constructor

        public TCPServer()
        {
            this.tcpListener   = new TcpListener(this.ipAddress, this.port);
            this.clientThreads = new List<Thread>();
            this.clients       = new List<TcpClient>();

            this.status = new ServerStatus();
            this.status = ServerStatus.stopped; 
        }

        public TCPServer(string _IPAddress, int _port) : this()
        {
            this.Port = _port;
            this.IPAddress = this.convertToIPAddress(_IPAddress); 
        }

  
        #endregion


        #region methods


        ///<summary>
        /// starts the TCPServer
        ///</summary>
        public void start()
        {
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
            this.status = ServerStatus.running;
        }


        ///<summary>
        /// stops the TCPServer
        ///</summary>
        public void stop()
        {

            if (this.status == ServerStatus.running || this.status == ServerStatus.paused)
            {
                foreach (TcpClient tcpclient in this.clients)
                {
                    if(tcpclient.Connected)
                        tcpclient.Close();
                }

                foreach (Thread clientThrad in this.clientThreads)
                {
                    if (clientThrad.IsAlive)
                        clientThrad.Abort();
                }

                if(this.listenThread.IsAlive)
                {
                    this.listenThread.Abort();
                }

                this.tcpListener.Stop();
             
            }

            this.status = ServerStatus.stopped;

        }


        /// <summary>
        /// check if the TCPSercer is able to start 
        /// with his current settings
        /// </summary>
        /// 
        /// <returns>
        /// true if the server is able to start
        /// otherwise false
        /// </returns>
        public bool canStart()
        {
            try
            {
                this.tcpListener.Start();
                this.tcpListener.Stop();
                return true;
            }

            catch (SocketException)
            {
                this.tcpListener.Stop();
                return false;
            }
        }


        ///<summary>
        /// starts an endless loop which accepts new clients
        ///</summary>
        private void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();
                this.status = ServerStatus.running;
            }
            catch (SocketException)
            {
                MessageBox.Show("starting server on port " + this.Port + " failed");
                this.stop();
                return;
            }

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();
                this.clients.Add(client);

                /* create a thread to handle communication
                 * with connected client                   */
                this.clientThreads.Add(new Thread(new ParameterizedThreadStart(HandleClientComm)));
                this.clientThreads[this.clientThreads.Count - 1].Start(client);
            }
        }


        ///<summary>
        /// Handles clientcommuniction
        /// 
        /// <param name="_client"> _client which communication should be handled</param>
        ///</summary>
        private void HandleClientComm(Object _client)
        {
            TcpClient     tcpClient    = (TcpClient)_client;
            NetworkStream clientStream = tcpClient.GetStream();


            /* buffer and received_message */
            byte[] message_byte = new byte[this.MaxMessageSize];
            int    bytesRead;
            string recieved_string;
            

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message_byte, 0, this.MaxMessageSize);
                }

                catch
                {
                    //a socket error has occured
                    //MessageBox.Show("Server: " + "a socket error has occured; Client: " + tcpClient.Client.LocalEndPoint.ToString() + " has interrupted the connection");
                    break;
                }

                finally
                {
                    clientStream.Close();
                    tcpClient.Close();
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    //MessageBox.Show("Server: " + "a client(" + tcpClient.Client.RemoteEndPoint.ToString() + ") has disconnected from the server");
                    break;
                }


                /* recieved Message from Client */
                recieved_string = new ASCIIEncoding().GetString(message_byte, 0, bytesRead);      

            }

            tcpClient.Close();
        }
       


        private IPAddress convertToIPAddress(string _ipAdress)
        {
            return IPAddress.Parse (_ipAdress);
        }

        #endregion

    }
}
