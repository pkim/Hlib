/*
 * Filename: FTP.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * 
 * Description: Serves properties, constants of the file transfer protocol. Also defaults are defined.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Handler.Interface.NetworkHandler.IP.FTP
{

    public class FTP
    {

        #region Constants

        public const Int32 DEFAULT_CONTROL_PORT  = 21;
        public const Int32 DEFAULT_TRANSFER_PORT = 20;

        public const Int32 DEFAULT_INACTIVE_TIMEOUT = 300;
        public const Int32 DEFAULT_TRANSFER_TIMEOUT = 420;
        public const Int32 DEFUALT_RECIEVE_TIMEOUT  = 10000;

        public const Int32 DEFAULT_BLOCK_SIZE = 512;

        public const Int32 DATA_PORT_RANGE_FROM = 1500;
        public const Int32 DATA_PORT_RANGE_TO   = 65000;

        public const Int32 RESPONSE_CODE_LENGTH = 3;

        

        public static readonly Encoding DEFAULT_ENCODING_TYPE = Encoding.ASCII;

        public static readonly SocketType   SocketType   = SocketType.Stream;

        public static readonly ProtocolType ProtocolType = ProtocolType.Tcp;

        public static readonly String TransferModeTypeASCII  = "A";
        public static readonly String TransferModeTypeBinary = "I";

       
        #endregion Constants


        #region Enums

        public enum ConnectionMode
        {
            ACTIVE,
            PASSIVE,
        }

        public enum TransferModeType
        {
            ASCII,
            BINARY,
        }

        /// <summary>
        /// The next consideration in transferring data is choosing the
        /// appropriate transmission mode.  There are three modes: one which
        /// formats the data and allows for restart procedures; one which also
        /// compresses the data for efficient transfer; and one which passes
        /// the data with little or no processing.  In this last case the mode
        /// interacts with the structure attribute to determine the type of
        /// processing.  In the compressed mode, the representation type
        /// determines the filler byte.
        /// 
        /// All data transfers must be completed with an end-of-file (EOF)
        /// which may be explicitly stated or implied by the closing of the
        /// data connection.  For files with record structure, all the
        /// end-of-record markers (EOR) are explicit, including the final one.
        /// For files transmitted in page structure a "last-page" page type is
        /// used.
        /// 
        /// NOTE:  In the rest of this section, byte means "transfer byte"
        /// except where explicitly stated otherwise.
        /// 
        /// For the purpose of standardized transfer, the sending host will
        /// translate its internal end of line or end of record denotation
        /// into the representation prescribed by the transfer mode and file
        /// structure, and the receiving host will perform the inverse
        /// translation to its internal denotation.  An IBM Mainframe record
        /// count field may not be recognized at another host, so the
        /// end-of-record information may be transferred as a two byte control
        /// code in Stream mode or as a flagged bit in a Block or Compressed
        /// mode descriptor.  End-of-line in an ASCII or EBCDIC file with no
        /// record structure should be indicated by <CRLF> or <NL>,
        /// respectively.  Since these transformations imply extra work for
        /// some systems, identical systems transferring non-record structured
        /// text files might wish to use a binary representation and stream
        /// mode for the transfer.
        /// </summary>
        public enum TransferMode
        {
            /// <summary>
            /// The data is transmitted as a stream of bytes.  There is no
            /// restriction on the representation type used; record structures
            /// are allowed.
            /// 
            /// In a record structured file EOR and EOF will each be indicated
            /// by a two-byte control code.  The first byte of the control code
            /// will be all ones, the escape character.  The second byte will
            /// have the low order bit on and zeros elsewhere for EOR and the
            /// second low order bit on for EOF; that is, the byte will have
            /// value 1 for EOR and value 2 for EOF.  EOR and EOF may be
            /// indicated together on the last byte transmitted by turning both
            /// low order bits on (i.e., the value 3).  If a byte of all ones
            /// was intended to be sent as data, it should be repeated in the
            /// second byte of the control code.
            /// 
            /// If the structure is a file structure, the EOF is indicated by
            /// the sending host closing the data connection and all bytes are
            /// data bytes.
            /// </summary>
            STREAM,

            /// <summary>
            /// The file is transmitted as a series of data blocks preceded by
            /// one or more header bytes.  The header bytes contain a count
            /// field, and descriptor code.  The count field indicates the
            /// total length of the data block in bytes, thus marking the
            /// beginning of the next data block (there are no filler bits).
            /// The descriptor code defines:  last block in the file (EOF) last
            /// block in the record (EOR), restart marker (see the Section on
            /// Error Recovery and Restart) or suspect data (i.e., the data
            /// being transferred is suspected of errors and is not reliable).
            /// This last code is NOT intended for error control within FTP.
            /// It is motivated by the desire of sites exchanging certain types
            /// of data (e.g., seismic or weather data) to send and receive all
            /// the data despite local errors (such as "magnetic tape read
            /// errors"), but to indicate in the transmission that certain
            /// portions are suspect).  Record structures are allowed in this
            /// mode, and any representation type may be used.
            /// 
            /// The header consists of the three bytes.  Of the 24 bits of
            /// header information, the 16 low order bits shall represent byte
            /// count, and the 8 high order bits shall represent descriptor
            /// codes as shown below.
            /// 
            ///          Block Header
            ///          
            /// +----------------+----------------+----------------+
            /// | Descriptor     |    Byte Count                   |
            /// |         8 bits |                      16 bits    |
            /// +----------------+----------------+----------------+
            /// 
            /// The descriptor codes are indicated by bit flags in the
            /// descriptor byte.  Four codes have been assigned, where each
            /// code number is the decimal value of the corresponding bit in
            /// the byte.
            /// 
            /// Code     Meaning
            /// 
            /// 128     End of data block is EOR
            /// 64     End of data block is EOF
            /// 32     Suspected errors in data block
            /// 16     Data block is a restart marker
            /// 
            /// With this encoding, more than one descriptor coded condition
            /// may exist for a particular block.  As many bits as necessary
            /// may be flagged.
            /// 
            /// The restart marker is embedded in the data stream as an
            /// integral number of 8-bit bytes representing printable
            /// characters in the language being used over the control
            /// connection (e.g., default--NVT-ASCII).  <SP> (Space, in the
            /// appropriate language) must not be used WITHIN a restart marker.
            /// 
            /// For example, to transmit a six-character marker, the following
            /// would be sent:
            /// 
            /// +--------+--------+--------+
            /// |Descrptr|  Byte count     |
            /// |code= 16|             = 6 |
            /// +--------+--------+--------+
            /// 
            /// +--------+--------+--------+
            /// | Marker | Marker | Marker |
            /// | 8 bits | 8 bits | 8 bits |
            /// +--------+--------+--------+
            /// 
            /// +--------+--------+--------+
            /// | Marker | Marker | Marker |
            /// | 8 bits | 8 bits | 8 bits |
            /// +--------+--------+--------+
            /// </summary>
            BLOCK,

            /// <summary>
            ///  There are three kinds of information to be sent:  regular data,
            ///  sent in a byte string; compressed data, consisting of
            ///  replications or filler; and control information, sent in a
            ///  two-byte escape sequence.  If n>0 bytes (up to 127) of regular
            ///  data are sent, these n bytes are preceded by a byte with the
            ///  left-most bit set to 0 and the right-most 7 bits containing the
            ///  number n.
            ///  
            /// Byte string:
            /// 
            /// 1       7                8                     8
            /// +-+-+-+-+-+-+-+-+ +-+-+-+-+-+-+-+-+     +-+-+-+-+-+-+-+-+
            /// |0|       n     | |    d(1)       | ... |      d(n)     |
            /// +-+-+-+-+-+-+-+-+ +-+-+-+-+-+-+-+-+     +-+-+-+-+-+-+-+-+
            ///                               ^             ^
            ///                               |---n bytes---|
            ///                                   of data
            /// 
            /// String of n data bytes d(1),..., d(n)
            /// Count n must be positive.
            /// 
            /// To compress a string of n replications of the data byte d, the
            /// following 2 bytes are sent:
            /// 
            /// Replicated Byte:
            /// 
            ///       2       6               8
            ///     +-+-+-+-+-+-+-+-+ +-+-+-+-+-+-+-+-+
            ///     |1 0|     n     | |       d       |
            ///     +-+-+-+-+-+-+-+-+ +-+-+-+-+-+-+-+-+
            ///     
            /// A string of n filler bytes can be compressed into a single
            /// byte, where the filler byte varies with the representation
            /// type.  If the type is ASCII or EBCDIC the filler byte is <SP>
            /// (Space, ASCII code 32, EBCDIC code 64).  If the type is Image
            /// or Local byte the filler is a zero byte.
            /// 
            /// Filler String:
            /// 
            ///      2       6
            ///    +-+-+-+-+-+-+-+-+
            ///    |1 1|     n     |
            ///    +-+-+-+-+-+-+-+-+
            ///    
            /// The escape sequence is a double byte, the first of which is the
            /// </summary>
            COMPRESSED,
        }

        #endregion Enums

    }
}
