/*
 * Filename: FTPServerResponseCode.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * 
 * Description: 
 * 
 * During FTP sessions, servers send and receive various numbered codes to/from FTP clients. 
 * Some codes represent errors, most others simply communicate the status of the connection. 
 * 
 * Some codes are informational only, others indicate that you have entered the wrong information, 
 * and others indicate what the information is that you need to provide before continuing.
 * 
 * The first digit of the reply code is used to indicate one of three possible outcomes, 1) success, 2) failure, and 3) error or incomplete:
 * 
 * 2xx - Success reply
 * 4xx or 5xx - Failure Reply
 * 1xx or 3xx - Error or Incomplete reply
 * 
 * The second digit defines the kind of error:
 * 
 * x0z - Syntax - These replies refer to syntax errors.
 * x1z - Information - Replies to requests for information.
 * x2z - Connections - Replies referring to the control and data connections.
 * x3z - Authentication and accounting - Replies for the login process and accounting procedures.
 * x4z - Not defined.
 * x5z - File system - These replschies relay status codes from the server file system.
 * 
 * The third digit of the reply code is used to provide additional detail for each of the categories defined by the second digit.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handler.Interface.NetworkHandler.IP.FTP
{

    /// <summary>
    /// ##############################
    /// # FTP Status and Error Codes #
    /// ##############################
    /// 
    /// During FTP sessions, servers send and receive various numbered codes to/from FTP clients. 
    /// Some codes represent errors, most others simply communicate the status of the connection. 
    /// 
    /// Some codes are informational only, others indicate that you have entered the wrong information, 
    /// and others indicate what the information is that you need to provide before continuing.
    /// 
    /// The first digit of the reply code is used to indicate one of three possible outcomes, 1) success, 2) failure, and 3) error or incomplete:
    /// 
    /// 2xx - Success reply
    /// 4xx or 5xx - Failure Reply
    /// 1xx or 3xx - Error or Incomplete reply
    /// 
    /// The second digit defines the kind of error:
    /// 
    /// x0z - Syntax - These replies refer to syntax errors.
    /// x1z - Information - Replies to requests for information.
    /// x2z - Connections - Replies referring to the control and data connections.
    /// x3z - Authentication and accounting - Replies for the login process and accounting procedures.
    /// x4z - Not defined.
    /// x5z - File system - These replschies relay status codes from the server file system.
    /// 
    /// The third digit of the reply code is used to provide additional detail for each of the categories defined by the second digit.
    /// </summary>
    public enum FTPServerResponseCode
    {

        #region Custom Series

        /// <summary>
        /// Means that a message has no response code
        /// </summary>
        None = 0,

        #endregion Custom Series


        /* Desciption *
         * 
         * Positive Preliminary reply
         * 
         * The requested action is being initiated, expect another reply before proceeding with a new command. 
         * (The user-proc227ess sending another command before the completion reply would be in violation of protocol, 
         * but server-FTP processes should queue any commands that arrive while a preceding command is in progress.) 
         * 
         * This type of reply can be used to indicate that the command was accepted and the user-process may now pay 
         * attention to the data connections, for implementations where simultaneous monitoring is difficult. 
         * The server-FTP process may send at most, one 1xx reply per command.
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        #region 100 Series

        /// <summary>
        /// The requested action is being initiated, expect another reply before proceeding with a new command.
        /// </summary>
        RequestedActionInitiaed = 100,

        /// <summary>
        /// Restart marker replay . In this case, the text is exact and not left to the particular implementation, it must read: MARK yyyy = mmmm where yyyy is USER_NAME-process data stream marker, and mmmm server's equivalent marker (note the spaces between markers and "=").
        /// </summary>
        Restart = 110,

        /// <summary>
        /// Service ready in nnn minutes.
        /// </summary>
        ServiceReady = 120,

        /// <summary>
        /// Data Connection already open, transfer starting.
        /// </summary>
        DataConnectionAlreadyOpen = 125,

        /// <summary>
        /// File status okay, about to open data connection.  FTP uses two ports: 21 for sending commands, and 20 for sending data. A status code of 150 indicates that the server is about to open a new connection on port 20 to send some data.
        /// </summary>
        OpenedNewConnection = 150,

        #endregion 100 Series


        /* Desciption *
             * 
             * Positive Completion reply
             * 
             * The requested action has been successfully completed. A new request may be initiated.
             * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        #region 200 Series

        /// <summary>
        /// Command okay.
        /// </summary>
        CommandOK = 200,

        /// <summary>
        /// Command not implemented, superfluous at this site.
        /// </summary>
        NoValidCommand = 202,

        /// <summary>
        /// System status, or system help reply.
        /// </summary>
        SystemStatus = 211,

        /// <summary>
        /// Directory status.
        /// </summary>
        DirectoryStatus = 212,

        /// <summary>
        /// File Status
        /// </summary>
        FileStatus = 213,

        /// <summary>
        /// Help message.
        /// </summary>
        HelpMessage = 214,

        /// <summary>
        /// NAME system type. (Where NAME is an official system name from the list in the Assigned Numbers document.)
        /// </summary>
        SystemName = 215,

        /// <summary>
        /// Service is ready for new user. Includes also the welcome message
        /// </summary>
        ServiceIsReadyForNewUser = 220,

        /// <summary>
        /// Service closing control connection. Logged out if appropriate.
        /// </summary>
        ServiceClosingControlConnection = 221,

        /// <summary>
        /// Data connection open, no transfer in progress.
        /// </summary>
        DataConnectionOpen = 225,

        /// <summary>
        /// Closing data connection. Requested file action successful (for example, file transfer or file abort). The command opens a data connection on port 20 to perform an action, such as transferring a file. This action successfully completes, and the data connection is closed.
        /// </summary>
        DataConnectionClosing = 226,

        /// <summary>
        /// Entering Passive Mode
        /// </summary>
        EnteringPassiveMode = 227,

        /// <summary>
        /// USER_NAME logged in, proceed. This status code appears after the client sends the correct password. It indicates that the user has successfully logged on.
        /// </summary>
        UserLoggedIn = 230,

        /// <summary>
        /// Requested file action okay, completed.
        /// </summary>
        RequestedFileActionCompleted = 250,

        /// <summary>
        /// "PATHNAME" created.
        /// </summary>
        DirectoryCreated = 257,

        #endregion 200 Series


        /* Desciption *
             * 
             * Positive Intermediate reply
             * 	
             * The command has been accepted, but the requested action is being held in abeyance, pending receipt of further 
             * information. The user should send another command specifying this information. This reply is used in command 
             * sequence groups.
             * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        #region 300 Series

        /// <summary>
        /// USER_NAME name okay, need password. 
        /// </summary>
        UserNameOKNeedPassword = 331,

        /// <summary>
        /// Need account for login.
        /// </summary>
        NeedAcount = 332,

        /// <summary>
        /// Requested file action pending further information.
        /// </summary>
        RequestedFileActionNeedFurtherInformation = 350,

        #endregion 300 Series


        /* Desciption *
             * 
             * Transient Negative Completion reply
             *
             * The command was not accepted and the requested action did not take place, but the error condition is temporary 
             * and the action may be requested again. The user should return to the beginning of the command sequence, if any. 
             * 
             * It is difficult to assign a meaning to "transient", particularly when two distinct sites 
             * (Server- and USER_NAME-processes) have to agree on the interpretation. 
             * 
             * Each reply in the 4xx category might have a slightly different time value, but the intent is that the user-process 
             * is encouraged to try again. A rule of thumb in determining if a reply fits into the 4xx or the 5xx 
             * (Permanent Negative) category is that replies are 4xx if the commands can be repeated without any change in 
             * command form or in properties of the USER_NAME or Server (e.g., the command is spelled the same with the 
             * same arguments used, 
             * 
             * The user does not change his file access or user name, the server does not put up a new implementation.) 
             * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        #region 400 Series

        /// <summary>
        /// Error 421 Service not available, closing control connection.
        /// </summary>
        ServiceNotAvailable = 421,

        /// <summary>
        /// Error 421 USER_NAME limit reached
        /// </summary>
        UserLimitReached = 421,

        /// <summary>
        /// Error 421 You are not authorized to make the connection
        /// </summary>
        NotAuthorizedOpenConnection = 421,

        /// <summary>
        /// Error 421 Max connections reached
        /// </summary>
        MaxConnectionsReached = 421,

        /// <summary>
        /// Error 421 Max connections exceeded
        /// </summary>
        MaxConnectionsExceeded = 421,

        /// <summary>
        /// Cannot open data connection.
        /// </summary>
        DataConnectionFailed = 425,

        /// <summary>
        /// Connection closed, transfer aborted. The command opens a data connection to perform an action, but that action is canceled, and the data connection is closed.
        /// </summary>
        DataConnectionClosed = 426,

        /// <summary>
        /// Requested file action not taken. File unavailable (e.g., file busy).
        /// </summary>
        FileUnavailable = 450,

        /// <summary>
        /// Requested action aborted: local error in processing.
        /// </summary>
        RequestedActionAborted = 451,

        /// <summary>
        /// Requested action not taken. Insufficient storage space in system.
        /// </summary>
        InsuficientStorageSpace = 452,

        #endregion 400 Series


        /* Desciption *
             * 
             * Permanent Negative Completion reply
             * 	
             * The command was not accepted and the requested action did not take place. The USER_NAME-process is discouraged from 
             * repeating the exact request (in the same sequence). Even some "permanent" error conditions can be corrected, so 
             * the human user may want to direct his USER_NAME-process to reinitiate the command sequence by direct action at some 
             * point in the future (e.g., after the spelling has been changed, or the user has altered his directory status.)
             * 
             * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        #region 500 Series

        /// <summary>
        /// Syntax error, command unrecognized, command line too long.
        /// </summary>
        SyntaxErrorInCommand = 500,

        /// <summary>
        /// Syntax error in parameters or arguments.
        /// </summary>
        SyntaxErrorInParameters = 501,

        /// <summary>
        /// Command not implemented.
        /// </summary>
        CommandNotImplemented = 502,

        /// <summary>
        /// Bad sequence of commands.
        /// </summary>
        BadSequenceOfCommands = 503,

        /// <summary>
        /// Command not implemented for that parameter.
        /// </summary>
        BadParameter = 504,

        /// <summary>
        /// USER_NAME not logged in. 
        /// </summary>
        UserNotLoggedIn = 530,

        /// <summary>
        /// Need account for storing files.
        /// </summary>
        NeedAccountForStpringFiles = 532,

        /// <summary>
        /// Requested action not taken. File not found
        /// </summary>
        FileNotFound = 550,

        /// <summary>
        /// Requested action not taken. File not accessible
        /// </summary>
        FileNotAccessilbe = 550,

        /// <summary>
        /// Requested file action aborted. Exceeded storage allocation. 
        /// </summary>
        RequestedFileActionAborted = 552,

        /// <summary>
        /// Requested action not taken. File name not allowed.  
        /// </summary>
        FileNameNotAllowd = 553,

        #endregion 500 Series


        /* Desciption *
             * 	Protected reply
             * 	
             * The RFC 2228 introduced the concept of protected replies to increase security over the FTP communications. 
             * The 6xx replies are Base64 encoded protected messages that serves as responses to secure commands. 
             * When properly decoded, these replies fall into the above categories.
             * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        #region 600 Series

        /// <summary>
        /// Integrity protected reply.
        /// </summary>
        IntegrityProtected = 631,

        /// <summary>
        /// Confidentiality and integrity protected reply.
        /// </summary>
        ConfidentialityAndIntegrityProected = 632,

        /// <summary>
        /// Confidentiality protected reply.
        /// </summary>
        ConfidentialityProtected = 633,

        #endregion 600 Series
    }
}
