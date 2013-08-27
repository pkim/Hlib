using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLib.Network.IP.HTTP;

namespace HLib.Network.IP
{
    public class HTMLRequest
    {
        #region Objects

        public String Host { get; set; }
        public String Page { get; set; }

        public IList<HTTPParameter> HTMLParameters { get; set; }
        
        #endregion Objects


        #region Constructor

        public HTMLRequest(String _host, String _page, params HTTPParameter[] _htmlParamters)
        {
            this.Host = _host;
            this.Page = _page;
            this.HTMLParameters = _htmlParamters;
        }

        #endregion Constructor


        #region Methods

        public override String ToString()
        {
            String url = String.Format("{0}{1}", this.Host, this.Page);

            if(this.HTMLParameters.Count == 0)
            {
                return url;
            }

            url = String.Format("{0}?", url);

            foreach (HTTPParameter htmlParamter in this.HTMLParameters)
            {
                String.Format("{0}{1}&", url, htmlParamter);
            }

            // remove last '&'
            url = url.Remove(url.Length - 1);

            return url;
        }

        #endregion Methods
    }
}
