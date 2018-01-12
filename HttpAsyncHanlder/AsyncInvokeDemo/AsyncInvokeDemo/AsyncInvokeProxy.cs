using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AsyncInvokeDemo
{
    public class AsyncInvokeProxy<T1>
    {
        private Action<T1> _task;

        public AsyncInvokeProxy(Action<T1> task)
        {
            this._task = task;
        }

        public void BeginEnvoke<T2>(T1 args, Action<T2, Exception> cb, T2 cbArgs)
        {
            this._task.BeginInvoke(args, new AsyncCallback((r) =>
            {
                try
                {
                    cb(cbArgs, null);
                    this._task.EndInvoke(r);
                }
                catch (Exception ex)
                {
                    cb(cbArgs, ex);
                }

            }), cbArgs);
        }
    }
}
