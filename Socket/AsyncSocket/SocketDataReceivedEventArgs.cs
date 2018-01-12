using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imps.Fmc.ESMSAdapter
{
    [Serializable]
    public class SocketDataReceivedEventArgs : EventArgs
    {
        private byte[] _buffer;
        private int _size;

        public byte[] Buffer
        {
            get { return _buffer; }
        }

        public int Size
        {
            get { return _size; }
        }

        public SocketDataReceivedEventArgs(byte[] buffer)
        {
            _buffer = buffer;
        }

        internal void SetSize(int size)
        {
            _size = size;
        }

    }

    public delegate void SocketDataReceivedEventHandler(object sender, SocketDataReceivedEventArgs e);
}
