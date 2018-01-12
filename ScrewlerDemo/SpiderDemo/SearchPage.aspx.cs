using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SpiderDemo.SearchUtil;
using System.Threading;
using System.IO;
using SpiderDemo.Entity;

namespace SpiderDemo
{
    public partial class SearchPage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                InitSetting();
            }
        }

        private void InitSetting()
        {
          
        }

        private void StartWork()
        {
            CacheHelper.EnableSearch = true;
            CacheHelper.KeyWord = txtKeyword.Text;

            ////第一个请求给新浪，获得返回的HTML流
            Stream htmlStream = HttpPostUtility.SendReq(CacheHelper.StartUrl);

            Link startLnk = new Link()
            {
                Href = CacheHelper.StartUrl,
                LinkName = "<a href = '" + CacheHelper.StartUrl + "' > 新浪 " + CacheHelper.StartUrl + " </a>"
            };

            ////解析出连接
            UrlAnalysisProcessor.GetHrefs(startLnk, htmlStream, CacheHelper.LnkPool);

            
            
            for (int i = 0; i < CacheHelper.ThreadList.Length; i++)
            {
                CacheHelper.ThreadList[i] = new ClamThread();
                CacheHelper.ThreadList[i].lnkPool = new List<Link>();
            }

            ////把连接平分给每个线程
            for (int i = 0; i < CacheHelper.LnkPool.Count / 2; i++)
            {
                int tIndex = i % CacheHelper.ThreadList.Length;
                CacheHelper.ThreadList[tIndex].lnkPool.Add(CacheHelper.LnkPool[i]);
            }

            Action<ClamThread> clamIt = new Action<ClamThread>((clt) =>
            {
                if (clt.lnkPool.Count > 0)
                {
                    Stream s = HttpPostUtility.SendReq(clt.lnkPool[0].Href);
                    DoIt(clt, s, clt.lnkPool[0]);
                }
            });


            for (int i = 0; i < CacheHelper.ThreadList.Length; i++)
            {
                CacheHelper.ThreadList[i]._thread = new Thread(new ThreadStart(() =>
                {
                    clamIt(CacheHelper.ThreadList[i]);
                }));

                /////每个线程开始工作的时候，休眠100ms
                CacheHelper.ThreadList[i]._thread.Start();
                Thread.Sleep(300);
            }
          

        }

        private void DoIt(ClamThread thread, Stream htmlStream, Link url)
        {

            if (!CacheHelper.EnableSearch)
            {
                return;
            }

            if (CacheHelper.SpideNum > CacheHelper.MaxResult)
            {
                return;
            }

            ////解析页面,URL符合条件放入缓存，并把页面的连接抓出来放入缓存
            UrlAnalysisProcessor.GetHrefs(url, htmlStream, thread.lnkPool);

            ////如果有连接，拿第一个发请求，没有就结束吧，反正这么耗资源的东西
            if (thread.lnkPool.Count > 0)
            {
                Link firstLnk;
                firstLnk = thread.lnkPool[0];
                ////拿到连接之后就在缓存中移除
                thread.lnkPool.Remove(firstLnk);

                firstLnk.TheadId = Thread.CurrentThread.ManagedThreadId;
                Stream content = HttpPostUtility.SendReq(firstLnk.Href);

                DoIt(thread, content, firstLnk);
            }
            else
            {
                //没连接了，停止吧,看其他线程的表现
                thread._thread.Abort();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.StartWork();

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {

        }

        protected void btnStop_Click(object sender, EventArgs e)
        {
            foreach (var t in CacheHelper.ThreadList)
            {
                t._thread.Abort();
                t._thread.DisableComObjectEagerCleanup();
            }
            CacheHelper.EnableSearch = false;
            //CacheHelper.ValidLnk.Clear();
            CacheHelper.LnkPool.Clear();
            CacheHelper.validLnk.Clear();
        }
    }
}