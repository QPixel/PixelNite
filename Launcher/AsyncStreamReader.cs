using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PixelNiteLauncher
{
    internal class AsyncStreamReader
    {
        private StreamReader _reader;
        protected readonly byte[] _buffer;

        public event AsyncStreamReader.EventHandler<string> ValueRecieved;

        public bool Active { get; private set; }

        public AsyncStreamReader(StreamReader reader)
        {
            this._reader = reader;
            this._buffer = new byte[4096];
            this.Active = false;
        }

        protected void Begin()
        {
            if (!this.Active)
                return;
            this._reader.BaseStream.BeginRead(this._buffer, 0, this._buffer.Length, new AsyncCallback(this.Read), (object), null);
        }

        public void Start()
        {
            if (!this.Active)
                return;
            this.Active = true;
            this.Begin();
        }
        private void Read(IAsyncResult result)
        {
            if (this._reader == null)
                return;
            int count = this._reader.BaseStream.EndRead(result);
            string str = (string)null;
            if (count > 0)
            {
                str = this._reader.CurrentEncoding.GetString(this._buffer, 0, count);
            } else
            {
                this.Active = false;
            }
            AsyncStreamReader.EventHandler<string> valueRecieved = this.ValueRecieved;
            if (valueRecieved != null)
                valueRecieved((object)this, str);
            this.Begin();
        }

        public delegate void EventHandler<args>(object sender, string value);

    }
}
