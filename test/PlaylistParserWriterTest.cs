using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

namespace M3U8Parser
{

    // import com.iheartradio.m3u8.data.*;
    // import org.junit.Test;

    // import java.io.ByteArrayOutputStream;
    // import java.io.FileInputStream;
    // import java.io.IOException;
    // import java.io.InputStream;
    // import java.util.ArrayList;
    // import java.util.Arrays;
    // import java.util.List;

    // import static org.junit.Assert.*;

    public class PlaylistParserWriterTest
    {
        Playlist readPlaylist(String fileName) // throws IOException, ParseException, PlaylistException 
        {
            Assert.NotNull(fileName);

            using (Stream ins = new FileStream("resources/" + fileName, FileMode.Open))
            {
                Playlist playlist = new PlaylistParser(ins, Format.EXT_M3U, Encoding.UTF_8).parse();
                return playlist;
            }
        }

        String writePlaylist(Playlist playlist) // throws IOException, ParseException, PlaylistException 
        {
            Assert.NotNull(playlist);

            using (MemoryStream os = new MemoryStream())
            {
                PlaylistWriter writer = new PlaylistWriter(os, Format.EXT_M3U, Encoding.UTF_8);
                writer.write(playlist);

                // return os.ToString(Encoding.UTF_8.value);
                os.Position = 0;
                using (var sr = new StreamReader(os, System.Text.Encoding.UTF8))
                    return sr.ReadToEnd();
            }
        }

        [Fact]
        public void simpleMediaPlaylist() // throws IOException, ParseException, PlaylistException 
        {
            Playlist playlist = readPlaylist("simpleMediaPlaylist.m3u8");

            String sPlaylist = writePlaylist(playlist);

            Debug.WriteLine(sPlaylist);
        }

        [Fact]
        public void liveMediaPlaylist() // throws IOException, ParseException, PlaylistException 
        {
            Playlist playlist = readPlaylist("liveMediaPlaylist.m3u8");

            String sPlaylist = writePlaylist(playlist);

            Debug.WriteLine(sPlaylist);
        }

        [Fact]
        public void playlistWithEncryptedMediaSegments() // throws IOException, ParseException, PlaylistException 
        {
            Playlist playlist = readPlaylist("playlistWithEncryptedMediaSegments.m3u8");

            String sPlaylist = writePlaylist(playlist);

            Debug.WriteLine(sPlaylist);
        }

        [Fact]
        public void masterPlaylist() // throws IOException, ParseException, PlaylistException 
        {
            Playlist playlist = readPlaylist("masterPlaylist.m3u8");

            String sPlaylist = writePlaylist(playlist);

            Debug.WriteLine(sPlaylist);
        }

