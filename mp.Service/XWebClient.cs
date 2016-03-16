using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace mp.Service
{
    public class XWebClient : WebClient
    {
        // Cookie 容器
        private CookieContainer cookieContainer = new CookieContainer();

        public string Get(string url, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; rv:41.0) Gecko/20100101 Firefox/41.0");

            var result = "";
            var tryCount = 0;
            while (true)
            {
                try
                {
                    result = encoding.GetString(DownloadData(url));
                    break;
                }
                catch
                {
                    tryCount++;
                    if (tryCount == 5)
                        break;
                }
            }
            return result;
        }

        public string Post(string url, object data = null, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:45.0) Gecko/20100101 Firefox/45.0");

            var values = new NameValueCollection();
            if (data != null)
            {
                foreach (var item in data.GetType().GetProperties())
                {
                    values.Add(item.Name, item.GetValue(data, null).ToString());
                }
            }

            var result = "";
            var tryCount = 0;
            while (true)
            {
                try
                {
                    result = encoding.GetString(UploadValues(url, values));
                    break;
                }
                catch
                {
                    tryCount++;
                    if (tryCount == 5)
                        break;
                }
            }
            return result;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                HttpWebRequest httpRequest = request as HttpWebRequest;
                httpRequest.CookieContainer = cookieContainer;
            }
            return request;
        }
    }

}
