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

        public void write(String str) // throws IOException 
        {
            mWriter.Write(str);
        }

        public void writeLine(String line) // throws IOException 
        {
            write(line + Constants.WRITE_NEW_LINE);
        }

        public void writeTag(String tag) // throws IOException 
        {
            writeLine(Constants.COMMENT_PREFIX + tag);
        }

        public void writeTag(String tag, String value) // throws IOException 
        {
            writeLine(Constants.COMMENT_PREFIX + tag + Constants.EXT_TAG_END + value);
        }

        public void flush() // throws IOException 
        {
            mWriter.Flush();
        }
    }
}
