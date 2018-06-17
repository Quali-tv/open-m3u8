using System;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    public class MediaPlaylistParserTest
    {
        [Fact]
        public void test()
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
