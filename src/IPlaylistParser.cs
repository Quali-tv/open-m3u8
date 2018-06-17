using System;
using System.Text;

namespace M3U8Parser
{
    interface IPlaylistParser
    {
        Playlist parse();
        bool isAvailable();
    }
}
