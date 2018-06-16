using System;
using System.Text;
using Xunit;
// import com.iheartradio.m3u8.data.MasterPlaylist;
// import com.iheartradio.m3u8.data.Playlist;
// import org.junit.Test;

// import static org.junit.Assert.assertEquals;
// import static org.junit.Assert.assertFalse;
// import static org.junit.Assert.assertTrue;

namespace M3U8Parser
{
    public class MasterPlaylistParserTest
    {
        [Fact]
        public void test() // throws Exception 
        {
            Playlist playlist = TestUtil.parsePlaylistFromResource("masterPlaylist.m3u8");
            MasterPlaylist masterPlaylist = playlist.getMasterPlaylist();

            Assert.True(playlist.hasMasterPlaylist());
            Assert.False(playlist.hasMediaPlaylist());

            Assert.True(masterPlaylist.hasStartData());
            Assert.Equal(4.5, masterPlaylist.getStartData().getTimeOffset(), 12);
            Assert.False(masterPlaylist.getStartData().isPrecise());
        }
    }
}
