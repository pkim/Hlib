﻿/*
 * Name: TCPClient
 * Date: 20 Februar 2011
 * Author: Patrik Kimmeswenger
 * Description: initializes a TCPClient
*/

using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace HLib.Network.IP.TCP
{
    public class TCPClient
    {

        #region objects

        /* Status */
        public enum ClientStatus 
        { transfering , connected, stopped };


        /* Client to Server information */
        private TcpClient  client;
        private IPEndPoint serverEndPoint;
        private NetworkStream clientStream;

        private IPAddress server_IPAdresse;
        private Int16     server_Port;


        /* Encoder */
        private ASCIIEncoding encoder;

        /* command transmitter mod */
        private enum Transmitter_Mod
        { send_and_recieve, send_Only, none }

        #endregion


        #region variables

        private ClientStatus status;

        #endregion


        #region proberties

        public Int32 StreamReadTimeout { get; set; }

        public Int32 ReceiveTimeout    { get; set; }

        public ClientStatus Status
        {
            get
            {
                return this.status;
            }
        }

        #endregion


        #region Constructor
        /* Constructor default*/

        public TCPClient()
        {
            this.status = new ClientStatus();
            this.status = ClientStatus.stopped;

            encoder = new ASCIIEncoding();

            // Timeouts
            this.StreamReadTimeout = 4000;
            this.ReceiveTimeout    = 4000;
        }


        /* Constructor with instant initialisation of ServerEndPoint */
        public TCPClient(String _server_IPv4Address, Int16 _server_Port)
        {
            this.status = ClientStatus.stopped;

            encoder = new ASCIIEncoding();
            this.server_IPAdresse = IPAddress.Parse(_server_IPv4Address);
            this.server_Port      = _server_Port;

        }

        #endregion


        #region methods

        ///<summary>
        /// checks if the client is connected
        /// to the server
        ///</summary>
        /// <returns>
        /// true if the server is connected otherwise false
        /// </returns>
        public Boolean isConnected()
        {
            if (this.client != null && this.client.Connected)
                return true;

            return false;
        }


        ///<summary>
        /// Connect the client to a server 
        ///</summary>
        public void connectToServer()
        {

            this.client = new TcpClient();

            if (this.server_IPAdresse != null && this.server_Port != 0)
            {
                this.serverEndPoint = new IPEndPoint(this.server_IPAdresse, this.server_Port);

                try
                {
                    this.status = ClientStatus.connected;
                    this.client.ReceiveTimeout = this.ReceiveTimeout;
                    this.client.Connect(serverEndPoint);
                    this.clientStream = client.GetStream();
                    this.clientStream.ReadTimeout = this.StreamReadTimeout;     
                }
                catch (SocketException _socketException)
                {
                    this.status = ClientStatus.stopped;
                    throw new Exception("Connecting to server FAILED!", _socketException.InnerException);
                }

            }
        }

        ///<summary>
        /// Connect the client to a server 
        /// 
        /// <returns>
        /// <value>true</value> if connecting to server is successfull
        /// otherwise <value>false</value>
        /// </returns>
        /// 
        /// <param name="server_IPv4Address">server IPv4 address</param>
        /// <param name="server_Port">server port</param>
        ///</summary>
        public Boolean connectToServer(String server_IPv4Address, Int16 server_Port)
        {
 
            this.client = new TcpClient();
            this.serverEndPoint = new IPEndPoint(IPAddress.Parse(server_IPv4Address), server_Port);

            try
            {
                this.client.ReceiveTimeout = this.ReceiveTimeout;
                this.client.Connect(serverEndPoint);
                this.clientStream = client.GetStream();
                this.clientStream.ReadTimeout = this.StreamReadTimeout;
                this.status = ClientStatus.connected;

                return true;
            }

            catch (SocketException _socketException )
            {
                throw _socketException;
            }
            
        }


        ///<summary>
        /// stops the coummunication
        ///</summary>
        public void stop()
        {
            if (this.client != null && !this.client.Connected)
            {
                this.client.Close();
                this.status = ClientStatus.stopped;
            }

            else if (this.client != null && this.client.Connected)
            {
                this.clientStream.Close();
                this.client.Close();
                this.status = ClientStatus.stopped;
            }
        }

        ///<summary>
        /// sends a message
        ///</summary>
        public void send(object _message)
        {
             
            try
            {
                byte[] buffer = encoder.GetBytes((String) _message);

                this.status = ClientStatus.transfering;
                if (this.clientStream != null)
                {
                    clientStream.Write(buffer, 0, buffer.Length);
                    clientStream.Flush();
                }  
            }
            catch (IOException _ioException)
            {
                throw new IOException("IOEeception: cant´t send message", _ioException.InnerException);
            }

            this.status = ClientStatus.connected;

        }

        #endregion

    }
}
