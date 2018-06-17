using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    public class MediaPlaylistLineParserTest : LineParserStateTestCase
    {
        [Fact]
        public void testEXT_X_TARGETDURATION()
        {
            IExtTagParser handler = MediaPlaylistLineParser.EXT_X_TARGETDURATION;
            String tag = Constants.EXT_X_TARGETDURATION_TAG;
            String line = "#" + tag + ":60";

            Assert.Equal(tag, handler.getTag());

            handler.parse(line, mParseState);
            Assert.Equal(60, (int)mParseState.getMedia().targetDuration);

            assertParseThrows(handler, line, ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES);
        }

        [Fact]
        public void testEXTINF()
        {
            IExtTagParser handler = MediaPlaylistLineParser.EXTINF;
            String tag = Constants.EXTINF_TAG;
            String line = "#" + tag + ":-1,TOP 100";

            Assert.Equal(tag, handler.getTag());

            handler.parse(line, mParseState);
            Assert.Equal(-1f, mParseState.getMedia().trackInfo.duration);
            Assert.Equal("TOP 100", mParseState.getMedia().trackInfo.title);
        }

        [Fact]
        public void testEXT_X_KEY()
        {
            IExtTagParser handler = MediaPlaylistLineParser.EXT_X_KEY;
            String tag = Constants.EXT_X_KEY_TAG;
            String uri = "http://foo.bar.com/";
            String format = "format";

            String line = "#" + tag +
                    ":METHOD=AES-128" +
                    ",URI=\"" + uri + "\"" +
                    ",IV=0x1234abcd5678EF90aabbccddeeff0011" +
                    ",KEYFORMAT=\"" + format + "\"" +
                    ",KEYFORMATVERSIONS=\"1/2/3\"";

            Assert.Equal(tag, handler.getTag());

            handler.parse(line, mParseState);
            EncryptionData encryptionData = mParseState.getMedia().encryptionData;
            Assert.Equal(EncryptionMethod.AES, encryptionData.getMethod());
            Assert.Equal(uri, encryptionData.getUri());

            Assert.Equal(
                    new List<byte>(){(byte) 0x12, (byte) 0x34, (byte) 0xAB, (byte) 0xCD,
                                 (byte) 0x56, (byte) 0x78, (byte) 0xEF, (byte) 0x90,
                                 (byte) 0xAA, (byte) 0xBB, (byte) 0xCC, (byte) 0xDD,
                                 (byte) 0xEE, (byte) 0xFF, (byte) 0x00, (byte) 0x11},
                    encryptionData.getInitializationVector());

            Assert.Equal(format, encryptionData.getKeyFormat());
            Assert.Equal(new List<int>() { 1, 2, 3 }, encryptionData.getKeyFormatVersions());
        }

        [Fact]
        public void testEXT_X_MAP()
        {
            IExtTagParser handler = MediaPlaylistLineParser.EXT_X_MAP;
            String tag = Constants.EXT_X_MAP;
            String uri = "init.mp4";
            long subRangeLength = 350;
            long offset = 76L;

            String line = "#" + tag +
                    ":URI=\"" + uri + "\"" +
                    ",BYTERANGE=\"" + subRangeLength + "@" + offset + "\"";

            Assert.Equal(tag, handler.getTag());
            handler.parse(line, mParseState);
            MapInfo mapInfo = mParseState.getMedia().mapInfo;
            Assert.Equal(uri, mapInfo.getUri());
            Assert.NotNull(mapInfo.getByteRange());
            Assert.Equal(subRangeLength, mapInfo.getByteRange().getSubRangeLength());
            Assert.Equal(offset, mapInfo.getByteRange().getOffset());
        }

        [Fact]
        public void testEXT_X_BYTERANGE()
        {
            IExtTagParser handler = MediaPlaylistLineParser.EXT_X_BYTERANGE;
            String tag = Constants.EXT_X_BYTERANGE_TAG;
            long subRangeLength = 350;
            long offset = 70L;

            String line = "#" + tag + ":" + subRangeLength + "@" + offset;

            Assert.Equal(tag, handler.getTag());
            handler.parse(line, mParseState);
            ByteRange byteRange = mParseState.getMedia().byteRange;
            Assert.Equal(subRangeLength, byteRange.getSubRangeLength());
            Assert.Equal(offset, byteRange.getOffset());
        }
    }
}
