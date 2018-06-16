using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
    // import com.iheartradio.m3u8.data.MediaData;
    // import com.iheartradio.m3u8.data.MediaType;
    // import com.iheartradio.m3u8.data.Resolution;
    // import com.iheartradio.m3u8.data.StreamInfo;

    // import org.junit.Test;

    // import java.util.ArrayList;
    // import java.util.Arrays;
    // import java.util.List;

namespace M3U8Parser
{
    public class MasterPlaylistLineParserTest : LineParserStateTestCase
    {
        [Fact]
        public void testEXT_X_MEDIA() // throws Exception 
        {
            List<MediaData> expectedMediaData = new List<MediaData>();
            IExtTagParser handler = MasterPlaylistLineParser.EXT_X_MEDIA;
            String tag = Constants.EXT_X_MEDIA_TAG;
            String groupId = "1234";
            String language = "lang";
            String associatedLanguage = "assoc-lang";
            String name = "Foo";
            String inStreamId = "SERVICE1";

            expectedMediaData.Add(new MediaData.Builder()
                    .withType(MediaType.CLOSED_CAPTIONS)
                    .withGroupId(groupId)
                    .withLanguage(language)
                    .withAssociatedLanguage(associatedLanguage)
                    .withName(name)
                    .withAutoSelect(true)
                    .withInStreamId(inStreamId)
                    .withCharacteristics(new List<string>() { "char1", "char2" })
                    .build());

            String line = "#" + tag +
                    ":TYPE=CLOSED-CAPTIONS" +
                    ",GROUP-ID=\"" + groupId + "\"" +
                    ",LANGUAGE=\"" + language + "\"" +
                    ",ASSOC-LANGUAGE=\"" + associatedLanguage + "\"" +
                    ",NAME=\"" + name + "\"" +
                    ",DEFAULT=NO" +
                    ",AUTOSELECT=YES" +
                    ",INSTREAM-ID=\"" + inStreamId + "\"" +
                    ",CHARACTERISTICS=\"char1,char2\"";

            Assert.Equal(tag, handler.getTag());

            handler.parse(line, mParseState);
            var actualMediaData = mParseState.getMaster().mediaData;
            Assert.Equal(expectedMediaData, actualMediaData);
        }

        [Fact]
        public void testEXT_X_STREAM_INF() // throws Exception 
        {
            IExtTagParser handler = MasterPlaylistLineParser.EXT_X_STREAM_INF;
            String tag = Constants.EXT_X_STREAM_INF_TAG;
            int bandwidth = 10000;
            int averageBandwidth = 5000;
            List<String> codecs = new List<String>() { "h.263", "h.264" };
            Resolution resolution = new Resolution(800, 600);
            String audio = "foo";
            String video = "bar";
            String subtitles = "titles";
            String closedCaptions = "captions";

            StreamInfo expectedStreamInfo = new StreamInfo.Builder()
                    .withBandwidth(bandwidth)
                    .withAverageBandwidth(averageBandwidth)
                    .withCodecs(codecs)
                    .withResolution(resolution)
                    .withAudio(audio)
                    .withVideo(video)
                    .withSubtitles(subtitles)
                    .withClosedCaptions(closedCaptions)
                    .build();

            String line = "#" + tag +
                    ":BANDWIDTH=" + bandwidth +
                    ",AVERAGE-BANDWIDTH=" + averageBandwidth +
                    ",CODECS=\"" + codecs[0] + "," + codecs[1] + "\"" +
                    ",RESOLUTION=" + resolution.width + "x" + resolution.height +
                    ",AUDIO=\"" + audio + "\"" +
                    ",VIDEO=\"" + video + "\"" +
                    ",SUBTITLES=\"" + subtitles + "\"" +
                    ",CLOSED-CAPTIONS=\"" + closedCaptions + "\"";

            Assert.Equal(tag, handler.getTag());

            handler.parse(line, mParseState);
            Assert.Equal(expectedStreamInfo, mParseState.getMaster().streamInfo);
        }
    }
}
