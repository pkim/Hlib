/*
 * Filename: FTPCommand.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * 
 * Description: 
 * 
 * Contains FTP commands that may be sent to an FTP server, including all commands that 
 * are standardized in RFC 959 by the IETF. All commands below are RFC 959 based unless 
 * stated otherwise. Note that most command-line FTP clients present their own set of 
 * commands to users. For example, GET is the common user command to download 
 * a file instead of the raw command RETR.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLib.Network.IP.FTP
{

    /// <summary>
    /// Contains FTP commands that may be sent to an FTP server, including all commands that 
    /// are standardized in RFC 959 by the IETF. All commands below are RFC 959 based unless 
    /// stated otherwise. Note that most command-line FTP clients present their own set of 
    /// commands to users. For example, GET is the common user command to download 
    /// a file instead of the raw command RETR.
    /// </summary>
    internal static class FTPCommand
    {
        /// <summary>
        /// This command tells the server to abort the previous FTP
        /// service command and any associated transfer of data.  The
        /// abort command may require "special action", as discussed in
        /// the Section on FTP Commands, to force recognition by the
        /// server.  No action is to be taken if the previous command
        /// has been completed (including data transfer).  The control
        /// connection is not to be closed by the server, but the data
        /// connection must be closed.
        /// 
        /// There are two cases for the server upon receipt of this
        /// command: (1) the FTP service command was already completed,
        /// or (2) the FTP service command is still in progress.
        /// </summary>
        public const String ABORT = "ABOR";

        /// <summary>
        /// The argument field is a Telnet String identifying the user's
        /// account.  The command is not necessarily related to the USER
        /// command, as some sites may require an account for login and
        /// others only for specific access, such as storing files.  In
        /// the latter case the command may arrive at any time.
        /// 
        /// There are reply codes to differentiate these cases for the
        /// automation: when account information is required for login,
        /// the response to a successful PASSword command is reply code
        /// 332.  On the other hand, if account information is NOT
        /// required for login, the reply to a successful PASSword
        /// command is 230; and if the account information is needed for
        /// a command issued later in the dialogue, the server should
        /// return a 332 or 532 reply depending on whether it stores
        /// (pending receipt of the ACCounT command) or discards the
        /// command, respectively.
        /// </summary>
        public const String ACCOUNT = "ACCT";
        
        /// <summary>
        /// Authentication/Security Data
        /// </summary>
        public const String AuthenticationSecurityData = "ADAT";

        /// <summary>
        /// This command may be required by some servers to reserve
        /// sufficient storage to accommodate the new file to be
        /// transferred.  The argument shall be a decimal integer
        /// representing the number of bytes (using the logical byte
        /// size) of storage to be reserved for the file.  For files
        /// sent with record or page structure a maximum record or page
        /// size (in logical bytes) might also be necessary; this is
        /// indicated by a decimal integer in a second argument field of
        /// </summary>
        public const String ALLOCATE = "ALLO";

        /// <summary>
        /// This command causes the server-DTP to accept the data
        /// transferred via the data connection and to store the data in
        /// a file at the server site.  If the file specified in the
        /// pathname exists at the server site, then the data shall be
        /// appended to that file; otherwise the file specified in the
        /// pathname shall be created at the server site.
        /// </summary>
        public const String APPEND = "APPE";

        /// <summary>
        /// Authentication/Security Mechanism
        /// </summary>
        public const String AuthenticationSecurityMechanism = "AUTH";

        /// <summary>
        /// Clear Command Channel
        /// </summary>
        public const String ClearCommandChannel = "CCC";

        /// <summary>
        /// This command is a special case of CWD, and is included to
        /// simplify the implementation of programs for transferring
        /// directory trees between operating systems having different
        /// syntaxes for naming the parent directory.  The reply codes
        /// shall be identical to the reply codes of CWD.
        /// </summary>
        public const String CHANGE_TO_PARENT_DIRECTORY = "CDUP";

        /// <summary>
        /// Confidentiality Protection Command
        /// </summary>
        public const String ProtectionCommand = "CONF";

        /// <summary>
        /// This command allows the user to work with a different
        /// directory or dataset for file storage or retrieval without
        /// altering his login or accounting information.  Transfer
        /// parameters are similarly unchanged.  The argument is a
        /// pathname specifying a directory or other system dependent
        /// file group designator.
        /// </summary>
        public const String CHANGE_WORKING_DIRECTORY = "CWD";

        /// <summary>
        /// This command causes the file specified in the pathname to be
        /// deleted at the server site.  If an extra level of protection
        /// is desired (such as the query, "Do you really wish to
        /// delete?"), it should be provided by the user-FTP process.
        /// </summary>
        public const String DELETE = "DELE";

        /// <summary>
        /// Privacy Protected Channel
        /// </summary>
        public const String ProtectedChannel = "ENC";

        /// <summary>
        /// Specifies an extended address and port to which the server should connect.
        /// </summary>
        public const String SpecifyExtendedAddress = "EPRT";

        /// <summary>
        /// Enter extended passive mode.
        /// </summary>
        public const String EnterExtendedPassiveMode = "EPSV";

        /// <summary>
        /// Get the feature list implemented by the server.
        /// </summary>
        public const String GetFeatureList = "FEAT";

        /// <summary>
        /// Language Negotiation
        /// </summary>
        public const String LanguageNegotiation = "LANG";

        /// <summary>
        /// This command causes a list to be sent from the server to the
        /// passive DTP.  If the pathname specifies a directory or other
        /// group of files, the server should transfer a list of files
        /// in the specified directory.  If the pathname specifies a
        /// file then the server should send current information on the
        /// file.  A null argument implies the user's current working or
        /// default directory.  The data transfer is over the data
        /// connection in type ASCII or type EBCDIC.  (The user must
        /// ensure that the TYPE is appropriately ASCII or EBCDIC).
        /// Since the information on a file may vary widely from system
        /// to system, this information may be hard to use automatically
        /// in a program, but may be quite useful to a human user.
        /// </summary>
        public const String LIST = "LIST";

        /// <summary>
        /// Specifies a long address and port to which the server should connect.
        /// </summary>
        public const String ServerConnectTo = "LPRT";

        /// <summary>
        /// Enter long passive mode.
        /// </summary>
        public const String EnterLongPassiveMode = "LPSV";

        /// <summary>
        /// Return the last-modified time of a specified file.
        /// </summary>
        public const String GetLastModifiedTimeOfFile = "MDTM";

        /// <summary>
        /// Integrity Protected Command
        /// </summary>
        public const String ProtectedCommand = "MIC";

        /// <summary>
        /// This command causes the directory specified in the pathname
        /// to be created as a directory (if the pathname is absolute)
        /// or as a subdirectory of the current working directory (if
        /// the pathname is relative).
        /// </summary>
        public const String MAKE_DIRECTORY = "MKD";

        /// <summary>
        /// Lists the contents of a directory if a directory is named.
        /// </summary>
        public const String ListDirectory = "MLSD";

        /// <summary>
        /// Provides data about exactly the Object named on its command line, and no others.
        /// </summary>
        public const String GetDataOfObject = "MLST";

        /// <summary>
        /// The argument is a single Telnet character code specifying
        /// the data transfer modes described in the Section on
        /// Transmission Modes.
        /// 
        /// The following codes are assigned for transfer modes:
        /// 
        ///     S - Stream
        ///     B - Block
        ///     C - Compressed
        ///     
        /// The default transfer mode is Stream.
        /// </summary>
        public const String TRANSFER_MODE = "MODE";

        /// <summary>
        /// This command causes a directory listing to be sent from
        /// server to user site.  The pathname should specify a
        /// directory or other system-specific file group descriptor; a
        /// null argument implies the current directory.  The server
        /// will return a stream of names of files and no other
        /// information.  The data will be transferred in ASCII or
        /// EBCDIC type over the data connection as valid pathname
        /// strings separated by <CRLF> or <NL>.  (Again the user must
        /// ensure that the TYPE is correct.)  This command is intended
        /// to return information that can be used by a program to
        /// further process the files automatically.  For example, in
        /// the implementation of a "multiple get" function.
        /// </summary>
        public const String NAME_LIST = "NLST";

        /// <summary>
        /// This command does not affect any parameters or previously
        /// entered commands. It specifies no action other than that the
        /// server send an OK reply.
        /// </summary>
        public const String NoOperation = "NOOP";

        /// <summary>
        /// Select options for a feature.
        /// </summary>
        public const String SelectOptions = "OPTS";

        /// <summary>
        /// The argument field is a Telnet String specifying the user's
        /// password.  This command must be immediately preceded by the
        /// user name command, and, for some sites, completes the user's
        /// identification for access control.  Since password
        /// information is quite sensitive, it is desirable in general
        /// to "mask" it or suppress typeout.  It appears that the
        /// server has no foolproof way to achieve this.  It is
        /// therefore the responsibility of the user-FTP process to hide
        /// the sensitive password information.
        /// </summary>
        public const String PASSWORD = "PASS";

        /// <summary>
        /// This command requests the server-DTP to "listen" on a data
        /// port (which is not its default data port) and to wait for a
        /// connection rather than initiate one upon receipt of a
        /// transfer command.  The response to this command includes the
        /// host and port address this server is listening on.
        /// </summary>
        public const String PASSIVE = "PASV";

        /// <summary>
        /// The argument is a HOST-PORT specification for the data port
        /// to be used in data connection.  There are defaults for both
        /// the user and server data ports, and under normal
        /// circumstances this command and its reply are not needed.  If
        /// this command is used, the argument is the concatenation of a
        /// 32-bit internet host address and a 16-bit TCP port address.
        /// This address information is broken into 8-bit fields and the
        /// value of each field is transmitted as a decimal number (in
        /// character String representation).  The fields are separated
        /// by commas.  A port command would be:
        /// 
        ///     PORT h1,h2,h3,h4,p1,p2
        ///     
        /// where h1 is the high order 8 bits of the internet host
        /// address.
        /// </summary>
        public const String DATA_PORT = "PORT";

        /// <summary>
        /// Data Channel Protection Level.
        /// </summary>
        public const String DataChannelProtectionLevel = "PROT";

        /// <summary>
        /// This command causes the name of the current working
        /// directory to be returned in the reply.
        /// </summary>
        public const String PRINT_WORKING_DIRECTORY = "PWD";

        /// <summary>
        /// This command terminates a USER and if file transfer is not
        /// in progress, the server closes the control connection.  If
        /// file transfer is in progress, the connection will remain
        /// open for result response and the server will then close it.
        /// If the user-process is transferring files for several USERs
        /// but does not wish to close and then reopen connections for
        /// each, then the REIN command should be used instead of QUIT.
        /// 
        /// An unexpected close on the control connection will cause the
        /// server to take the effective action of an abort (ABOR) and a
        /// logout (QUIT).
        /// </summary>
        public const String LOGOUT = "QUIT";

        /// <summary>
        /// This command terminates a USER, flushing all I/O and account
        /// information, except to allow any transfer in progress to be
        /// completed.  All parameters are reset to the default settings
        /// and the control connection is left open.  This is identical
        /// to the state in which a user finds himself immediately after
        /// the control connection is opened.  A USER command may be
        /// expected to follow.
        /// </summary>
        public const String REINITIALIZE = "REIN";

        /// <summary>
        /// The argument field represents the server marker at which
        /// file transfer is to be restarted.  This command does not
        /// cause file transfer but skips over the file to the specified
        /// data checkpoint.  This command shall be immediately followed
        /// by the appropriate FTP service command which shall cause
        /// file transfer to resume.
        /// </summary>
        public const String RESTART = "REST";

        /// <summary>
        /// This command causes the server-DTP to transfer a copy of the
        /// file, specified in the pathname, to the server- or user-DTP
        /// at the other end of the data connection.  The status and
        /// contents of the file at the server site shall be unaffected.
        /// </summary>
        public const String RETRIEVE = "RETR";

        /// <summary>
        /// This command causes the directory specified in the pathname
        /// to be removed as a directory (if the pathname is absolute)
        /// or as a subdirectory of the current working directory (if
        /// the pathname is relative).  See Appendix II.
        /// </summary>
        public const String REMOVE_DIRECTORY = "RMD";

        /// <summary>
        /// This command specifies the old pathname of the file which is
        /// to be renamed.  This command must be immediately followed by
        /// a "rename to" command specifying the new file pathname.
        /// </summary>
        public const String RENAME_FROM = "RNFR";

        /// <summary>
        /// This command specifies the new pathname of the file
        /// specified in the immediately preceding "rename from"
        /// command.  Together the two commands cause a file to be
        /// renamed.
        /// </summary>
        public const String RENAME_TO = "RNTO";

        /// <summary>
        /// This command is used by the server to provide services
        /// specific to his system that are essential to file transfer
        /// but not sufficiently universal to be included as commands in
        /// the protocol.  The nature of these services and the
        /// specification of their syntax can be stated in a reply to
        /// the HELP SITE command.
        /// </summary>
        public const String SITE_PARAMETERS = "SITE";

        /// <summary>
        /// Return the size of a file.
        /// </summary>
        public const String RETRIEVESize = "SIZE";

        /// <summary>
        /// This command allows the user to mount a different file
        /// system data structure without altering his login or
        /// accounting information.  Transfer parameters are similarly
        /// unchanged.  The argument is a pathname specifying a
        /// directory or other system dependent file group designator.
        /// </summary>
        public const String MountFileStructure = "SMNT";

        /// <summary>
        /// This command shall cause a status response to be sent over
        /// the control connection in the form of a reply.  The command
        /// may be sent during a file transfer (along with the Telnet IP
        /// and Synch signals in which
        /// case the server will respond with the status of the
        /// operation in progress, or it may be sent between file
        /// transfers.  In the latter case, the command may have an
        /// argument field.  If the argument is a pathname, the command
        /// is analogous to the "list" command except that data shall be
        /// transferred over the control connection.  If a partial
        /// pathname is given, the server may respond with a list of
        /// file names or attributes associated with that specification.
        /// If no argument is given, the server should return general
        /// status information about the server FTP process.  This
        /// should include current values of all transfer parameters and
        /// the status of connections.
        /// </summary>
        public const String STATUS = "STAT";

        /// <summary>
        /// This command shall cause the server to send helpful
        /// information regarding its implementation status over the
        /// control connection to the user.  The command may take an
        /// argument (e.g., any command name) and return more specific
        /// information as a response.  The reply is type 211 or 214.
        /// It is suggested that HELP be allowed before entering a USER
        /// command. The server may use this reply to specify
        /// site-dependent parameters, e.g., in response to HELP SITE.
        /// </summary>
        public const String HELP = "HELP";

        /// <summary>
        /// This command causes the server-DTP to accept the data
        /// transferred via the data connection and to store the data as
        /// a file at the server site.  If the file specified in the
        /// pathname exists at the server site, then its contents shall
        /// be replaced by the data being transferred.  A new file is
        /// created at the server site if the file specified in the
        /// pathname does not already exist.
        /// </summary>
        public const String STORE = "STOR";

        /// <summary>
        /// This command behaves like STOR except that the resultant
        /// file is to be created in the current directory under a name
        /// unique to that directory.  The 250 Transfer Started response
        /// must include the name generated.
        /// </summary>
        public const String STORE_UNIQE = "STOU";

        /// <summary>
        /// The argument is a single Telnet character code specifying
        /// file structure described in the Section on Data
        /// Representation and Storage.
        /// 
        /// The following codes are assigned for structure:
        /// 
        ///     F - File (no record structure)
        ///     R - Record structure
        ///     P - Page structure
        ///     
        /// The default structure is File.
        /// </summary>
        public const String SetFileTransferStructur = "STRU";

        /// <summary>
        /// This command is used to find out the type of operating
        /// system at the server.  The reply shall have as its first
        /// word one of the system names listed in the current version
        /// of the Assigned Numbers document [4].
        /// </summary>
        public const String SYSTEM = "SYST";

        /// <summary>
        ///  The argument specifies the representation type as described
        ///  in the Section on Data Representation and Storage.  Several
        ///  types take a second parameter.  The first parameter is
        ///  denoted by a single Telnet character, as is the second
        ///  Format parameter for ASCII and EBCDIC; the second parameter
        ///  for local byte is a decimal integer to indicate Bytesize.
        ///  The parameters are separated by a <SP> (Space, ASCII code
        ///  32).
        ///  
        /// The following codes are assigned for type:
        /// 
        ///                 \    /
        ///       A - ASCII |    | N - Non-print
        ///                 |    | T - Telnet format effectors
        ///       E - EBCDIC|    | C - Carriage Control (ASA)
        ///                 /    \
        ///       I - Image
        ///       
        ///       L <byte size> - Local byte Byte size
        /// </summary>
        public const String REPRESENTATION_TYPE = "TYPE";

        /// <summary>
        /// The argument field is a Telnet String identifying the user.
        /// The user identification is that which is required by the
        /// server for access to its file system.  This command will
        /// normally be the first command transmitted by the user after
        /// the control connections are made (some servers may require
        /// this).  Additional identification information in the form of
        /// a password and/or an account command may also be required by
        /// some servers.  Servers may allow a new USER command to be
        /// entered at any point in order to change the access control
        /// and/or accounting information.  This has the effect of
        /// flushing any user, password, and account information already
        /// supplied and beginning the login sequence again.  All
        /// transfer parameters are unchanged and any file transfer in
        /// progress is completed under the old access control
        /// parameters.
        /// </summary>
        public const String USER_NAME = "USER";
    }
}
