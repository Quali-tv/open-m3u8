using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    public class M3uParserTest
    {
        [Fact]
        public void testParse() // throws Exception 
        {
            String absolute = "http://www.my.song/file1.mp3";
            String relative = "user1/file2.mp3";

            String validData =
                            "#some comment\n" +
                            absolute + "\n" +
                            "\n" +
                            relative + "\n" +
                            "\n";

            List<TrackData> expectedTracks = new List<TrackData>() {
                    new TrackData.Builder().withUri(absolute).build(),
                    new TrackData.Builder().withUri(relative).build()
            };

            Stream inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(validData));
            MediaPlaylist mediaPlaylist = new M3uParser(inputStream, Encoding.UTF_8).parse().getMediaPlaylist();

            Assert.Equal(expectedTracks, mediaPlaylist.getTracks());
        }
    }
}
