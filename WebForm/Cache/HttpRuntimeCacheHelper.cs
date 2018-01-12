 public class CacheHelper
    {
        #region  缓存对象

        //根据建议修改单实例方法 修改后每次程序启动前必须先执行Initialize()方法，创建一个对象实例

        //volatile 关键字表示字段可能被多个并发执行线程修改
        protected static volatile System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;

        protected int _timeOut = 720; // 默认缓存存活期为720分钟(12小时)

        private static object syncObj = new object();

        private static CacheHelper instance;
        private static readonly object syncRoot = new object();

        /// <summary>
        /// 获取缓存对象实例
        /// </summary>
        /// <returns></returns>
        public static CacheHelper Getinstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CacheHelper();
                    }
                }
            }
            return instance;
        }

        //设置到期相对时间[单位：／分钟] 
        public int TimeOut
        {
            set { _timeOut = value > 0 ? value : 6000; }
            get { return _timeOut > 0 ? _timeOut : 6000; }
        }

        /// <summary>
        /// 获取全局缓存对象
        /// </summary>
        public static System.Web.Caching.Cache GetWebCacheObj
        {
            get { return webCache; }
        }

        /// <summary>
        /// 添加一个永不过期的缓存
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public void AddObjectNoExpire(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            //移除缓存的回调委托
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);
            
            webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
        }

        public void AddObject(string objId, object o, int expire)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            //移除缓存的回调委托
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            if (expire == 0)
            {
                webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, null, DateTime.Now.AddSeconds(expire), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        public void AddObject(string objId, object o)
        {

            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            //移除缓存的回调委托
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            if (TimeOut == 6000)
            {
                webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, null, DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }


        public void AddObjectWith(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            webCache.Insert(objId, o, null, System.DateTime.Now.AddHours(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
        }

        //建立回调委托的一个实例
        public void onRemove(string key, object val, CacheItemRemovedReason reason)
        {

            switch (reason)
            {
                case CacheItemRemovedReason.DependencyChanged:
                    break;
                case CacheItemRemovedReason.Expired:
                    {
                        //CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(this.onRemove);

                        //webCache.Insert(key, val, null, System.DateTime.Now.AddMinutes(TimeOut),
                        //    System.Web.Caching.Cache.NoSlidingExpiration,
                        //    System.Web.Caching.CacheItemPriority.High,
                        //    callBack);
                        break;
                    }
                case CacheItemRemovedReason.Removed:
                    {
                        break;
                    }
                case CacheItemRemovedReason.Underused:
                    {
                        //ToolsHelper.Tracing.WarnFmt("Cache Underused key={0}",key);
                        break;
                    }
                default: break;
            }

            //如需要使用缓存日志,则需要使用下面代码 可用缓冲日志跟踪缓存内存是否用完
            //myLogVisitor.WriteLog(this,key,val,reason);.
            //MatchTools.WriteLog(this.ToString()+" "+key+" "+val.ToString()+" "+reason.ToString());

        }


        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        public void RemoveObject(string objId)
        {
            //objectTable.Remove(objId);
            if (objId == null || objId.Length == 0)
            {
                return;
            }
            webCache.Remove(objId);
        }

        /// <summary>
        /// 清空所有缓存对象
        /// </summary>
        public void RemoveObjectAll()
        {
            IDictionaryEnumerator CacheEnum = webCache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                webCache.Remove(CacheEnum.Key.ToString());
            }            
        }


        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public object RetrieveObject(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return null;
            }

            return webCache.Get(objId);
        }

        #endregion
    }