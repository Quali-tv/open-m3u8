using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace M3U8Parser
{
    public class ExtendedM3uParserTest
    {
        // public ExtendedM3uParserTest()
        // {
        //     System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        // }

        [Fact]
        public void testParseMaster()
        {
            List<MediaData> expectedMediaData = new List<MediaData>();

            expectedMediaData.Add(new MediaData.Builder()
                    .withType(MediaType.AUDIO)
                    .withGroupId("1234")
                    .withName("Foo")
                    .build());

            StreamInfo expectedStreamInfo = new StreamInfo.Builder()
                    .withBandwidth(500)
                    .build();

            String validData =
                    "#EXTM3U\n" +
                            "#EXT-X-VERSION:2\n" +
                            "#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"1234\",NAME=\"Foo\"\n" +
                            "#EXT-X-STREAM-INF:BANDWIDTH=500\n" +
                            "http://foo.bar.com/\n" +
                            "\n";

            Stream inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(validData));
            ExtendedM3uParser parser = new ExtendedM3uParser(inputStream, Encoding.UTF_8, ParsingMode.STRICT);

            Assert.True(parser.isAvailable());

            Playlist playlist = parser.parse();

            Assert.False(parser.isAvailable());
            Assert.True(playlist.isExtended());
            Assert.Equal(2, playlist.getCompatibilityVersion());
            Assert.True(playlist.hasMasterPlaylist());
            Assert.Equal(expectedMediaData, playlist.getMasterPlaylist().getMediaData());
            Assert.Equal(expectedStreamInfo, playlist.getMasterPlaylist().getPlaylists()[0].getStreamInfo());
        }

        [Fact]
        public void testLenientParsing()
        {
            String validData =
                    "#EXTM3U\n" +
                            "#EXT-X-VERSION:2\n" +
                            "#EXT-X-TARGETDURATION:60\n" +
                            "#EXT-X-MEDIA-SEQUENCE:10\n" +
                            "#EXT-FAXS-CM:MIIa4QYJKoZIhvcNAQcCoIIa0jCCGs4C...\n" +
                            "#some comment\n" +
                            "#EXTINF:120.0,title 1\n" +
                            "http://www.my.song/file1.mp3\n" +
                            "\n";

            Stream inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(validData));
            Playlist playlist = new ExtendedM3uParser(inputStream, Encoding.UTF_8, ParsingMode.LENIENT).parse();

            Assert.True(playlist.isExtended());
            Assert.True(playlist.getMediaPlaylist().hasUnknownTags());
            Assert.True(playlist.getMediaPlaylist().getUnknownTags()[0].Length > 0);
        }

        [Fact]
        public void testParseMedia()
        {
            String absolute = "http://www.my.song/file1.mp3";
            String relative = "user1/file2.mp3";

            String validData =
                    "#EXTM3U\n" +
                            "#EXT-X-VERSION:2\n" +
                            "#EXT-X-TARGETDURATION:60\n" +
                            "#EXT-X-MEDIA-SEQUENCE:10\n" +
                            "#some comment\n" +
                            "#EXTINF:120.0,title 1\n" +
                            absolute + "\n" +
                            "#EXTINF:100.0,title 2\n" +
                            "#EXT-X-PROGRAM-DATE-TIME:2010-02-19T14:54:23.031+08:00\n" +
                            "\n" +
                            relative + "\n" +
                            "\n";

            List<TrackData> expectedTracks = new List<TrackData>() {
                new TrackData.Builder().withUri(absolute).withTrackInfo(new TrackInfo(120, "title 1")).build(),
                new TrackData.Builder().withUri(relative).withTrackInfo(new TrackInfo(100, "title 2")).withProgramDateTime("2010-02-19T14:54:23.031+08:00").build()};

            Stream inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(validData));
            Playlist playlist = new ExtendedM3uParser(inputStream, Encoding.UTF_8, ParsingMode.STRICT).parse();

            Assert.True(playlist.isExtended());
            Assert.Equal(2, playlist.getCompatibilityVersion());
            Assert.True(playlist.hasMediaPlaylist());
            Assert.Equal(60, playlist.getMediaPlaylist().getTargetDuration());
            Assert.Equal(10, playlist.getMediaPlaylist().getMediaSequenceNumber());
            Assert.Equal(expectedTracks, playlist.getMediaPlaylist().getTracks());
        }

        [Fact]
        public void testParsingMultiplePlaylists()
        {
            using(Stream inputStream = TestUtil.inputStreamFromResource("twoMediaPlaylists.m3u8"))
            {
                PlaylistParser parser = new PlaylistParser(inputStream, Format.EXT_M3U, Encoding.UTF_8);

                Assert.True(parser.isAvailable());

                Playlist playlist1 = parser.parse();

                Assert.True(parser.isAvailable());

                Playlist playlist2 = parser.parse();

                Assert.False(parser.isAvailable());

                List<TrackData> expected1 = new List<TrackData>() {
                    makeTrackData("http://media.example.com/first.ts", 9.009f),
                    makeTrackData("http://media.example.com/second.ts", 9.009f),
                    makeTrackData("http://media.example.com/third.ts", 3.003f)};

                var actual1 = playlist1.getMediaPlaylist().getTracks();
                Assert.Equal(
                        expected1,
                        actual1);

                Assert.Equal(
                        new List<TrackData>() {
                            makeTrackData("http://media.example.com/fourth.ts", 9.01f),
                            makeTrackData("http://media.example.com/fifth.ts", 9.011f)},
                        playlist2.getMediaPlaylist().getTracks());

                // Assert.Equal(0, inputStream.available()); // TODO: Why? And what's the c# equivalent
            }
        }

        [Fact]
        public void testParseDiscontinuity()
        {
            String absolute = "http://www.my.song/file1.mp3";
            String relative = "user1/file2.mp3";

            String validData =
                    "#EXTM3U\n" +
                            "#EXT-X-VERSION:2\n" +
                            "#EXT-X-TARGETDURATION:60\n" +
                            "#EXT-X-MEDIA-SEQUENCE:10\n" +
                            "#some comment\n" +
                            "#EXTINF:120.0,title 1\n" +
                            absolute + "\n" +
                            "#EXT-X-DISCONTINUITY\n" +
                            "#EXTINF:100.0,title 2\n" +
                            "\n" +
                            relative + "\n" +
                            "\n";

            Stream inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(validData));
            MediaPlaylist mediaPlaylist = new ExtendedM3uParser(inputStream, Encoding.UTF_8, ParsingMode.STRICT).parse().getMediaPlaylist();

            Assert.False(mediaPlaylist.getTracks()[0].hasDiscontinuity());
            Assert.True(mediaPlaylist.getTracks()[1].hasDiscontinuity());
            Assert.Equal(0, mediaPlaylist.getDiscontinuitySequenceNumber(0));
            Assert.Equal(1, mediaPlaylist.getDiscontinuitySequenceNumber(1));
        }

        private static TrackData makeTrackData(String uri, float duration)
        {
            return new TrackData.Builder()
                    .withTrackInfo(new TrackInfo(duration, null))
                    .withUri(uri)
                    .build();
        }
    }
}
