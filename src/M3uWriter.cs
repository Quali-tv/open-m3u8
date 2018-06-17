using System;
using System.IO;
using System.Text;

namespace M3U8Parser
{
    public class M3uWriter : Writer
    {
        public M3uWriter(Stream outputStream, Encoding encoding) : base(outputStream, encoding) { }

        public override void doWrite(Playlist playlist)
        {
            throw new InvalidOperationException();
        }
    }
}