        [Fact]
        public void masterPlaylistWithIFrames() // throws IOException, ParseException, PlaylistException 
        {
            Playlist playlist = readPlaylist("masterPlaylistWithIFrames.m3u8");
            Assert.True(playlist.hasMasterPlaylist());

            MasterPlaylist masterPlaylist = playlist.getMasterPlaylist();
            Assert.NotNull(masterPlaylist);

            List<PlaylistData> playlistDatas = masterPlaylist.getPlaylists();
            List<IFrameStreamInfo> iFrameInfo = masterPlaylist.getIFramePlaylists();
            Assert.NotNull(playlistDatas);
            Assert.NotNull(iFrameInfo);
            Assert.Equal(4, playlistDatas.Count);
            Assert.Equal(3, iFrameInfo.Count);

            PlaylistData lowXStreamInf = playlistDatas[0];
            Assert.NotNull(lowXStreamInf);
            Assert.NotNull(lowXStreamInf.getStreamInfo());
            Assert.Equal(1280000, lowXStreamInf.getStreamInfo().getBandwidth());
            Assert.Equal("low/audio-video.m3u8", lowXStreamInf.getUri());

            PlaylistData midXStreamInf = playlistDatas[1];
            Assert.NotNull(midXStreamInf);
            Assert.NotNull(midXStreamInf.getStreamInfo());
            Assert.Equal(2560000, midXStreamInf.getStreamInfo().getBandwidth());
            Assert.Equal("mid/audio-video.m3u8", midXStreamInf.getUri());

            PlaylistData hiXStreamInf = playlistDatas[2];
            Assert.NotNull(hiXStreamInf);
            Assert.NotNull(hiXStreamInf.getStreamInfo());
            Assert.Equal(7680000, hiXStreamInf.getStreamInfo().getBandwidth());
            Assert.Equal("hi/audio-video.m3u8", hiXStreamInf.getUri());

            PlaylistData audioXStreamInf = playlistDatas[3];
            Assert.NotNull(audioXStreamInf);
            Assert.NotNull(audioXStreamInf.getStreamInfo());
            Assert.Equal(65000, audioXStreamInf.getStreamInfo().getBandwidth());
            Assert.NotNull(audioXStreamInf.getStreamInfo().getCodecs());
            Assert.Equal(1, audioXStreamInf.getStreamInfo().getCodecs().Count);
            Assert.Equal("mp4a.40.5", audioXStreamInf.getStreamInfo().getCodecs()[0]);
            Assert.Equal("audio-only.m3u8", audioXStreamInf.getUri());

            IFrameStreamInfo lowXIFrameStreamInf = iFrameInfo[0];
            Assert.NotNull(lowXIFrameStreamInf);
            Assert.Equal(86000, lowXIFrameStreamInf.getBandwidth());
            Assert.Equal("low/iframe.m3u8", lowXIFrameStreamInf.getUri());

            IFrameStreamInfo midXIFrameStreamInf = iFrameInfo[1];
            Assert.NotNull(midXIFrameStreamInf);
            Assert.Equal(150000, midXIFrameStreamInf.getBandwidth());
            Assert.Equal("mid/iframe.m3u8", midXIFrameStreamInf.getUri());

            IFrameStreamInfo hiXIFrameStreamInf = iFrameInfo[2];
            Assert.NotNull(hiXIFrameStreamInf);
            Assert.Equal(550000, hiXIFrameStreamInf.getBandwidth());
            Assert.Equal("hi/iframe.m3u8", hiXIFrameStreamInf.getUri());

            String writtenPlaylist = writePlaylist(playlist);
            Assert.Equal(
                "#EXTM3U\n" +
                "#EXT-X-VERSION:1\n" +
                "#EXT-X-STREAM-INF:BANDWIDTH=1280000\n" +
                "low/audio-video.m3u8\n" +
                "#EXT-X-STREAM-INF:BANDWIDTH=2560000\n" +
                "mid/audio-video.m3u8\n" +
                "#EXT-X-STREAM-INF:BANDWIDTH=7680000\n" +
                "hi/audio-video.m3u8\n" +
                "#EXT-X-STREAM-INF:CODECS=\"mp4a.40.5\",BANDWIDTH=65000\n" +
                "audio-only.m3u8\n" +
                "#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=86000,URI=\"low/iframe.m3u8\"\n" +
                "#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=150000,URI=\"mid/iframe.m3u8\"\n" +
                "#EXT-X-I-FRAME-STREAM-INF:BANDWIDTH=550000,URI=\"hi/iframe.m3u8\"\n",
                writtenPlaylist);
        }

        [Fact]
        public void masterPlaylistWithAlternativeAudio() // throws IOException, ParseException, PlaylistException 
        {
            Playlist playlist = readPlaylist("masterPlaylistWithAlternativeAudio.m3u8");

            String sPlaylist = writePlaylist(playlist);

            Debug.WriteLine(sPlaylist);
        }

        [Fact]
        public void masterPlaylistWithAlternativeVideo() // throws IOException, ParseException, PlaylistException 
        {
            Playlist playlist = readPlaylist("masterPlaylistWithAlternativeVideo.m3u8");

            String sPlaylist = writePlaylist(playlist);

            Debug.WriteLine(sPlaylist);
        }

        [Fact]
        public void discontinutyPlaylist() // throws IOException, ParseException, PlaylistException 
        {
            Playlist playlist = readPlaylist("withDiscontinuity.m3u8");
            String sPlaylist = writePlaylist(playlist);
            Debug.WriteLine("***************");
            Debug.WriteLine(sPlaylist);
        }

        [Fact]
        public void playlistWithByteRanges() // throws Exception 
        {
            Playlist playlist = TestUtil.parsePlaylistFromResource("mediaPlaylistWithByteRanges.m3u8");
            MediaPlaylist mediaPlaylist = playlist.getMediaPlaylist();
            List<ByteRange> byteRanges = new List<ByteRange>();
            foreach (TrackData track in mediaPlaylist.getTracks())
            {
                Assert.True(track.hasByteRange());
                byteRanges.Add(track.getByteRange());
            }

            List<ByteRange> expectedRanges = new List<ByteRange>(){
                new ByteRange(0, 10),
                new ByteRange(20),
                new ByteRange(30)
            };

            Assert.Equal(expectedRanges, byteRanges);

            var writtenPlaylist = writePlaylist(playlist);

            Assert.Equal(
                "#EXTM3U\n" +
                "#EXT-X-VERSION:4\n" +
                "#EXT-X-TARGETDURATION:10\n" +
                "#EXT-X-MEDIA-SEQUENCE:0\n" +
                "#EXT-X-BYTERANGE:0@10\n" +
                "#EXTINF:9.009\n" +
                "http://media.example.com/first.ts\n" +
                "#EXT-X-BYTERANGE:20\n" +
                "#EXTINF:9.009\n" +
                "http://media.example.com/first.ts\n" +
                "#EXT-X-BYTERANGE:30\n" +
                "#EXTINF:3.003\n" +
                "http://media.example.com/first.ts\n" +
                "#EXT-X-ENDLIST\n",
                writtenPlaylist);
        }
    }
}
