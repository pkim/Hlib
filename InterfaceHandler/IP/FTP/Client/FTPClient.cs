/*
 * Filename: FTP.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * 
 * Description: 
 * 
 * The objectives of FTP are 
 * 
 * 1) to promote sharing of files (computerprograms and/or data), 
 * 
 * 2) to encourage indirect or implicit (viaprograms) use of remote computers, 
 * 
 * 3) to shield a user from variations in file storage systems among hosts, and 
 * 
 * 4) to transfer data reliably and efficiently.  FTP, though usable directly by a user
 *    at a terminal, is designed mainly for use by programs.
 *    
 * The attempt in this specification is to satisfy the diverse needs of
 * users of maxi-hosts, mini-hosts, personal workstations, and TACs,
 * with a simple, and easily implemented protocol design.
 * 
 * This paper assumes knowledge of the Transmission Control Protocol
 * (TCP) and the Telnet Protocol.  These documents are contained
 * in the ARPA-Internet protocol handbook.
 * 
 * see: http://tools.ietf.org/html/rfc959
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Threading;


namespace Handler.Interface.HLib.Network.IP.FTP
{

    /// <summary>
    /// The objectives of FTP are 
    /// 
    /// 1) to promote sharing of files (computer
    /// programs and/or data), 
    /// 
    /// 2) to encourage indirect or implicit (via
    /// programs) use of remote computers, 
    /// 
    /// 3) to shield a user from
    /// variations in file storage systems among hosts, and 
    /// 
    /// 4) to transfer
    /// data reliably and efficiently.  FTP, though usable directly by a user
    /// at a terminal, is designed mainly for use by programs.
    ///    
    /// The attempt in this specification is to satisfy the diverse needs of
    /// users of maxi-hosts, mini-hosts, personal workstations, and TACs,
    /// with a simple, and easily implemented protocol design.
    /// 
    /// see: http://tools.ietf.org/html/rfc959
    /// </summary>
    public class FTPClient : IFTPClient
    {

        #region Objects

        private Boolean isConnected = false;
        private Boolean isTransfering = true;

        private IPEndPoint controlIPEndPoint;
        private IPEndPoint dataIPEndPoint;

        private TcpClient tcpClient;

        private IPHostEntry server;

        #endregion Objects


        #region Properties

        /// <summary>
        /// Is true if the client is connected successfully
        /// </summary>
        public Boolean IsConnected
        { get { return this.isConnected; } }

        /// <summary>
        /// IS true if the client is transfering data at the moment
        /// </summary>
        public Boolean IsTransfering
        { get { return this.isTransfering; } }

        /// <summary>
        /// Port number the FTP server is listening on
        /// </summary>
        public Int16 Port { get; set; }

        /// <summary>
        /// IP address or hostname to connect to
        /// </summary>
        public String Server
        {
            get
            {
                return this.server.HostName;
            }

            set
            {
                try
                {
                    this.server = Dns.GetHostEntry(value);
                }
                catch (Exception)
                {
                    throw new Exception(String.Format("Unable to reach host \"{0}\"", value));
                }
            }
        }


        /// <summary>
        /// Username to login as
        /// </summary>
        public String User { get; set; }

        /// <summary>
        /// Password for account
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// The size of the message blocks.
        /// Default is 512
        /// </summary>
        public Int32 BlockSize { get; set; }

        /// <summary>
        /// The timeout (milliseconds) for waiting for connection.
        /// The default is 300.
        /// </summary>
        public Int32 ConnectTimeout { get; set; }

        /// <summary>
        /// The timeout (milliseconds) for waiting for data recieve.
        /// The default is 420.
        /// </summary>
        public Int32 TransferTimeout { get; set; }

        /// <summary>
        /// The timeout (milliseconds) for waiting for data recieve.
        /// The default is 10000
        /// </summary>
        public Int32 RecieveTimeout { get; set; }

        /// <summary>
        /// The addressfamily of the communication between client and the server.
        /// Default is "InterNetwork" which means IPv4.
        /// </summary>
        public AddressFamily AddressFamily { get; set; }

        /// <summary>
        /// The Encoding of the communication
        /// Default is ASCII
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// The mode in which the FTPClient should connect to the ftp server.
        /// Default is "passive" mode
        /// </summary>
        public FTP.ConnectionMode ConnectionMode { get; set; }

        /// <summary>
        /// The mode in which data will be transfered
        /// </summary>
        public FTP.TransferMode TransferMode { get; set; }

        /// <summary>
        /// The type of the transferMode which handles the transfer of data
        /// </summary>
        public FTP.TransferModeType TransferModeType { get; set; }

        #endregion Properties


        #region Constructor


        /// <summary>
        /// Initializes a new ftp Client 
        /// </summary>
        public FTPClient()
        {
            this.Server = String.Empty;
            this.Port = FTP.DEFAULT_CONTROL_PORT;

            this.User = String.Empty;
            this.Password = String.Empty;

            this.ConnectTimeout = PropertyManager.Property_FTP.InactiveTimeout;
            this.TransferTimeout = PropertyManager.Property_FTP.TransferTimeout;
            this.RecieveTimeout = PropertyManager.Property_FTP.RecieveTimeout;

            this.ConnectionMode = PropertyManager.Property_FTP.Connection_Mode;
            this.TransferMode = PropertyManager.Property_FTP.Transfer_Mode;
            this.TransferModeType = FTP.TransferModeType.ASCII;

            this.AddressFamily = AddressFamily.InterNetwork;

            this.Encoding = Encoding.GetEncoding(PropertyManager.Property_FTP.EncodingCodePage);

            this.tcpClient = new TcpClient();

            this.BlockSize = PropertyManager.Property_FTP.BlockSize;

            this.controlIPEndPoint = null;
            this.dataIPEndPoint = null;

            this.isConnected = false;
            this.isTransfering = false;

        }

        /// <summary>
        /// Initializes a new ftp Client 
        /// </summary>
        /// <param name="_server">The hostname dnsname or ipaddress of the server</param>
        public FTPClient(String _server)
            : this()
        {
            this.Server = _server;
        }

        /// <summary>
        /// Initializes a new ftp Client 
        /// </summary>
        /// <param name="_server">The hostname dns-name or ip-address of the server</param>
        /// <param name="_port">The port of FTP server. Default is 21</param>
        public FTPClient(String _server, Int16 _port)
            : this(_server)
        {
            this.Port = _port;
        }

        /// <summary>
        /// Initializes a new ftp Client 
        /// </summary>
        /// <param name="_server">The hostname dns-name or ip-address of the server</param>
        /// <param name="_port">The port of FTP server. Default is 21</param>
        /// <param name="_user">The user, which have the permission to connect to the server</param>
        public FTPClient(String _server, Int16 _port, String _user)
            : this(_server, _port)
        {
            this.User = _user;
        }

        /// <summary>
        /// Initializes a new ftp Client 
        /// </summary>
        /// <param name="_server">The hostname dns-name or ip-address of the server</param>
        /// <param name="_port">The port of FTP server. Default is 21</param>
        /// <param name="_user">The user, which have the permission to connect to the server</param>
        /// <param name="_password">The password regarding to the user, which to parameters are need for the connection</param>
        public FTPClient(String _server, Int16 _port, String _user, String _password)
            : this(_server, _port, _user)
        {
            this.Password = _password;
        }

        /// <summary>
        /// Initializes a new ftp Client 
        /// </summary>
        /// <param name="_server">The hostname dns-name or ip-address of the server</param>
        /// <param name="_user">The user, which have the permission to connect to the server</param>
        /// <param name="_password">The password regarding to the user, which to parameters are need for the connection</param>
        public FTPClient(String _server, String _user, String _password)
            : this(_server)
        {
            this.User = _user;
            this.Password = _password;
        }

        #endregion Constructor


        #region Events

        #endregion Events


        #region Methods


        #region Private Methods

        /// <summary>
        /// Select the first IPv4Address in the given ipaddresses
        /// </summary>
        /// <param name="_addresses">The ipaddresses from which the method will select the first IPv4Address</param>
        /// <returns></returns>
        private IPAddress getIPv4Address(IPAddress[] _addresses)
        {
            Int32 i = new Int32();

            // while no IPv4Address is found
            while (i != _addresses.Count() && _addresses[i].AddressFamily != AddressFamily.InterNetwork)
            {
                i++;
            }

            // if no IPv4Address has been foudn
            if (i == _addresses.Count())
                return null;

            // return the selected IPv4Address
            return _addresses[i];

        }

        /// <summary>
        /// Send a ftp command to the connected server
        /// </summary>
        /// <param name="_ftpCommand">The ftp command</param>
        /// <param name="_parameters">The parameters of the command</param>
        /// <returns>A list of FTPResponseMessages</returns>
        private List<FTPResponseMessage> sendCommand(String _ftpCommand, params String[] _parameters)
        {
            NetworkStream networkStream;

            // connect to the FTPServer, if isn't already connected
            this.Connect();

            // Initialization
            try
            {
                networkStream = this.tcpClient.GetStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }

            Byte[] ftpcommand;


            // if the _parameters aren't null or more than 0 parameters are needed
            if (_parameters != null || _parameters.Count() != 0)
            {
                // append a whitespace
                _ftpCommand += " ";

                // for each parameter
                foreach (String parameter in _parameters)
                {
                    // append the parameter + ',' to the fptcommand
                    _ftpCommand = String.Format("{0}{1},", _ftpCommand, parameter);
                }

                // Remove the last ','
                _ftpCommand = _ftpCommand.Remove(_ftpCommand.Length - 1);
            }

            // the ftp command as byte-array
            ftpcommand = this.Encoding.GetBytes((_ftpCommand + "\r\n").ToCharArray());

            try
            {
                // if the tcpClient is connected
                if (this.tcpClient.Connected)
                {
                    // send the command
                    networkStream.Write(ftpcommand, 0, ftpcommand.Length);
                }

                else
                    throw new Exception("Server has interrupted the connection");
            }
            catch (Exception)
            {
                throw new Exception("Server antwortet nicht");
            }

            // return the response message
            return this.readResponse();
        }

        /// <summary>
        /// Send a ftp command
        /// </summary>
        /// <param name="_ftpCommand">The ftp command</param>
        /// <param name="_encoding"></param>
        /// <param name="_parameters">The parameters of the command</param>
        /// <returns>A list of FTPResponseMessages</returns>
        private List<FTPResponseMessage> sendCommand(String _ftpCommand, Encoding _encoding, params String[] _parameters)
        {
            // set the encoding 
            this.Encoding = _encoding;

            // return the response messages
            return this.sendCommand(_ftpCommand, _parameters);
        }

        /// <summary>
        /// Reads the response messages of the server
        /// </summary>
        /// <returns>A list of FTPResponseMessages</returns>
        private List<FTPResponseMessage> readResponse()
        {
            // Initialization
            NetworkStream netowrkStream = this.tcpClient.GetStream();

            List<FTPResponseMessage> ftpResponseMessages = new List<FTPResponseMessage>();

            Int32 tryCount = new Int32();


            // Read the messages of the server
            List<FTPResponseMessage> tempMessage = this.readLines(netowrkStream);


            // wait as long a message recieved or the timeout is reached
            while (tempMessage.Count == 0)
            {
                if (tryCount == this.RecieveTimeout / 10)
                {
                    throw new Exception("Server does not return a response");
                }

                // Sleep 10 seconds
                Thread.Sleep(10);

                // add 1 to the try Counter
                tryCount++;

                // try to read lines if the server has send a message
                tempMessage = readLines(netowrkStream);
            }

            // add this recieved message to the list of ftp messages
            ftpResponseMessages.AddRange(tempMessage);

            // while a new message is available
            while ((tempMessage[tempMessage.Count - 1]).Message.Substring(0, 1) == "-")
            {
                // read this message
                tempMessage = readLines(netowrkStream);

                // add thi recieved message to the list of ftpmessages
                ftpResponseMessages.AddRange(tempMessage);
            }

            // return the list of fptmessages
            return ftpResponseMessages;
        }

        /// <summary>
        /// Trys to read the next available message
        /// </summary>
        /// <param name="stream">The stream where the messages is recieving</param>
        /// <returns>Returns the list of FTPResponseMessages</returns>
        private List<FTPResponseMessage> readLines(NetworkStream stream)
        {
            // Initialization
            List<FTPResponseMessage> responseMessages = new List<FTPResponseMessage>();

            char[] seperator = { '\n' };
            char[] toRemove = { '\r' };

            Byte[] buffer = new Byte[this.BlockSize];
            Int32 bytes = new Int32();
            String tempMessage = "";
            String[] messages;


            // while new messages are available on the stream
            while (stream.DataAvailable)
            {
                // read the message
                bytes = stream.Read(buffer, 0, buffer.Length);

                // encodes the recieved message
                tempMessage += this.Encoding.GetString(buffer, 0, bytes);
            }

            // split the message by the speerator
            messages = tempMessage.Split(seperator);

            // for each message in the splitted array of messages
            foreach (String message in messages)
            {
                // if the message is not empty
                if (messages.Length > 0)
                {
                    // Initialize a new ftpResponseMessage
                    FTPResponseMessage responseMessage = new FTPResponseMessage();

                    // remove the heading and trailing whitespaces
                    String ftpMessage = message.Trim(toRemove);

                    // initialize a default responseCode
                    Int32 responseCode = (Int32)FTPServerResponseCode.None;


                    // if the recieved ftpMessage is null or empty
                    if (String.IsNullOrEmpty(ftpMessage))
                    {
                        // parse the next message
                        continue;
                    }

                    // if the message includes a responseCode
                    if (ftpMessage.Length > 2 && Int32.TryParse(ftpMessage.Substring(0, FTP.RESPONSE_CODE_LENGTH), out responseCode))
                    {
                        // delete this response from the full message
                        ftpMessage = message.Substring(FTP.RESPONSE_CODE_LENGTH);
                    }


                    responseMessage.Message = ftpMessage;                              // set the ftpMessage 
                    responseMessage.ResponseCode = (FTPServerResponseCode)responseCode;     // set the responseCode

                    // add this ftpmessage to the list of the ftpMessages
                    responseMessages.Add(responseMessage);
                }
            }

            // return the list of ftpMessages
            return responseMessages;
        }

        /// <summary>
        /// Lock the TcpClient
        /// </summary>
        private void lockTcpClient()
        {
            System.Threading.Monitor.Enter(this.tcpClient);
        }

        /// <summary>
        /// Unlock the TcpClient
        /// </summary>
        private void unlockTcpClient()
        {
            System.Threading.Monitor.Exit(this.tcpClient);
        }

        /// <summary>
        /// Set the type of transfer
        /// </summary>
        /// <param name="_type">The transfer type</param>
        /// <returns>Returns a list of FTPResponseMessages</returns>
        private List<FTPResponseMessage> setTransferType(FTP.TransferModeType _type)
        {
            // Initializatoin
            ArrayList tempMessageList = new ArrayList();
            List<FTPResponseMessage> responseMessages = new List<FTPResponseMessage>();

            // lock the tcpClient
            this.lockTcpClient();

            switch (_type)
            {
                case FTP.TransferModeType.ASCII:

                    // set transferType to ASCII
                    responseMessages = this.sendCommand(FTPCommand.REPRESENTATION_TYPE, FTP.TransferModeTypeASCII);
                    break;


                case FTP.TransferModeType.BINARY:

                    // set transferType fo BINARY
                    responseMessages = this.sendCommand(FTPCommand.REPRESENTATION_TYPE, FTP.TransferModeTypeBinary);
                    break;


                default:

                    // unlock the tcpClient
                    this.unlockTcpClient();

                    // return a empty list
                    return responseMessages;

            }

            if (responseMessages == null)
                return null;

            if (responseMessages.Count == 0)
            {
                // unlock the tcpClient
                this.unlockTcpClient();

                // return a empty list
                return responseMessages;
            }


            if (responseMessages[0].ResponseCode != FTPServerResponseCode.CommandOK)
            {
                throw new Exception((String)tempMessageList[0]);
            }

            // unlock the tcpClient
            unlockTcpClient();

            // return the list of responseMessages
            return responseMessages;
        }

        /// <summary>
        /// Create a new TCPLister which represents a dataLister for the data ftp connection
        /// </summary>
        /// <returns>Returns the created TcpListener</returns>
        private TcpListener createDataListner()
        {
            // generate a new port data transfer connection
            Int32 port = this.generateDataPort();
            try
            {
                // set the port for active data transfer
                this.setDataPort(port);
            }
            catch (Exception)
            {
                throw new Exception("Server doesn't support Active mode");
            }

            // Select a IPv4Address from all available addresses, because IPv6 isn't supported yet
            IPAddress hostAddress = this.getIPv4Address(Dns.GetHostEntry(Dns.GetHostName()).AddressList);


            if (hostAddress != null)
            {
                // generate the new TcpListener based on the IPv4Address and the generated data port
                TcpListener listner = new TcpListener(hostAddress, port);

                return listner;
            }

            return null;
        }

        /// <summary>
        /// Creates a new TcpClient for passive transfer mode
        /// </summary>
        /// <returns></returns>
        private TcpClient createDataClient()
        {
            // generate a new port data transfer connection
            Int32 port = generateDataPort();

            TcpClient client = new TcpClient();


            try
            {
                // connect to the data socket of the ftp server
                client.Connect(this.Server, port);
            }
            catch (Exception)
            {
                throw new Exception("Unable to open data connection");
            }

            return client;
        }

        /// <summary>
        /// Set the port for active transfer mode
        /// </summary>
        /// <param name="portNumber"></param>
        private void setDataPort(Int32 portNumber)
        {
            // lock the object
            lockTcpClient();

            // saves the repsonse messages from the server
            List<FTPResponseMessage> responseMessages;

            // generate port range
            Int32 portHigh = portNumber >> 8;
            Int32 portLow = portNumber & 255;

            // select a IPv4Address from the pool
            IPAddress hostAddress = this.getIPv4Address(Dns.GetHostEntry(Dns.GetHostName()).AddressList);

            // replace the dots of the ip with ',' => e.g 192,168,0,1
            String hostname = hostAddress.ToString().Replace(".", ",");

            // send the ftp command to set de port or data transfer
            responseMessages = this.sendCommand(FTPCommand.DATA_PORT
                                          , hostname
                                          , portHigh.ToString()
                                          , portLow.ToString()
                                          );


            // if the responseMessage of the server isn't OK
            if (responseMessages[0].ResponseCode != FTPServerResponseCode.CommandOK)
            {
                throw new Exception(responseMessages[0].Message);
            }

            // unlock the tcp Client object
            unlockTcpClient();
        }


        private Int32 generateDataPort()
        {
            lockTcpClient();

            Int32 port = new Int32();

            switch (this.ConnectionMode)
            {
                case FTP.ConnectionMode.ACTIVE:

                    Random rnd = new Random((Int32)DateTime.Now.Ticks);
                    port = FTP.DATA_PORT_RANGE_FROM + rnd.Next(FTP.DATA_PORT_RANGE_TO - FTP.DATA_PORT_RANGE_FROM);
                    break;


                case FTP.ConnectionMode.PASSIVE:

                    List<FTPResponseMessage> responseMessages;

                    responseMessages = this.sendCommand(FTPCommand.PASSIVE);

                    if (responseMessages[0].ResponseCode != FTPServerResponseCode.EnteringPassiveMode)
                    {
                        throw new Exception((String)responseMessages[0].Message + " Passive Mode not implemented");
                    }

                    String message = responseMessages[0].Message;

                    Int32 index1 = message.IndexOf(",", 0);
                    Int32 index2 = message.IndexOf(",", index1 + 1);
                    Int32 index3 = message.IndexOf(",", index2 + 1);
                    Int32 index4 = message.IndexOf(",", index3 + 1);
                    Int32 index5 = message.IndexOf(",", index4 + 1);
                    Int32 index6 = message.IndexOf(")", index5 + 1);

                    port = 256 * Int32.Parse(message.Substring(index4 + 1, index5 - index4 - 1)) + Int32.Parse(message.Substring(index5 + 1, index6 - index5 - 1));

                    break;
            }

            unlockTcpClient();

            return port;
        }


        #endregion Private Methods


        #region Public Methods


        public Boolean Disconnect()
        {
            List<FTPResponseMessage> responseMessages = new List<FTPResponseMessage>();

            if (this.tcpClient != null && this.tcpClient.Connected)
            {
                responseMessages = this.sendCommand(FTPCommand.LOGOUT);
                this.tcpClient.Close();

                this.isConnected = false;
            }

            return true;
        }

        public Boolean Connect(String _server, Int16 _port, String _user, String _password)
        {
            this.Password = _password;

            return this.Connect(_server, _port, _password);
        }

        public Boolean Connect(String _server, Int32 _port, String _user, String _password)
        {
            this.Password = _password;

            Int16 port = Convert.ToInt16(_port);

            return this.Connect(_server, port, _password);
        }

        public Boolean Connect(String _server, Int16 _port, String _user)
        {
            this.User = _user;

            return this.Connect(_server, _port);
        }

        public Boolean Connect(String _server, Int16 _port)
        {
            this.Port = _port;

            return this.Connect(_server);
        }

        public Boolean Connect(String _server, String _user, String _password)
        {
            this.Password = _password;

            return this.Connect(_server, _user);
        }

        public Boolean Connect(String _server, String _user)
        {
            this.User = _user;

            return this.Connect(_server);
        }

        public Boolean Connect(String _server)
        {
            this.Server = _server;

            return this.Connect();
        }

        /// <summary>
        /// This method will try to connect the client to a server. The objects 
        /// Server, Port (Default port), 
        /// </summary>
        /// <returns>If the connection has been successfully established this method will return true otherwise false</returns>
        public Boolean Connect()
        {
            if (this.tcpClient.Connected)
                return true;

            // if no host is configured
            if (String.IsNullOrEmpty(this.Server))
                throw new Exception("No Server has been set.");

            // if no user is configured
            if (String.IsNullOrEmpty(this.User))
                throw new Exception("No user has been set.");

            if (this.tcpClient == null)
            {
                this.tcpClient = new TcpClient();
            }

            IPAddress hostAddress = this.getIPv4Address(this.server.AddressList);

            if (hostAddress == null)
                throw new Exception("Invalid hostaddress");

            this.controlIPEndPoint = new IPEndPoint(hostAddress, this.Port);

            try
            {
                this.tcpClient.Connect(this.controlIPEndPoint);
            }
            catch (ObjectDisposedException)
            {
                this.tcpClient = new TcpClient();
                return this.Connect();
            }

            catch
            {
                throw new Exception("Unable to connect to server!");
            }

            List<FTPResponseMessage> responseMessages = this.readResponse();

            if (responseMessages[0].ResponseCode != FTPServerResponseCode.ServiceIsReadyForNewUser)
            {
                this.Disconnect();
                throw new Exception(responseMessages[0].Message);
            }

            responseMessages = this.sendCommand(FTPCommand.USER_NAME, this.User);

            switch (responseMessages[0].ResponseCode)
            {

                case FTPServerResponseCode.UserNameOKNeedPassword:

                    if (String.IsNullOrEmpty(this.Password))
                    {
                        this.Disconnect();
                        throw new Exception(responseMessages[0].Message);
                    }

                    responseMessages = this.sendCommand(FTPCommand.PASSWORD, this.Password);

                    if (responseMessages[0].ResponseCode != FTPServerResponseCode.UserLoggedIn)
                    {
                        this.Disconnect();
                        throw new Exception(responseMessages[0].Message);
                    }

                    break;


                case FTPServerResponseCode.UserLoggedIn:
                    break;
            }

            this.isConnected = true;
            return true;
        }


        public List<FTPItem> GetItemsExtended()
        {
            lockTcpClient();

            TcpListener listner = null;
            TcpClient client = null;
            NetworkStream networkStream = null;

            List<FTPItem> items = new List<FTPItem>();
            List<FTPResponseMessage> tmpFilesAndFolderList = new List<FTPResponseMessage>();

            List<FTPResponseMessage> responseMessages;


            setTransferType(FTP.TransferModeType.ASCII);

            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                listner = createDataListner();
                listner.Start();
            }
            else
            {
                client = createDataClient();
            }


            responseMessages = this.sendCommand(FTPCommand.LIST);

            if (!(responseMessages[0].ResponseCode == FTPServerResponseCode.OpenedNewConnection || responseMessages[0].ResponseCode == FTPServerResponseCode.DataConnectionAlreadyOpen))
            {
                throw new Exception(responseMessages[0].Message);
            }

            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                client = listner.AcceptTcpClient();
            }

            networkStream = client.GetStream();

            tmpFilesAndFolderList = readLines(networkStream);

            String errorMessage = String.Empty;

            if (responseMessages.Count == 1)
            {
                responseMessages = this.readResponse();
                errorMessage = responseMessages[0].Message;
            }

            else
            {
                errorMessage = responseMessages[1].Message;
            }

            if (!(responseMessages[0].ResponseCode == FTPServerResponseCode.DataConnectionClosing || responseMessages[1].ResponseCode == FTPServerResponseCode.DataConnectionClosing))
            {
                throw new Exception(errorMessage);
            }

            try
            {
                networkStream.Close();
                client.Close();
            }
            catch (Exception)
            {
                throw new Exception("Network Error");
            }


            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                listner.Stop();
            }

            unlockTcpClient();

            foreach (FTPResponseMessage item in tmpFilesAndFolderList)
            {
                items.Add(FTPItem.GetFTPItem(item.Message, (Char)32));
            }

            return items;
        }

        public List<FTPItem> GetItems()
        {
            lockTcpClient();

            TcpListener listner = null;
            TcpClient client = null;

            NetworkStream networkStream = null;

            List<FTPResponseMessage> responseMessages = new List<FTPResponseMessage>();


            List<FTPItem> ftpItems = new List<FTPItem>();

            setTransferType(FTP.TransferModeType.ASCII);

            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                listner = createDataListner();
                listner.Start();
            }
            else
            {
                client = createDataClient();
            }


            responseMessages = this.sendCommand(FTPCommand.NAME_LIST);


            switch (responseMessages[0].ResponseCode)
            {
                case FTPServerResponseCode.OpenedNewConnection:
                case FTPServerResponseCode.DataConnectionAlreadyOpen:

                    break;


                case FTPServerResponseCode.FileNotFound:

                    return ftpItems;


                default:

                    throw new Exception(responseMessages[0].Message);
            }


            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                client = listner.AcceptTcpClient();
            }

            networkStream = client.GetStream();

            responseMessages = readLines(networkStream);

            String errorMessage = String.Empty;

            if (responseMessages.Count == 1)
            {
                responseMessages = this.readResponse();
                errorMessage = responseMessages[0].Message;
            }

            else
            {
                errorMessage = responseMessages[1].Message;
            }

            if (!(responseMessages[1].ResponseCode == FTPServerResponseCode.DataConnectionClosing))
            {
                throw new Exception(errorMessage);
            }

            networkStream.Close();
            client.Close();

            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                listner.Stop();
            }

            unlockTcpClient();

            return ftpItems;
        }

        public List<FTPItem> GetFiles()
        {
            List<FTPItem> files = new List<FTPItem>();
            List<FTPItem> items = this.GetItemsExtended();

            foreach (FTPItem item in items)
            {
                if (!item.IsDirectory)
                    files.Add(item);
            }

            return files;
        }

        public List<FTPItem> GetDirectories()
        {
            List<FTPItem> directories = new List<FTPItem>();
            List<FTPItem> items = this.GetItemsExtended();


            foreach (FTPItem item in items)
            {
                if (item.IsDirectory)
                    directories.Add(item);
            }

            return directories;
        }

        public String GetFileDate(String _fileName)
        {
            this.lockTcpClient();

            this.Connect();

            List<FTPResponseMessage> responseMessages = this.sendCommand(FTPCommand.GetLastModifiedTimeOfFile, _fileName);

            if (responseMessages[0].ResponseCode != FTPServerResponseCode.FileStatus)
            {
                throw new Exception(responseMessages[0].Message);
            }

            this.unlockTcpClient();

            return responseMessages[0].Message;
        }

        public String GetWorkingDirectory()
        {

            String workingDirectory = String.Empty;


            this.lockTcpClient();

            this.Connect();

            List<FTPResponseMessage> responseMessages = this.sendCommand(FTPCommand.PRINT_WORKING_DIRECTORY);

            if (responseMessages[0].ResponseCode != FTPServerResponseCode.DirectoryCreated)
                throw new Exception(responseMessages[0].Message);

            try
            {
                workingDirectory = responseMessages[0].Message.Substring(0, responseMessages[0].Message.LastIndexOf("\"") + 1);
            }
            catch (Exception _ex)
            {
                throw new Exception("Unable to resolve working directory: " + _ex);
            }

            this.unlockTcpClient();

            return workingDirectory;
        }

        public Boolean ChangeWorkingDirectory(String _path)
        {
            this.lockTcpClient();

            this.Connect();

            List<FTPResponseMessage> responseMessages = this.sendCommand(FTPCommand.CHANGE_WORKING_DIRECTORY, _path);

            if (responseMessages == null)
                return false;

            if (responseMessages[0].ResponseCode != FTPServerResponseCode.RequestedFileActionCompleted)
                //throw new Exception(responseMessages[0].Message);
                return false;

            this.unlockTcpClient();

            return true;
        }

        public Boolean MakeDirectory(String _directory)
        {
            this.lockTcpClient();

            this.Connect();

            List<FTPResponseMessage> responseMessage = this.sendCommand(FTPCommand.MAKE_DIRECTORY, _directory);

            switch (responseMessage[0].ResponseCode)
            {
                case FTPServerResponseCode.DirectoryCreated:
                case FTPServerResponseCode.RequestedFileActionCompleted:

                    this.unlockTcpClient();
                    return true;


                case FTPServerResponseCode.FileNotFound:

                    this.unlockTcpClient();
                    return false;


                default:
                    throw new Exception(responseMessage[0].Message);
            }
        }

        public Boolean RemoveDirectory(String _directory)
        {
            this.lockTcpClient();

            this.Connect();

            List<FTPResponseMessage> responseMessage = this.sendCommand(FTPCommand.REMOVE_DIRECTORY, _directory);

            // if the server couldnt delete the directory, because the directory is not empty or something went wrong. (Permissions)
            if (!(responseMessage[0].ResponseCode == FTPServerResponseCode.FileNotFound || responseMessage[0].ResponseCode == FTPServerResponseCode.RequestedFileActionCompleted))
            {
                throw new Exception(responseMessage[0].Message);
            }

            // if the directory contains subitems delete the subItems recursively
            if (responseMessage[0].ResponseCode == FTPServerResponseCode.FileNotFound)
            {
                try
                {
                    this.ChangeWorkingDirectory(_directory);

                    List<FTPItem> subItems = this.GetItemsExtended();
                    if (subItems.Count != 0)
                    {
                        foreach (FTPItem ftpItem in subItems)
                        {
                            if (ftpItem.IsDirectory)
                                this.RemoveDirectory(ftpItem.Name);

                            else
                                this.RemoveFile(ftpItem.Name);
                        }
                    }

                    this.ChangeWorkingDirectory("..");
                    this.RemoveDirectory(_directory);
                }
                catch (Exception exception)
                {
                    throw exception;
                }

            }

            this.unlockTcpClient();

            return true;
        }

        public Boolean RemoveFile(String _file)
        {
            this.lockTcpClient();

            this.Connect();

            List<FTPResponseMessage> responseMessage = this.sendCommand(FTPCommand.DELETE, _file);

            if (responseMessage[0].ResponseCode != FTPServerResponseCode.RequestedFileActionCompleted)
            {
                throw new Exception(responseMessage[0].Message);
            }

            this.unlockTcpClient();

            return true;
        }

        public Boolean RenameItem(String _file, String _newFileName)
        {
            List<FTPResponseMessage> responseMessage;

            this.lockTcpClient();

            this.Connect();

            responseMessage = this.sendCommand(FTPCommand.RENAME_FROM, _file);

            if (responseMessage[0].ResponseCode != FTPServerResponseCode.RequestedFileActionNeedFurtherInformation)
            {
                throw new Exception(responseMessage[0].Message);
            }

            else
            {
                responseMessage = this.sendCommand(FTPCommand.RENAME_TO, _newFileName);

                if (responseMessage[0].ResponseCode != FTPServerResponseCode.RequestedFileActionCompleted)
                {
                    throw new Exception(responseMessage[0].Message);
                }
            }

            this.unlockTcpClient();

            return true;
        }

        public Int64 RETRIEVESize(String _file)
        {
            Int32 fileSize = new Int32();

            this.lockTcpClient();

            this.Connect();

            List<FTPResponseMessage> responseMessage = this.sendCommand(FTPCommand.RETRIEVESize, _file);

            if (responseMessage[0].ResponseCode != FTPServerResponseCode.FileStatus)
            {
                throw new Exception(responseMessage[0].Message);
            }

            if (!Int32.TryParse(responseMessage[0].Message, out fileSize))
                throw new Exception("Unable to parse message: " + responseMessage[0].Message);

            this.unlockTcpClient();

            return fileSize;
        }


        public Int64 Upload(String _localfile)
        {
            return this.Upload(_localfile, Path.GetFileName(_localfile), FTP.TransferModeType.BINARY);
        }

        public Int64 Upload(String _localfile, String _remoteFile)
        {
            return this.Upload(_localfile, _remoteFile, FTP.TransferModeType.BINARY);
        }

        public Int64 Upload(String _localfile, FTP.TransferModeType _transferModeType)
        {
            return this.Upload(_localfile, Path.GetFileName(_localfile), _transferModeType);
        }

        public Int64 Upload(String _localFile, String _remoteFile, FTP.TransferModeType _transferModeType)
        {
            FileStream fileStream = new FileStream(_localFile, FileMode.Open);
            TcpListener listner = null;
            TcpClient client = null;
            NetworkStream networkStream = null;

            List<FTPResponseMessage> responseMessages = new List<FTPResponseMessage>();

            Byte[] buffer = new Byte[this.BlockSize];
            Int32 bytes = new Int32();
            Int64 totalBytes = new Int32();


            this.lockTcpClient();

            this.setTransferType(_transferModeType);

            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                listner = this.createDataListner();
                listner.Start();
            }

            else
            {
                client = this.createDataClient();
            }


            responseMessages = this.sendCommand(FTPCommand.STORE, _remoteFile);

            if (responseMessages[0].ResponseCode != FTPServerResponseCode.OpenedNewConnection
              && responseMessages[0].ResponseCode != FTPServerResponseCode.DataConnectionAlreadyOpen
               )
            {
                throw new Exception(responseMessages[0].Message);
            }


            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                client = listner.AcceptTcpClient();
            }

            networkStream = client.GetStream();

            while (totalBytes < fileStream.Length)
            {
                bytes = (Int32)fileStream.Read(buffer, 0, this.BlockSize);
                totalBytes += bytes;
                networkStream.Write(buffer, 0, bytes);
            }

            networkStream.Close();
            client.Close();

            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                listner.Stop();
            }


            FTPResponseMessage errorMessage;

            if (responseMessages.Count == 1)
            {
                responseMessages = this.readResponse();
                errorMessage = responseMessages[0];
            }

            else
            {
                errorMessage = responseMessages[1];
            }

            if (errorMessage.ResponseCode != FTPServerResponseCode.DataConnectionClosing)
            {
                throw new Exception(errorMessage.Message);
            }

            fileStream.Close();

            this.unlockTcpClient();

            return totalBytes;
        }


        public Int64 Download(String _remoteFile)
        {

            String localFilePath = String.Format("{0}{1}",
                                        PropertyManager.Property_FTP.DownloadCachePath,
                                        _remoteFile
                                        );

            return this.Download(_remoteFile, localFilePath, FTP.TransferModeType.BINARY);
        }

        public Int64 Download(String _remoteFile, String _localfile)
        {
            return this.Download(_remoteFile, _localfile, FTP.TransferModeType.BINARY);
        }

        public Int64 Download(String _remoteFile, FTP.TransferModeType _transferModeType)
        {
            String localFilePath = String.Format("{0}{1}",
                                     PropertyManager.Property_FTP.DownloadCachePath,
                                     _remoteFile
                                     );

            return this.Download(_remoteFile, localFilePath, _transferModeType);
        }

        public Int64 Download(String _remoteFile, String _localFile, FTP.TransferModeType _transferModeType)
        {
            FileStream fileStream = new FileStream(_localFile, FileMode.Create);
            TcpListener listner = null;
            TcpClient client = null;
            NetworkStream networkStream = null;

            List<FTPResponseMessage> responseMessages = new List<FTPResponseMessage>();

            Byte[] buffer = new Byte[this.BlockSize];
            Int32 bytes = new Int32();
            Int64 totalBytes = new Int32();


            this.lockTcpClient();

            this.setTransferType(_transferModeType);

            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                listner = this.createDataListner();
                listner.Start();
            }

            else
            {
                client = this.createDataClient();
            }


            responseMessages = this.sendCommand(FTPCommand.RETRIEVE, _remoteFile);

            if (responseMessages[0].ResponseCode != FTPServerResponseCode.OpenedNewConnection
              && responseMessages[0].ResponseCode != FTPServerResponseCode.DataConnectionAlreadyOpen
               )
            {
                throw new Exception(responseMessages[0].Message);
            }


            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                client = listner.AcceptTcpClient();
            }

            networkStream = client.GetStream();

            do
            {
                bytes = (Int32)networkStream.Read(buffer, 0, this.BlockSize);
                totalBytes += bytes;

                fileStream.Write(buffer, 0, bytes);
            }
            while (bytes != 0);

            networkStream.Close();
            client.Close();

            if (this.ConnectionMode == FTP.ConnectionMode.ACTIVE)
            {
                listner.Stop();
            }


            FTPResponseMessage errorMessage;

            if (responseMessages.Count == 1)
            {
                responseMessages = this.readResponse();
                errorMessage = responseMessages[0];
            }

            else
            {
                errorMessage = responseMessages[1];
            }

            if (errorMessage.ResponseCode != FTPServerResponseCode.DataConnectionClosing)
            {
                throw new Exception(errorMessage.Message);
            }

            fileStream.Close();

            this.unlockTcpClient();

            return totalBytes;
        }

        #endregion Public Methods


        #endregion Methods
    }
}
