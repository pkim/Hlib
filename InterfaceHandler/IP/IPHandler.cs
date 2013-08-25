using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace HLib.Network.IP
{
    public class IPHandler
    {

        #region Constants

        /* IPAddress KeyValues */

        /// <summary>
        /// Liston on any IPAddress 
        /// </summary>
        public static IPAddress ANY_IPADDRESS
        { 
            get
            {
                return IPAddress.Any;
            }
        }

        /// <summary>
        /// Listen on no IPAddress 
        /// </summary>
        public static IPAddress NONE_IPADDRESS
        {
            get
            {
                return IPAddress.None;
            }
        }



        /* Port Range */

        /// <summary>
        /// the highest possible port of a IPv4 socket
        /// </summary>
        public const Int32 PORT_RANGE_MAX = 65536;

        /// <summary>
        /// the lowest possible port of a IPv4 socket
        /// </summary>
        public const Int32 PORT_RANGE_MIN = 1;

        #endregion


        /// <summary>
        /// Converts a String to a IPAddress
        /// </summary>
        /// <param name="_ipAdress">IPAddress as a String</param>
        /// <returns>the converted IPAddress Object or null if the given hostaddress isn't valid</returns>
        public static IPAddress getIPAddress(String _hostAddress)
        {
            try
            {
                
                // IPAddresslist for DNSresolve
                IPAddress[] ipAddressList;

                // IPAddress for parsing the hostAddress String
                IPAddress ipAddress;


                // is _hostAddress an IPAddress?
                if (IPAddress.TryParse(_hostAddress, out ipAddress))
                    return ipAddress;

                // is _hostAddress a Dnsname?
                ipAddressList = resolveDNS(_hostAddress);

                // if the method wasn't able to find a configured 
                if (ipAddressList.Length != 0)
                    return ipAddressList[0];    // return the array of IPAddresses

                // is no valid HostAddress
                return null;    //return an empty array because the method wasn't able to find any IPAddress
            }

            catch (Exception)
            { return null; }

        }

        /// <summary>
        /// Reads the configured Systems' network interface ipaddresses
        /// </summary>
        /// <param name="_hostAddress"></param>
        /// <returns></returns>
        private static IPAddress[] resolveDNS(String _hostAddress)
        {
            try
            {
                return Dns.GetHostAddresses(_hostAddress);
            }
            catch (Exception)
            { return null; }
        }

    }
}
