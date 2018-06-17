using System;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    public class MasterPlaylistParserTest
    {
        [Fact]
        public void test()
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
