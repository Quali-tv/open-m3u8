using System;
using System.IO;
using System.Text;

namespace M3U8Parser
{
    public class TagWriter
    {
        private readonly StreamWriter mWriter;

        public TagWriter(StreamWriter outputStreamWriter)
        {
            mWriter = outputStreamWriter;
        }

        public void write(String str)
        {
            mWriter.Write(str);
        }

        public void writeLine(String line)
        {
            write(line + Constants.WRITE_NEW_LINE);
        }

        public void writeTag(String tag)
        {
            writeLine(Constants.COMMENT_PREFIX + tag);
        }

        public void writeTag(String tag, String value)
        {
            writeLine(Constants.COMMENT_PREFIX + tag + Constants.EXT_TAG_END + value);
        }

        public void flush()
        {
            mWriter.Flush();
        }
    }
}
