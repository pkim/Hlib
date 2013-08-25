/*
 * Name:            Proxy
 * Creation Date:   08 Jänner 2011
 * Modify Date:     11 April 2011
 * Author:          Patrik Kimmeswenger
 * Description:     Includes Methods and Properties to serve a Proxy
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NetworkHandler.IP.Proxy
{
    /// <summary>
    /// Includes Methods and Properties to serve a Proxy
    /// </summary>
    public class Proxy
    {
        
        #region Attributes

        private String webProxyAddress = "";
        private Int32  port            = 3128;
      
        #endregion


        #region Properties

        // Enabled
        /// <summary>
        /// memory if the proxy is en- or disabled
        /// </summary>
        public Boolean Enabled { get; set; }

        // WebProxy
        /// <summary>
        /// Address of the WebProxy
        /// as IP or URI
        /// </summary>
        public String WebProxyAdress
        {
            get
            {
                return this.webProxyAddress;
            }

            set
            {

                if (this.IsValidIP(value) || Uri.CheckSchemeName(value))
                {
                    this.webProxyAddress = value;
                }
            }
        }

        // Port
        /// <summary>
        /// Port of the ProxyServer
        /// A Value between 0 and 65536
        /// </summary>
        public int Port 
        {
            get
            {
                return this.port;
            }

            set
            {
                if (value >= 0 && value <= 65536)
                    this.port = value;
            }
        }

        // Username
        /// <summary>
        /// Username the Proxy needs to identitcates the user
        /// Often in use with Password
        /// </summary>
        public string Username { get; set; }

        // Password
        /// <summary>
        /// Password the Proxy needs to identicates the user
        /// often in use with Username
        /// </summary>
        public string Password { get; set; }
        

        #endregion


        #region Constructor

        public Proxy()
        {
        }

        public Proxy(string _webProxyAdress, int _port, string _username, string _password)
        {
            
            this.WebProxyAdress = _webProxyAdress;
            this.Port           = _port;
            this.Username       = _username;
            this.Password       = _password;

        }

        #endregion


        #region Methods

        /// <summary>
        /// Determines whether the specified string is an IP address.
        /// </summary>
        /// <param name="IP">The string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified IP is IP; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidIP(string IP)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(IP, @"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$\b");
        }

        /// <summary>
        /// Initializes an WebProxy, using the Properties and Attributes of the Proxy class
        /// </summary>
        /// 
        /// <returns>
        /// A WebProxy configured with the specified Attributes of the Proxy class
        /// </returns>
        public WebProxy getWebProxy()
        {
            //WebProxy
            WebProxy webProxy;

            // if the webProxyAddress is valid
            if (this.webProxyAddress.Length != 0)
            {
                // Creates a new instance of a WebProxy
                webProxy = new WebProxy(this.WebProxyAdress, this.Port);
            }

            // otherwise return null if the webProxyAddress is invalid
            else return null;


            // if the username and password are valid or setted
            if (this.Username.Length != 0 && this.Password.Length != 0)
            {
                //Create NetworkCredentials for the WebProxy, using username and password
                webProxy.Credentials = new NetworkCredential(this.Username, this.Password);
            }
            
            // return the WebProxy
            return webProxy;
        }

        #endregion

    }
}
