/*
 * Filename: FTPResponseMessage.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * 
 * Description: 
 * 
 * This struct is needed for the messages which the ftp server will have sent. 
 * When recieving the a message the revieve method in the ftpClient class will parse the message
 * and save the message and responsecode in this struct
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Handler.Interface.HLib.Network.IP.FTP
{
    public struct FTPResponseMessage
    {
        public String                Message      { get; set; }
        public FTPServerResponseCode ResponseCode { get; set; }
    }
}
