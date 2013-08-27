/*
 * Filename: Property_FTP.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 02.01.2012
 * Description: Contains properties which are used for the ftp connection.
 */

using System;
using HLib.Settings.Property;

namespace HLib.Network.IP.FTP
{
    [Serializable]
    public class Property_FTP : Property<Property_FTP>
    {
        #region Properties

        #region Timeouts

        /// <summary>
        /// The timeout (milliseconds) for waiting for connection.
        /// The default is 300.
        /// </summary>
        public Int32 InactiveTimeout { get; set; }

        /// <summary>
        /// The timeout (milliseconds) for waiting for data send.
        /// The default is 420.
        /// </summary>
        public Int32 TransferTimeout { get; set; }

        /// <summary>
        /// The timeout (milliseconds) for waiting for data recieve.
        /// The default is 10000
        /// </summary>
        public Int32 RecieveTimeout { get; set; }

        #endregion Timeouts

        /// <summary>
        /// The path of the default local destination location
        /// </summary>
        public String DownloadCachePath { get; set; }

        /// <summary>
        /// The size of the message blocks.
        /// Default is 512
        /// </summary>
        public Int32 BlockSize { get; set; }


        /// <summary>
        /// The Encoding of the communication
        /// Default is ASCII
        /// </summary>
        public Int32 EncodingCodePage { get; set; }

        /// <summary>
        /// The mode in which the FTPClient should connect to the ftp server.
        /// Default is "passive" mode
        /// </summary>
        public FTP.ConnectionMode Connection_Mode { get; set; }

        /// <summary>
        /// The transfer mode.
        /// Default is STREAM.
        /// </summary>
        public FTP.TransferMode Transfer_Mode { get; set; }

        #endregion Properties       
        
        #region Constructor

        private Property_FTP()
        {
            this.InactiveTimeout = FTP.DEFAULT_INACTIVE_TIMEOUT;
            this.TransferTimeout = FTP.DEFAULT_TRANSFER_TIMEOUT;
            this.RecieveTimeout  = FTP.DEFUALT_RECIEVE_TIMEOUT;

            this.BlockSize = FTP.DEFAULT_BLOCK_SIZE;
            
            this.EncodingCodePage   = FTP.DEFAULT_ENCODING_TYPE.CodePage;
            this.Connection_Mode    = FTP.ConnectionMode.PASSIVE;
            this.Transfer_Mode      = FTP.TransferMode.STREAM;

            this.DownloadCachePath = String.Format(@"{0}\", Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
        }

        #endregion Cosntructor
    }
}
