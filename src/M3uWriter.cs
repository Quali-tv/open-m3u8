using System;
using System.IO;
using System.Text;
//import java.io.IOException;
//import java.io.OutputStream;
//
//import com.iheartradio.m3u8.data.Playlist;

namespace M3U8Parser
{
    public class M3uWriter : Writer
    {
        public M3uWriter(Stream outputStream, Encoding encoding) : base(outputStream, encoding) { }

        public override void doWrite(Playlist playlist) //throws IOException 
        {
            throw new InvalidOperationException();
        }
    }
}
