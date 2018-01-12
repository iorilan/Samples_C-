using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Imps.Services.CommonV4;
using System.Threading;

namespace Imps.Services.IDCService.Utility
{
    public class TransactionManager<V>
    {
        private object _syncRoot = new object();
        private int _pending_count_default = 3000;
        private int _poll_interval_default = 5000;
        private int _timeout_default = 25 * 1000;
        private Action<V> _timeoutCallback = null;
        private Dictionary<string, int> pending_values = new Dictionary<string, int>();
        private static bool _init = false;
        //private readonly Action _overPendingCallback = null;
        private static Dictionary<string, Container<string, V, DateTime>> _transList = new Dictionary<string, Container<string, V, DateTime>>();
        private Thread _thread;
        private static TransactionManager<V> _instance;
        //private static readonly ITracing tracing = TracingManager.GetTracing(typeof(TransactionManager<V>));

        private TransactionManager() { }

        public static TransactionManager<V> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TransactionManager<V>();
                    _instance.InitClearThread();
                }
                return _instance;
            }
        }
        public void Initialize(Action<V> timeoutCallback)
        {
            lock (_syncRoot)
            {
                Instance._timeoutCallback = timeoutCallback;
            }
        }
        public void Initialize(int timeout, int pending, int interval, Action<V> timeoutCallback)
        {
            lock (_syncRoot)
            {
                Initialize(timeoutCallback);
                Instance._timeout_default = timeout;
                Instance._pending_count_default = pending;
                Instance._poll_interval_default = interval;
            }
        }
        public void Initialize(int timeout, int pending, int interval, Dictionary<string, int> dic, Action<V> timeoutCallback)
        {
            lock (_syncRoot)
            {
                Initialize(timeout, pending, interval, timeoutCallback);
                Instance.pending_values = dic;
            }
        }

        private void InitClearThread()
        {
            try
            {
                if (_init) return;
                _thread = new Thread(new ThreadStart(ThreadProc));
                _thread.Name = string.Format("ManagerStar:{0}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_FFFFFFF"));
                _thread.IsBackground = true;
                _thread.Start();
                _init = true;
            }
            catch (Exception e)
            {
                _init = false;
                //tracing.Error(e, "InitClearThread exception.");
            }
        }
        public static int ConnectionPending
        {
            get
            {
                return _transList.Count;
            }
        }
        //private static Dictionary<string, Container<string, V, DateTime>> RequestList
        //{
        //    get
        //    {
        //        if (_transList == null)
        //            _transList = Singleton<Dictionary<string, Container<string, V, DateTime>>>.Instance;
        //        return _transList;
        //    }
        //}
        public string Register(V instance)
        {
            if (!_init)
                InitClearThread();
            if (_transList.Count < _pending_count_default)
                lock (_syncRoot)
                {
                    if (_transList.Count < _pending_count_default)
                    {
                        Guid guid = Guid.NewGuid();
                        DateTime now = DateTime.Now;
                        Container<string, V, DateTime> ret = new Container<string, V, DateTime>(guid.ToString(), instance, now);
                        _transList.Add(guid.ToString(), ret);
                        //tracing.InfoFmt("register guid - {0},register time - {1}.", guid.ToString(), now.ToString("HH:mm:ss"));
                        return guid.ToString();
                    }
                }
            //if (_overPendingCallback != null)
            //    _overPendingCallback.Invoke();
            return string.Empty;
        }
        public bool Register(V instance, string groupKey)
        {
            if (!_init)
                InitClearThread();
            int pending = _pending_count_default;
            if (pending_values.ContainsKey(groupKey))
                pending = pending_values[groupKey];
            if (_transList.Count < pending)
                lock (_syncRoot)
                {
                    if (_transList.Count < pending)
                    {
                        Guid guid = Guid.NewGuid();
                        DateTime now = DateTime.Now;
                        Container<string, V, DateTime> ret = new Container<string, V, DateTime>(guid.ToString(), instance, now);
                        _transList.Add(guid.ToString(), ret);
                        //tracing.InfoFmt("register guid - {0},register time - {1},register key - {2}", guid.ToString(), now.ToString("HH:mm:ss"), groupKey);
                        return true;
                    }
                }
            return false;
        }

        public bool Unregister(string key)
        {
            if (_transList.ContainsKey(key))
                lock (_syncRoot)
                {
                    if (_transList.ContainsKey(key))
                    {
                        //tracing.InfoFmt("unregister guid - {0},register time - {1},register time - {2}", key, _transList[key].State.ToString("HH:mm:ss"), DateTime.Now.ToString("HH:mm:ss"));
                        _transList.Remove(key);
                        return true;
                    }
                }
            return false;
        }
        void ThreadProc()
        {
            while (true)
            {
                try
                {
                    Container<string, V, DateTime>[] transactions = null;
                    lock (_syncRoot)
                    {
                        transactions = new Container<string, V, DateTime>[_transList.Count];
                        _transList.Values.CopyTo(transactions, 0);
                    }
                    DateTime now = DateTime.Now;
                    for (int i = 0; i < transactions.Length; ++i)
                    {
                        DateTime timeout = transactions[i].State.AddMilliseconds(_timeout_default);
                        if (timeout > now)
                            continue;
                        try
                        {
                            if (Unregister(transactions[i].Key))
                            {
                                if (_timeoutCallback != null)
                                    _timeoutCallback.Invoke(transactions[i].Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("OnTransactionTimeout.{0}", ex.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    Thread.Sleep(_poll_interval_default);
                }
            }
        }

    }
}
