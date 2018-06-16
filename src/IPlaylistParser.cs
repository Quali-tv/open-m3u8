using System;
using System.Text;
//import com.iheartradio.m3u8.data.Playlist;
//import java.io.IOException;

namespace M3U8Parser
{
    interface IPlaylistParser
    {
        Playlist parse(); // throws IOException, ParseException, PlaylistException
        bool isAvailable();
    }
}
