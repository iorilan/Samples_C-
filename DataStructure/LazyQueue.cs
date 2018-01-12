using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerToolServer.Util
{

    /// <summary>
    /// 到指定的秒数或到达某个数量执行出队
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyQueue<T>
    {

        /// <summary>
        /// 当前的队列
        /// </summary>
        private Queue<T> _current;

        /// <summary>
        /// 回调，当时间达到maxSec或_curentQueue达到maxNum数量时，执行
        /// </summary>
        private Action<T[]> _dequeueAction;

        /// <summary>
        /// 队列的最大数量
        /// </summary>
        private int _maxNum;

        /// <summary>
        /// 监听队列的线程
        /// </summary>
        private Thread _executeThread;

        /// <summary>
        /// 当前秒数
        /// </summary>
        private int _currentSec = 0;

        /// <summary>
        /// 最大秒数，就执行这个方法
        /// </summary>
        private int _maxSec;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="sec">最迟的执行秒数</param>
        /// <param name="num">队列的最大数量</param>
        /// <param name="dequeueAction">出队函数</param>
        public LazyQueue(int sec, int num, Action<T[]> dequeueAction)
        {
            this._maxSec = sec;
            this._maxNum = num;
            this._dequeueAction = dequeueAction;
            this._current = new Queue<T>();
            this._currentSec = 0;

            _executeThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    this._currentSec++;
                    if (this._current.Count >= _maxNum || this._currentSec >= _maxSec)
                    {
                        this._dequeueAction.Invoke(this._current.ToArray());
                        
                        ////别忘了清空队列和秒数
                        this._current.Clear();
                        this._currentSec = 0;
                    }
                    ////每秒检测一下队列的数量和秒数
                    Thread.Sleep(1000);
                }
            }));
            this._executeThread.Start();

        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(T value)
        {
            this._current.Enqueue(value);
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            return this._current.Dequeue();
        }

        /// <summary>
        /// 清缓存
        /// </summary>
        public void Flush()
        {
            this._maxSec = 0;
            this._maxNum = 0;
            this._currentSec = 0;
            this._dequeueAction = null;
            this._current.Clear();
            this._executeThread.Abort();
        }

    }
}
