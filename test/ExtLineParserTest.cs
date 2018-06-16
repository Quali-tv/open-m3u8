using System;
using System.Text;
using Xunit;
//import org.junit.Test;

namespace M3U8Parser
{
    public class ExtLineParserTest : LineParserStateTestCase
    {
        [Fact]
        public void testEXTM3U() // throws Exception 
        {
            IExtTagParser handler = ExtLineParser.EXTM3U_HANDLER;
            String tag = Constants.EXTM3U_TAG;
            String line = "#" + tag;

            Assert.Equal(tag, handler.getTag());

            handler.parse(line, mParseState);
            Assert.True(mParseState.isExtended());

            assertParseThrows(handler, line, ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES);
        }

        [Fact]
        public void testEXT_X_VERSION() // throws Exception 
        {
            IExtTagParser handler = ExtLineParser.EXT_X_VERSION_HANDLER;
            String tag = Constants.EXT_X_VERSION_TAG;
            String line = "#" + tag + ":2";

            Assert.Equal(tag, handler.getTag());

            assertParseThrows(handler, line + ".", ParseExceptionType.BAD_EXT_TAG_FORMAT);

            handler.parse(line, mParseState);
            Assert.Equal(2, mParseState.getCompatibilityVersion());

            assertParseThrows(handler, line, ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES);
        }

        [Fact]
        public void testEXT_X_START() // throws Exception 
        {
            IExtTagParser parser = ExtLineParser.EXT_X_START;
            String tag = Constants.EXT_X_START_TAG;
            String line = "#" + tag +
                    ":TIME-OFFSET=-4.5" +
                    ",PRECISE=YES";

            Assert.Equal(tag, parser.getTag());
            assertParseThrows(parser, line + ".", ParseExceptionType.NOT_YES_OR_NO);

            parser.parse(line, mParseState);
            Assert.Equal(-4.5, mParseState.startData.getTimeOffset(), 12);
            Assert.Equal(true, mParseState.startData.isPrecise());

            assertParseThrows(parser, line, ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES);
        }
    }
}
