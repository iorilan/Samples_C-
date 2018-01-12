 public class CacheHelper
    {
        #region  �������

        //���ݽ����޸ĵ�ʵ������ �޸ĺ�ÿ�γ�������ǰ������ִ��Initialize()����������һ������ʵ��

        //volatile �ؼ��ֱ�ʾ�ֶο��ܱ��������ִ���߳��޸�
        protected static volatile System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;

        protected int _timeOut = 720; // Ĭ�ϻ�������Ϊ720����(12Сʱ)

        private static object syncObj = new object();

        private static CacheHelper instance;
        private static readonly object syncRoot = new object();

        /// <summary>
        /// ��ȡ�������ʵ��
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

        //���õ������ʱ��[��λ��������] 
        public int TimeOut
        {
            set { _timeOut = value > 0 ? value : 6000; }
            get { return _timeOut > 0 ? _timeOut : 6000; }
        }

        /// <summary>
        /// ��ȡȫ�ֻ������
        /// </summary>
        public static System.Web.Caching.Cache GetWebCacheObj
        {
            get { return webCache; }
        }

        /// <summary>
        /// ���һ���������ڵĻ���
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public void AddObjectNoExpire(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            //�Ƴ�����Ļص�ί��
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);
            
            webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
        }

        public void AddObject(string objId, object o, int expire)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            //�Ƴ�����Ļص�ί��
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
        /// ���뵱ǰ���󵽻�����
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        public void AddObject(string objId, object o)
        {

            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            //�Ƴ�����Ļص�ί��
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

        //�����ص�ί�е�һ��ʵ��
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

            //����Ҫʹ�û�����־,����Ҫʹ��������� ���û�����־���ٻ����ڴ��Ƿ�����
            //myLogVisitor.WriteLog(this,key,val,reason);.
            //MatchTools.WriteLog(this.ToString()+" "+key+" "+val.ToString()+" "+reason.ToString());

        }


        /// <summary>
        /// ɾ���������
        /// </summary>
        /// <param name="objId">����Ĺؼ���</param>
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
        /// ������л������
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
        /// ����һ��ָ���Ķ���
        /// </summary>
        /// <param name="objId">����Ĺؼ���</param>
        /// <returns>����</returns>
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