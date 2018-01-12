using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmppStack.Helpers;

namespace SmppStack
{
    public class StreamBuffer
    {
        #region Private fields

        private const int PreferredBufferSize = 4096;
        private byte[] _buffer;
        private int _begin;
        private int _end;

        #endregion


        #region Constructor

        public StreamBuffer()
        {
            _buffer = new byte[PreferredBufferSize];
        }

        public StreamBuffer(int size)
        {
            _buffer = new byte[size];
            _end = size;
        }
        #endregion


        #region Internal properties

        internal byte[] InnerBuffer
        {
            get { return _buffer; }
        }

        #endregion


        #region Public properties

        public int BeginPosition
        {
            get { return _begin; }
            set { _begin = value; }
        }

        public int EndPosition
        {
            get { return _end; }
            set { _end = value; }
        }

        public int Size
        {
            get { return _end - _begin; }
        }

        public byte this[int pos]
        {
            get { return _buffer[GetExactOffset(pos)]; }
            set { _buffer[GetExactOffset(pos)] = value; }
        }

        #endregion


        #region Public methods

        public void Append(byte[] buffer, int offset, int size)
        {
            if (_begin > 0)
                Truncate();
            if (offset < 0)
                throw new ArgumentOutOfRangeException(string.Format("Argument offset={0} is out of range!", offset));
            if (_end < 0)
                throw new ArgumentOutOfRangeException(string.Format("Argument _end={0} is out of range!", _end));
            if (size < 0)
                throw new ArgumentOutOfRangeException(string.Format("Argument size={0} is out of range!", size));

            int emptySize = _buffer.Length - _end;
            if (emptySize < size)
                Realloc(_buffer.Length + size - emptySize);
            Buffer.BlockCopy(buffer, offset, _buffer, _end, size);
            _end += size;
        }

        public byte[] ToBytes()
        {
            if (_buffer.Length > _end)
                return BufferHelper.GetBlock(_buffer, _begin, _end - _begin);
            return _buffer;
        }

        public void Clear()
        {
            _buffer = new byte[PreferredBufferSize];
            _begin = 0;
            _end = 0;
        }

        #endregion


        #region Public Get Methods

        public byte[] GetBytes(int offset, int count)
        {
            if (_begin + offset + count > _buffer.Length)
                count = _buffer.Length - offset - _begin;
            return BufferHelper.GetBlock(_buffer, GetExactOffset(offset), count);
        }

        public int GetInt32(int offset)
        {
            return BitConverter.ToInt32(Reverse32(GetExactOffset(offset)), 0);
        }

        public uint GetUInt32(int offset)
        {
            return BitConverter.ToUInt32(Reverse32(GetExactOffset(offset)), 0);
        }

        public ulong GetUInt64(int offset)
        {
            return BitConverter.ToUInt64(_buffer, GetExactOffset(offset));
        }

        public bool GetBoolean(int offset)
        {
            return BitConverter.ToBoolean(_buffer, GetExactOffset(offset));
        }

       

        #endregion


        #region Public Set Methods

        // string一般是右补0，所以不用count
        public void SetValue(int offset, string value, int count)
        {
            byte[] buf = Encoding.ASCII.GetBytes(value);
            SetValue(offset, buf, buf.Length);
        }

        public void SetValue(int offset, int value)
        {
            SetValue(offset, Reverse32(BitConverter.GetBytes(value)), 4);
        }

        public void SetValue(int offset, uint value)
        {
            SetValue(offset, Reverse32(BitConverter.GetBytes(value)), 4);
        }

        public void SetValue(int offset, ulong value)
        {
            SetValue(offset, BitConverter.GetBytes(value), 8);
        }

        public void SetValue(int offset, bool value)
        {
            offset = GetExactOffset(offset);
            this[offset] = (byte)value.GetHashCode();
        }

        public void SetValue(int offset, byte[] value, int count)
        {
            offset = GetExactOffset(offset);
            Buffer.BlockCopy(value, 0, _buffer, offset, count);
        }

        #endregion


        #region Private methods

        private void Truncate()
        {
            Buffer.BlockCopy(_buffer, _begin, _buffer, 0, _end - _begin);
            _end = _end - _begin;
            _begin = 0;

            // always keep a small buffer
            if (_buffer.Length > PreferredBufferSize && _end < PreferredBufferSize)
            {
                byte[] buf = new byte[PreferredBufferSize];
                Buffer.BlockCopy(_buffer, 0, buf, 0, _end);
                _buffer = buf;
            }
        }

        private void Realloc(int size)
        {
            byte[] newBuf = new byte[size];
            Buffer.BlockCopy(_buffer, 0, newBuf, 0, _end);
            _buffer = newBuf;
        }

        private byte[] Reverse32(int offset)
        {
            int size = 4;
            byte[] buffer = BufferHelper.GetBlock(_buffer, offset, size);
            return Reverse32(buffer);
        }

        private byte[] Reverse32(byte[] buffer)
        {
            byte tmp = buffer[0];
            buffer[0] = buffer[3];
            buffer[3] = tmp;

            tmp = buffer[1];
            buffer[1] = buffer[2];
            buffer[2] = tmp;

            return buffer;
        }

        private int GetExactOffset(int offset)
        {
            return _begin + offset;
        }

        #endregion


    }
}
