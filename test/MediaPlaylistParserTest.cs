using System;
using System.Text;
using Xunit;
// import com.iheartradio.m3u8.data.MediaPlaylist;
// import com.iheartradio.m3u8.data.Playlist;
// import org.junit.Test;

// import static org.junit.Assert.assertEquals;
// import static org.junit.Assert.assertFalse;
// import static org.junit.Assert.assertTrue;

namespace M3U8Parser
{
    public class MediaPlaylistParserTest
    {
        [Fact]
        public void test() // throws Exception 
        {
            Playlist playlist = TestUtil.parsePlaylistFromResource("mediaPlaylist.m3u8");
            MediaPlaylist mediaPlaylist = playlist.getMediaPlaylist();

            Assert.False(playlist.hasMasterPlaylist());
            Assert.True(playlist.hasMediaPlaylist());
            Assert.True(mediaPlaylist.hasStartData());
            Assert.Equal(-4.5, mediaPlaylist.getStartData().getTimeOffset(), 12);
            Assert.True(mediaPlaylist.getStartData().isPrecise());
            Assert.Equal(10, mediaPlaylist.getTargetDuration());
        }
    }
}
