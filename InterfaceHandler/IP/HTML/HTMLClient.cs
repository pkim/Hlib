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
using System.IO;
using System.Configuration;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using HLib.Network.IP.HTTP;
using HLib.Network.IP.HTML;
using HtmlAgilityPack;

namespace HLib.Network.IP.HTML
{
    public class HTMLClient
    {
        
        #region enums

        // WebRequestMethod
        public enum WebRequestMethod { GET, POST }

        #endregion

        #region Constants

        public const Char URL_PARAMETER_SEPERATOR = '&';
             
        #endregion

        #region Attributes
        
        //WebProxy
        private WebProxy webProxy;

        // Cookies
        private CookieContainer cookieContainer;

        #endregion

        #region Properties

        //LogedIn
        public Boolean LogedIn { get; set; }

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

        public void getHTML(Uri _host, IList<HTTPParameter> _getParamters, IList<HTTPParameter> _postParamters,
                            Encoding _encoding, ref HtmlDocument _htmlDocument)
        {
          #region Objects

          HttpWebRequest webRequest = null;

          String url = String.Empty;
          String postParameters = String.Empty;
          String getParameters = String.Empty;

          #endregion Objects

          try
          {
            // prepare GET Paramters
            if (_getParamters != null && _getParamters.Count != 0)
            {
              foreach (HTTPParameter getParameter in _getParamters)
              {
                getParameters = String.Format("{0}{1}{2}",
                                    getParameters,
                                    getParameter,
                                    URL_PARAMETER_SEPERATOR
                                   );
              }

              getParameters = getParameters.Remove(getParameters.Length - 1);

              // generate URL
              url = String.Format("{0}?{1}", _host.AbsoluteUri, getParameters);

            }

            else
            {
              url = _host.AbsoluteUri;
            }

            // HTTPWebRequest
            webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Proxy = this.webProxy;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = this.cookieContainer;

            // prepare POST parameters
            if (_postParamters != null && _postParamters.Count != 0)
            {
              foreach (HTTPParameter postParameter in _postParamters)
              {
                postParameters += String.Format("{0}{1}",
                                        postParameter,
                                        URL_PARAMETER_SEPERATOR
                                       );
              }

              postParameters = postParameters.Remove(postParameters.Length - 1);

              webRequest.Method = WebRequestMethod.POST.ToString();
              webRequest.ContentLength = postParameters.Length;

              using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
              {
                requestWriter.Write(postParameters);
                requestWriter.Close();
              }
            }

            else
            {
              webRequest.Method = WebRequestMethod.GET.ToString();
            }

            using (HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse)
            {
              // add cookies

              foreach (Cookie cookie in webResponse.Cookies)
              {
                cookie.Path = "/";
                this.cookieContainer.Add(cookie);
              }

              using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()))
              {
                _htmlDocument.LoadHtml(responseReader.ReadToEnd());
                responseReader.Close();
              }
            }

            // END TRY
          }

          catch (WebException exception)
          {
            this.LogedIn = false;

            throw new Exception(String.Format("Host \"{0}\" can not be reached\nError: Connection timeout - {1}", _host, exception));
          }

          catch (Exception exception)
          {
            this.LogedIn = false;

            throw exception;
          }
        }

        public HtmlDocument getHTML(Uri _host, IList<HTTPParameter> _getParamters, IList<HTTPParameter> _postParamters, Encoding _encoding)
        {
          HtmlDocument htmlDocument = new HtmlDocument();
          try
          {
            this.getHTML(_host, _getParamters, _postParamters, _encoding, ref htmlDocument);
          }
          catch (Exception e)
          {
            throw e;
          }
       
          return htmlDocument;
        }

        public HtmlDocument getHTML(String _host, IList<HTTPParameter> _getParamters, IList<HTTPParameter> _postParamters, Encoding _encoding)
        {
          try
          {
            return this.getHTML(new Uri(_host), _getParamters, _postParamters, _encoding);
          }
          catch (Exception e)
          {
            throw e;
          }
        }

        /// <summary>
        /// This method will try to login to a http and html based application
        /// </summary>
        /// <param name="_host">The host of the web application</param>
        /// <param name="_postParamters">The post parameters of the request. Almost contains username and password</param>
        /// <param name="_webRequestMethod">GET or POST Method</param>
        /// <param name="_encoding">Encodig type. In most cases it is recommended to use 'Encoding.Default'</param>
        /// <returns>
        /// Returns the result of the request in form of a html page. It is hard to check if the login was successfull even 
        /// you don't know what should get compared with. So you have to do it yourself^^
        /// </returns>
        public HtmlDocument loginToServer(Uri _host, IList<HTTPParameter> _postParamters, WebRequestMethod _webRequestMethod, Encoding _encoding)
        {
            return this.getHTML(_host, null, _postParamters, _encoding);
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
