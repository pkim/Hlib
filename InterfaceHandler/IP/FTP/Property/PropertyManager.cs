/*
 * Filename: PropertyManager.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * Description: Serves the FTP Property, where divers settings get de/serialized
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLib.Network.IP.FTP
{
    public class PropertyManager
    {
        public static Property_FTP Property_FTP = Property_FTP.GetInstance();
    }
}
