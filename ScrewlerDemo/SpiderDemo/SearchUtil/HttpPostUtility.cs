using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace SpiderDemo.SearchUtil
{
    public static class HttpPostUtility
    {
        /// <summary>
        /// 暂时写成同步的吧，等后期再优化
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream SendReq(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) {
                    return null;
                }
               //  WebProxy wp = new WebProxy("10.0.1.33:8080");
               // wp.Credentials = new System.Net.NetworkCredential("lanliang", "Pass@word1", "feinno");

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                //myRequest.Proxy = wp;
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

                Stream s= myResponse.GetResponseStream();
                return s;
            }
            ////给一些网站发请求权限会受到限制
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}