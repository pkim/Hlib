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

using HLib.Network.IP;
using System.Collections.Generic;
using System.Diagnostics;


namespace HLib.Network.IP.TCP
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class TCPServer
    {

        #region Constant Values

        #endregion


        #region Objects

        /* server information */
        private Int32       port;                             // the port on which the server is listening
        private IPAddress ipAddress;                        // the IPAddress of the interface on which the server is listening

        /* Status */
        public enum      ServerStatus { RUNNING, STOPPED, PAUSED }; // The possible status of the server
        protected        ServerStatus status;                       // The current status of the server

        /* server Listener, clients and Threads */

        protected TcpListener  tcpListener;         // listens on interface, if there is a ingoing connection
        private   Thread       listenThread;        // the thread in which the tcpListener is listening
        protected List<Object> clients;             // the list of clients; Each client which is has an active session is in this list
        protected List<Thread> clientThreads;       // the thread in which the clients get initialized
        
        #endregion


        #region Proberties

        /// <summary>
        /// The name of the TCPServer
        /// </summary>
        /// <value>Name of the TCPServer as String. Default is "TCPServer"</value>
        public String Name
        { get; set; }

        /// <summary>
        /// The current status of the server
        /// </summary>
        /// <value>
        /// Serverstatus is an enumartion and has "RUNNING, STOPPED, PAUSED" as attribute
        /// </value>
        public ServerStatus Status
        {
            get { return this.status; }

        }

        /// <summary>
        /// The IPAddress of the TCPServer on which it is listening.
        /// </summary>
        public IPAddress IPAddress
        {
            get
            {
                return this.ipAddress;
            }

            set
            {
                this.ipAddress = value;
            }
        }

        /// <summary>
        /// The port of the server on which it is listening
        /// <value>The port can be an Integer from 1-65536</value>
        /// </summary>
        public Int32 Port
        {
            get
            {
                return this.port;
            }

            set
            {
                if (value >= IPHandler.PORT_RANGE_MIN && value <= IPHandler.PORT_RANGE_MAX)
                    this.port = value;
            }
        }

        /// <summary>
        /// sets maximum of recieving messagesize 
        /// the default is <value>1500</value> Bytes
        /// </summary>
        public Int32 MaxMessageSize 
        { get; set; }

        /// <summary>
        /// sets maximum of allowed clients.
        /// Default is 0 which means no limit
        /// </summary>
        public Int32 MaximalAllowedClients 
        { get; set; }

        /// <summary>
        /// Sets de Encoding algorythmus which should be used to
        /// encode the messages.
        /// Default is <value>UTF8</value> encoding
        /// </summary>
        public Encoding MessageEncoding 
        { get; set; }

        /// <summary>
        /// return the number of connected clients
        /// </summary>
        public Int32 NumberOf_ConnectedClients
        {
            get
            {
                return this.clients.Count;
            }
        }

        #endregion


        #region Delagetes

        /// <summary>
        /// This delegate handles recieving messages from the client
        /// </summary>
        /// <param name="sender">The Class where this event has accured</param>
        /// <param name="clientMessage">The EventArgs includes a ClientMessage Object which contains the Message and some socketinformations from the client</param>
        public delegate void MessageRecieved_Handler (Object sender, TCPServer_EventArgs_MessageRecieved client_Message);

        /// <summary>
        /// This delegate handles connecting clients
        /// </summary>
        /// <param name="sender">The Class where this event has accured</param>
        /// <param name="client_SocketInfo">The EventArgs inclueds a SocketInformation ( IP, Port ) from the connecting client</param>
        public delegate void ClientConnect_Handler   (Object sender, TCPServer_StreamBasedClient_EventArgs_ClientConnect client_SocketInfo);

        /// <summary>
        /// This delegate handles disconnecting clients
        /// </summary>
        /// <param name="sende">The Class where this event has accured</param>
        /// <param name="client_SocketInfo">The EventArgs inclueds a SocketInformation ( IP, Port ) from the connecting client</param>
        public delegate void ClientDisconnect_Handler(object sende, TCPServer_StreamBasedClient_EventArgs_ClientDisconnect client_SocketInfo);


        /// <summary>
        /// This delegate handles clients which connection aborts suddenly
        /// </summary>
        /// <param name="sender"The Class where this event has accured</param>
        /// <param name="client_SocketInfo">The EventArgs inclueds a SocketInformation ( IP, Port ) from the connecting client</param>
        public delegate void ClientConnectionAborted_Handler(object sender, TCPServer_StreamBasedClient_EventArgs_ClienConnectionAbort client_SocketInfo);


        /// <summary>
        /// This delegate handles the stopping of the server
        /// </summary>
        /// <param name="sender">The Class where this event has accured</param>
        /// <param name="?"></param>
        public delegate void ServerStopped_Handler(object sender);
        
        

        #endregion

 
        #region Constructor

        /// <summary>
        /// the basic constructor which initializes a TCPServer-Object
        /// and the sets the values of the Object which therefore are 
        /// needed to start the TCPServer successfully afterwards
        /// </summary>
        public TCPServer()
        {
            // TCP Objects
            this.clientThreads = new List<Thread>();    // initializes a list of threads which contains the active clients processes
            this.clients       = new List<Object>();    // initializes a list of threads which contains the active clients
            
            // Status
            this.status   = new ServerStatus();         // initializes the ServerStatus-Object which represents the current status of the server
            this.status   = ServerStatus.STOPPED;       // sets the ServerStatus to STOPPED as default, because the server isn't able to start in this state

            // MaxMessageSize
            this.MaxMessageSize = 1500;                 // maximal 1500 Bytes can a recieved message have 

            // MaxClients
            this.MaximalAllowedClients = new Int32();             // no limited number of allowed clients

            // Encoding
            this.MessageEncoding = new UTF8Encoding();  // the Encoding algorithmus which is uses to encode the recieved messages
        
            // Name of the TCPServer
            this.Name = "TCPServer";                    // sets the Name of the TCPServer as global variable
           
        }

        /// <summary>
        /// the constructor which initializes a TCPServer-Object
        /// and the sets the values of the Object which therefore are 
        /// needed to start the TCPServer successfully afterwards
        /// </summary>
        /// <param name="_ipAddress">The IPAddress of the interface on which the TCPServer should listen</param>
        /// <param name="_port">The Port on which the TCPServer should listen</param>
        public TCPServer(IPAddress _ipAddress, Int32 _port) : this()
        {
            this.Port        = _port;         // sets the port as global variable
            this.IPAddress   = _ipAddress;    // sets the IPAdress as global variable
        }

        /// <summary>
        /// the constructor which initializes a TCPServer-Object
        /// and the sets the values of the Object which therefore are 
        /// needed to start the TCPServer successfully afterwards
        /// </summary>
        /// <param name="_ipAddress">The IPAddress of the interface on which the TCPServer should listen</param>
        /// <param name="_port">The Port on which the TCPServer should listen</param>
        /// <param name="_name">The Name of the TCPServer</param>
        public TCPServer(IPAddress _ipAddress, Int32 _port, String _name) : this()
        {
            this.Port      = _port;             // sets the port as global variable
            this.IPAddress = _ipAddress;        // sets the IPAdress as global variable

            this.Name = _name;                  // sets the Name of the TCPServer as global variable
        }

        #endregion


        #region Methods

        ///<summary>
        /// starts the TCPServer
        ///</summary>
        public void Start()
        {
            // if the setted IPAddress isn't null and 
            // the setted Port isn't 0, means not configured
            if (this.ipAddress != null && this.port != 0)
            {
                // initialize a new TCPListener with the seted IPAddress and Port
                this.tcpListener = new TcpListener(this.ipAddress, this.port);
            }

            // else throw an Exception that report that the IPAddress or Port either isn't configured
            else throw new Exception("No IPAddress or Port is setted");


            this.listenThread = new Thread(new ThreadStart(ListenForClients));  // initialize a new Thread that works as "Listen for Clients"-Thread
            this.listenThread.Start();                                          // start the listenThread; since yet the server is able to accept clients

            this.status   = ServerStatus.RUNNING;   // switch the ServerStatus to "RUNNING"
        }


        ///<summary>
        /// Stops the TCPServer
        ///</summary>
        public virtual void Stop()
        {
            
            // if the Server is running or paused
            if (this.status == ServerStatus.RUNNING || this.status == ServerStatus.PAUSED)
            {

                try
                {
                    // Set the status to STOPPED each endless while loop is running as long 
                    // as the ServerStatus isn't STOPPED 
                    this.status = ServerStatus.STOPPED;

                    // check the polymorph typ of the current object
                    switch (this.GetType().Name)
                    {
                            
                        // if the type of the current object is "TCPServer_ByteBased"
                        case "TCPServer_ByteBased":

                            // Do for each socket in the list of clients
                            foreach (Socket socket in this.clients)
                            {
                                // if the socket is established
                                if (socket.Connected)
                                    socket.Disconnect(false);   // Disconnect from the server

                                socket.Shutdown(SocketShutdown.Both);   // Stops recieving and sending packets
                                socket.Close();                         // close the socket
                                this.clients.Remove(socket);            // remove this client from the list of clients
                                
                            }

                            break;


                        // if the type of the current object is "TCPServer_Streambased"
                        case "TCPServer_Streambased":

                            // Do for each TcpClient in the list of clients
                            foreach (TcpClient tcpclient in this.clients)
                            {
                                
                                // if the socket is established
                                if (tcpclient.Connected)    
                                    tcpclient.Close();          // close the tcp connection

                                this.clients.Remove(tcpclient); // remove this clients form the list of clients

                            }

                            break;
                    }

                    // Do for each thread in clientThreads
                    foreach (Thread clientThread in this.clientThreads)
                    {
                        // if the clientThread is running
                        if (clientThread.IsAlive)
                            clientThread.Abort();   // abort the clientThread
                    }

                    // if the listenThread is running
                    if (this.listenThread.IsAlive)
                    {
                        this.listenThread.Abort();  // abort the listenThread
                    }

                    this.tcpListener.Stop();    // stop the tcpListener

                }

                // catch any exception
                catch (Exception ex)
                {
                    Console.WriteLine("Error by closing TCPServer \n " + ex.Message);
                }
             
            }

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
        public Boolean CanStart()
        {
            try
            {
                // if the ipAddress and the port isn't configured
                if (this.ipAddress == null || this.port == 0)
                    return false;   // return that the server isn`t able to start

                // else
                this.tcpListener = new TcpListener(this.ipAddress, this.port);  // initialize a new TcpListener
                this.tcpListener.Start();                                       // start the tcpListener
                this.tcpListener.Stop();                                        // stop the tcpListener

                // TCPListener has successfully started and stoped
                return true;    // return that the is able to start
            }
            
            // catch any SocketException
            // if an SocketException appears, the tcpListener wasn't able to start and stop successfully
            catch (SocketException)
            {
                // if the tcpListener isn't initialized
                if( this.tcpListener != null )
                    this.tcpListener.Stop();    // stop the tcpListener

                return false;       // return that the server isn't able to start
            }
        }


        ///<summary>
        /// starts an endless loop which accepts new clients
        ///</summary>
        protected virtual void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();               // start the tcpListener
                this.status = ServerStatus.RUNNING;     // set the ServerStatus to RUNNING
            }

            // catch any SocketException
            catch (SocketException)
            {
                this.Stop();                                                                // stop the whole TCPServer
                throw new Exception("Starting server on port " + this.Port + " failed");    // throw that Exception
            }

        }

        #endregion

    }
}
