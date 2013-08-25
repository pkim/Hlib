/*
 * Name: HTML_Client
 * Date: 03 April 2011
 * Author: Patrik Kimmeswenger
 * Description: Enthält die Methode um HTML communication aufzubauen
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace Handler.NetworkHandler.HTML
{
    public class HTMLClient
    {
        
        #region enums

        // WebRequestMethod
        public enum WebRequestMethod { GET, POST }

        #endregion


        #region Attributes

        //logedIn
        private Boolean logedIn;    //default is, setted by constructor, false
        
        //WebProxy
        private WebProxy webProxy;

        // Cookies
        private CookieContainer cookieContainer;

        #endregion


        #region Properties

        //LogedIn
        public Boolean LogedIn {
            
            get
            {
                return this.logedIn;
            }
        }

        //WebProxy
        public WebProxy WebProxy {
            
            set
            {
                this.webProxy = value;
            }
        }

        public Int32 ResponseStreamReadTimeout { get; set; }    //default is 3000 (ms) settet by constructor

        #endregion


        #region Constructor

        public HTMLClient()
        {
            //CookieContainer
            this.cookieContainer = new CookieContainer();
            
            //LogedIn
            this.logedIn = false;

            //Timeouts
            this.ResponseStreamReadTimeout = 3000;
        }

        public HTMLClient(WebProxy _webProxy) : this()
        {
            //WebProxy
            this.WebProxy = _webProxy;
        }

        #endregion

        #region Methods

        /// <summary>
        /// gets the html sourceCode from an expected URL
        /// </summary>
        /// 
        /// <param name="_url">
        /// URL of the WebSite which html sourceCode should be loaded
        /// </param>
        /// 
        /// <param name="_webRequestMethod">
        /// Method which should be used to send a request to webserver
        /// </param>
        /// 
        /// <param name="_encoding">
        /// Encoding type of the html sourceCode
        /// </param>
        /// 
        /// <returns>
        /// the requested html SourceCode
        /// </returns>
        public string get_HTML(string _url, WebRequestMethod _webRequestMethod, Encoding _encoding)
        {
            /* objects */

            // HTTPWeb
            HttpWebRequest  request  = null;
            HttpWebResponse response = null;

            // Stream Reader
            StreamReader streamReader = null;

            // HTML SourceCode
            string html_SourceCode = null;  


            /* try to set a request, reading the response 
             * and return the html sourceCode
             */
            try
            {
                // set request
                request                 = (HttpWebRequest)WebRequest.Create(_url);
                request.Method          = _webRequestMethod.ToString();
                request.ContentType     = "text/html; charset=utf-8";
                request.CookieContainer = this.cookieContainer;
                request.Proxy           = this.webProxy;
                
           
                // initialize response
                response = (HttpWebResponse)request.GetResponse();
                response.GetResponseStream().ReadTimeout = this.ResponseStreamReadTimeout;


                // get HTTP Response if response is able to read stream
                if (response.GetResponseStream().CanRead)
                {
                    // read straem
                    streamReader = new StreamReader(response.GetResponseStream(), _encoding);
                    html_SourceCode = streamReader.ReadToEnd();
                }

                return html_SourceCode;
            }

            // WebException
            catch (WebException)
            {
                throw new Exception("Host can not be reached\nError: Connection timeout");
            }

            // UriFormatException
            catch (UriFormatException)
            {
                throw new Exception("URL is incorrect");
            }

            // Any else Exception
            catch (Exception exception)
            {
                throw exception;
            }

            finally
            {
                // Close streamReader connection
                if(streamReader != null)
                    streamReader.Close();

                // Close HTML connection
                if(response != null)
                    response.Close();
            }
         
        }
    

        /// <summary>
        /// LogIn to webserver
        /// </summary>
        /// 
        /// <param name="_loginurl">
        /// URL of the login website
        /// </param>
        /// 
        /// <param name="_username">
        /// Username when needed
        /// </param>
        /// 
        /// <param name="_password">
        /// Password when needed
        /// </param>
        /// 
        /// <param name="_webRequestMethod">
        /// Method which should be used to send a request to webserver
        /// </param>
        public void loginToServer(String _loginurl, String _username, String _password, WebRequestMethod _webRequestMethod)
        {
            
            // objects
            HttpWebRequest   webRequest      = null;
            HttpWebResponse  webResponse     = null;
            StreamWriter     requestWriter   = null;
            String           postData        = null;
            StreamReader     responseReader  = null;
            CookieCollection ccCookies       = null;
            String           responseData = null;


            try
            {

                // pre initialization
                webRequest    = WebRequest.Create(_loginurl) as HttpWebRequest;
                postData      = String.Format("username={0}&password={1}&testcookies=1", _username, _password);


                // get needed cookies for login
                this.getCookies(_loginurl);
 

                // HTTPWebRequest
                webRequest.Proxy           = this.webProxy;
                webRequest.Method          = _webRequestMethod.ToString();
                webRequest.ContentLength   = postData.Length;
                webRequest.ContentType     = "application/x-www-form-urlencoded";
                webRequest.CookieContainer = this.cookieContainer;

                // write http request
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(postData);
                requestWriter.Close();

                // get response and set cookies
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                ccCookies   = webResponse.Cookies;

                /*
                for (int i = 0; i < ccCookies.Count; i++)
                {
                    Console.WriteLine("Name: " + ccCookies[i].Name + "\nWert: " + ccCookies[i].Value + "\nDomain: " + ccCookies[i].Domain);
                }*/

                // read Response
                responseReader = new StreamReader(webResponse.GetResponseStream());
                responseData = responseReader.ReadToEnd();
                responseReader.Close();

                // Login was successfull
                this.logedIn = true;
                Console.WriteLine("login successfull");

                // END TRY
            }

            catch (WebException)
            {

                Console.WriteLine("login failed");
                this.logedIn = false;

                throw new Exception("Host can not be reached\nError: Connection timeout");
            }

            catch (Exception exception)
            {

                Console.WriteLine("login failed");
                this.logedIn = false;

                throw exception;
            }

            finally
            {
                // Close requestWriter
                if (requestWriter != null)
                    requestWriter.Close();

                // Close responseReader
                if(responseReader != null)
                    responseReader.Close();
            }
            
        }


        /// <summary>
        /// loads the cookies which are needed for the secure login
        /// </summary>
        /// 
        /// <param name="_url">
        /// the url of the website from where cookies should be loaded
        /// </param>
        public void getCookies(string _url)
        {
            // HttpWeb
            HttpWebRequest  httpWebRequest;
            HttpWebResponse httpWebResponse;

            // Cookies
            CookieCollection cookieCollection;
            String           cookieString = "";
            String           myCookies; 

            // StreamRader
            StreamReader streamResponseReader;
            String       streamResponseData;


            //HttpWebRequest
            httpWebRequest = WebRequest.Create(_url) as HttpWebRequest;

            httpWebRequest.Method          = "GET";
            httpWebRequest.ContentType     = "text/html; charset=utf-8";
            httpWebRequest.Proxy           = this.webProxy;
            this.cookieContainer           = new CookieContainer();
            httpWebRequest.CookieContainer = cookieContainer;

            
            // try to getHttpWebResponse
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }

            catch (Exception exception)
            {
                throw exception;
            }


            // Cookies
            cookieCollection = httpWebResponse.Cookies;
            myCookies = "";
            cookieString = "";

            // print cookies
            for (int i = 0; i < cookieCollection.Count; i++)
            {

                myCookies += "Name: " + cookieCollection[i].Name + "\nWert: " + cookieCollection[i].Value + "\nDomain: " + cookieCollection[i].Domain + "\n\n";
                cookieString += cookieCollection[i].Name + "=" + cookieCollection[i].Value + "; ";

            }


            //StreamRsponseReader
            streamResponseReader = new StreamReader(httpWebResponse.GetResponseStream());

            try
            {
                streamResponseData = streamResponseReader.ReadToEnd();
            }

            catch (Exception exception)
            {
                throw exception;
            }

            finally
            {
                streamResponseReader.Close();
            }

        }


        /// <summary>
        /// Allow all certifacates
        /// </summary>
        public void allow_AllCertificates()
        {      
            // allows for validation of SSL conversations
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }


        /// <summary>
        /// Deny all Certificates (Default)
        /// </summary>
        public void deny_AllCertifacates()
        {
            // denies for validation of SSL conversations
            ServicePointManager.ServerCertificateValidationCallback = delegate { return false; };
        }

        #endregion
    }
}
