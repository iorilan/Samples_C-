using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AsyncHttpRequestDemo
{
    public class AsynStream
    {
        int _offset;
        /// <summary>
        /// 偏移量
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        int _count;
        /// <summary>
        /// 读取次数
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        int _unreadLength;
        /// <summary>
        /// 没有读取的长度
        /// </summary>
        public int UnreadLength
        {
            get { return _unreadLength; }
            set { _unreadLength = value; }
        }
        public AsynStream(int offset, int count, Stream netStream)
        {
            _offset = offset;
            _count = count;
            _netStream = netStream;
        }
        public AsynStream(Stream netStream)
        {
            _netStream = netStream;
        }


        Stream _netStream;

        public Stream NetStream
        {
            get { return _netStream; }
            set { _netStream = value; }
        }

    }
}
