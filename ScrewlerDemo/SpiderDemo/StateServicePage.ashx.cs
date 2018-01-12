using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SpiderDemo.SearchUtil;
using SpiderDemo.Entity;

namespace SpiderDemo
{
    /// <summary>
    /// StateServicePage 的摘要说明
    /// </summary>
    public class StateServicePage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";


            if (context.Request["op"] != null && context.Request["op"] == "info")
            {
                context.Response.Write(ShowState());
            }
        }


        public string ShowState()
        {
            StringBuilder sbRet = new StringBuilder(100);
            string ret = GetValidLnkStr();

            int count = 0;
            
                for (int i = 0; i < CacheHelper.ThreadList.Length; i++)
                {
                    if (CacheHelper.ThreadList[i] != null && CacheHelper.ThreadList[i].lnkPool != null)
                    count += CacheHelper.ThreadList[i].lnkPool.Count;
                }
            
            sbRet.AppendLine("服务是否运行 : " + CacheHelper.EnableSearch + "<br />");
            sbRet.AppendLine("连接池总数: " + count + "<br />");
            sbRet.AppendLine("搜索结果：<br /> " + ret);

            return sbRet.ToString();
        }

        private string GetValidLnkStr()
        {
            StringBuilder sb = new StringBuilder(120);
            Link[] cloneLnk = new Link[CacheHelper.validLnk.Count];

            CacheHelper.validLnk.CopyTo(cloneLnk, 0);

            for (int i = 0; i < cloneLnk.Length; i++)
            {
                sb.AppendLine("<br />" + cloneLnk[i].LinkName + "<br />" + cloneLnk[i].Context);
            }

            return sb.ToString();
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}