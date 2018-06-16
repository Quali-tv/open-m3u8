using System;
using System.Text;
// import com.iheartradio.m3u8.data.PlaylistData;

namespace M3U8Parser
{
    class PlaylistLineParser : LineParser
    {
        public void parse(String line, ParseState state)
        {
            PlaylistData.Builder builder = new PlaylistData.Builder();
            MasterParseState masterState = state.getMaster();

            masterState.playlists.Add(builder
                    .withUri(line)
                    .withStreamInfo(masterState.streamInfo)
                    .build());

            masterState.streamInfo = null;
        }
    }
}
