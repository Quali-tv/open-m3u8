using System;
using System.IO;
using System.Text;

namespace M3U8Parser
{
    public abstract class BaseM3uParser : IPlaylistParser
    {
        protected readonly M3uScanner mScanner;
        protected readonly Encoding mEncoding;

        public BaseM3uParser(Stream inputStream, Encoding encoding)
        {
            mScanner = new M3uScanner(inputStream, encoding);
            mEncoding = encoding;
        }

        public virtual bool isAvailable()
        {
            return mScanner.hasNext();
        }

        public abstract Playlist parse();

        protected void validateAvailable()
        {
            if (!isAvailable())
            {
                throw new EndOfStreamException();
            }
        }
    }
}
