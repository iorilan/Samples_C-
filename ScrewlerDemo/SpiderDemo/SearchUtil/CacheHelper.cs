using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpiderDemo.Entity;
using System.Threading;

namespace SpiderDemo.SearchUtil
{
    public static class CacheHelper
    {
        public static bool EnableSearch;

        /// <summary>
        /// 起始URL
        /// </summary>
        public const string StartUrl = "http://www.sina.com.cn";


        /// <summary>
        /// 爬取的最大数量，性能优化一下，如果可以及时释放资源就可以一直爬了
        /// </summary>
        public const int MaxNum = 1000;

        /// <summary>
        /// 最多爬出1000个结果
        /// </summary>
        public const int MaxResult = 1000;


        /// <summary>
        /// 当前爬到的数量
        /// </summary>
        public static int SpideNum;

        /// <summary>
        /// 关键字
        /// </summary>
        public static string KeyWord;

        /// <summary>
        /// 运行时间
        /// </summary>
        public static int RuningTime;

        /// <summary>
        /// 最多运行时间
        /// </summary>
        public static int MaxRuningtime;

        /// <summary>
        /// 10个线程同时去爬
        /// </summary>
        public static ClamThread[] ThreadList = new ClamThread[10];

        /// <summary>
        /// 第一次爬到的连接，连接池
        /// </summary>
        public static List<Link> LnkPool = new List<Link>();

        /// <summary>
        /// 拿到的合法连接
        /// </summary>
        public static List<Link> validLnk = new List<Link>();

        /// <summary>
        /// 拿连接的时候  不要拿同样的
        /// </summary>
        public static readonly object syncObj = new object();
    }
}