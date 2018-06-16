using System;
using System.Text;
// import com.iheartradio.m3u8.data.Playlist;
// import java.io.IOException;

namespace M3U8Parser
{
    public interface SectionWriter
    {
        void write(TagWriter tagWriter, Playlist playlist); // throws IOException, ParseException;
    }
}
