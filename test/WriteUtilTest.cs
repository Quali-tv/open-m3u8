using System;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    public class WriteUtilTest
    {
        [Fact]
        public void writeQuotedStringShouldIgnoreNullTagValueForOptionalFields()
        {
            String outputString = WriteUtil.writeQuotedString(null, true, "some-key");

            //Assert.That(outputString,is ("\"\""));
            Assert.Equal("\"\"", outputString);
        }

        [Fact]
        public void writeQuotedStringShouldNotIgnoreNullTagValue()
        {
            Assert.Throws<NullReferenceException>(() =>
                WriteUtil.writeQuotedString(null, "some-key"));
        }

        [Fact]
        public void writeQuotedStringShouldNotIgnoreSuppliedOptionalValue()
        {
            //Assert.That(WriteUtil.writeQuotedString("blah", "some-key"),is ("\"blah\""));
            Assert.Equal("\"blah\"", WriteUtil.writeQuotedString("blah", "some-key"));
        }
    }
}
