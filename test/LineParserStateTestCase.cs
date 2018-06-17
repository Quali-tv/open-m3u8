using System;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    public class LineParserStateTestCase // : TestCase // TODO: <--- JUnit stuff
    {
        protected ParseState mParseState //; // Replaced setUp method below... should be fine I think
            = new ParseState(Encoding.UTF_8);

        // JUnit specific setUp method... should be fine done as above... I think
        //protected override void setUp()
        //{
        //    mParseState = new ParseState(Encoding.UTF_8);
        //}

        protected void assertParseThrows(IExtTagParser handler, String line, ParseExceptionType exceptionType)
        {
            try
            {
                handler.parse(line, mParseState);
                Assert.False(true);
            }
            catch (ParseException exception)
            {
                Assert.Equal(exceptionType, exception.type);
            }
        }

        //[Fact]
        //public void test()
        //{
        //    // workaround for no tests found warning
        //}
    }
}
