/*
 * Filename: IFTPClient.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * 
 * Description: 
 * 
 * This interface is needed to serve the methods of a ftp Client as well as of a SFTP Client.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace Handler.Interface.NetworkHandler.IP.FTP
{
    public interface IFTPClient
    {

        #region Properties

        /// <summary>
        /// Is true if the client is connected successfully
        /// </summary>
        Boolean IsConnected
        { get; }

        /// <summary>
        /// Port number the FTP server is listening on
        /// </summary>
        Int16 Port { get; set; }

        /// <summary>
        /// IP address or hostname to connect to
        /// </summary>
        String Server { get; set; }

        /// <summary>
        /// Username to login as
        /// </summary>
        String User { get; set; }

        /// <summary>
        /// Password for account
        /// </summary>
        String Password { get; set; }

        /// <summary>
        /// The timeout (milliseconds) for waiting for connection
        /// </summary>
        Int32 ConnectTimeout { get; set; }

        /// <summary>
        /// The timeout (milliseconds) for waiting for data recieve
        /// </summary>
        Int32 TransferTimeout { get; set; }

        /// <summary>
        /// The size of the message blocks.
        /// Default is 512
        /// </summary>
        Int32 BlockSize { get; set; }

        /// <summary>
        /// The Encoding of the communication
        /// Default is ASCII
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// The Mode in which the FTPClient should connect to the ftp server.
        /// Default is Passive
        /// </summary>
        FTP.ConnectionMode ConnectionMode { get; set; }

        FTP.TransferMode TransferMode { get; set; }

        FTP.TransferModeType TransferModeType { get; set; }

        #endregion Properties


        #region Methods

        Boolean Disconnect();

        Boolean Connect(String _server, Int16 _port, String _user, String _password);

        Boolean Connect(String _server, String _user, String _password);

        Boolean Connect(String _server, String _user);

        Boolean Connect();

        Boolean Connect(String _server, Int32 _port, String _user, String _password);

        List<FTPItem> GetItems();

        List<FTPItem> GetFiles();

        List<FTPItem> GetDirectories();

        List<FTPItem> GetItemsExtended();

        String GetFileDate(String _fileName);

        String GetWorkingDirectory();

        Boolean ChangeWorkingDirectory(String _path);

        Boolean MakeDirectory(String _directory);

        Boolean RemoveDirectory(String _directory);

        Boolean RemoveFile(String _file);

        Boolean RenameItem(String _file, String _newFileName);

        Int64 RETRIEVESize(String _file);

        Int64 Upload(String _localFile);

        Int64 Upload(String _localFile, String _remoteFile);

        Int64 Download(String _remoteFile);

        Int64 Download(String _remoteFile, String _localFile);
   
        #endregion Methods
    }
}
