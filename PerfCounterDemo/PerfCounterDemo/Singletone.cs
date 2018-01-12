using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerfCounterDemo
{
    public class Singletone<T> where T : new()
    {
        private static object syncObj = new object();

        private static T instance;

        public static T Instance
        {
            get
            {
                lock (syncObj)
                {
                    if (null == instance)
                    {
                        lock (syncObj)
                        {
                            instance = new T();
                        }
                    }

                }

                return instance;
            }
        }
    }
}
