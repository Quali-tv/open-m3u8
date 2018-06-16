using System;
using System.IO;
using System.Text;
//import java.io.IOException;
//import java.io.OutputStream;
//import java.io.OutputStreamWriter;
//import java.io.UnsupportedEncodingException;
//
//import com.iheartradio.m3u8.data.Playlist;

namespace M3U8Parser
{
    public abstract class Writer
    {
        protected readonly TagWriter tagWriter;

        public Writer(Stream outputStream, Encoding encoding)
        {
            try
            {
                tagWriter = new TagWriter(new StreamWriter(outputStream, encoding.getValue()));
            }
            catch (Exception e) // TODO: Was UnsupportedEncodingException. c# equivalent?
            {
                throw new ArgumentException(e.Message, e);
            }
        }

        public void writeTagLine(String tag) //throws IOException 
        {
            writeLine(Constants.COMMENT_PREFIX + tag);
        }

        public void writeTagLine(String tag, Object value) //throws IOException 
        {
            writeLine(Constants.COMMENT_PREFIX + tag + Constants.EXT_TAG_END + value);
        }

        public void writeLine(String line) //throws IOException 
        {
            tagWriter.write(line);
            tagWriter.write("\n");
        }

        public void write(Playlist playlist) //throws IOException, ParseException, PlaylistException 
        {
            doWrite(playlist);

            tagWriter.flush();
        }

        public abstract void doWrite(Playlist playlist); // throws IOException, ParseException, PlaylistException;
    }
}
