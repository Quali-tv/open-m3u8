using System;
using System.Text;
using Xunit;
// import org.junit.Test;

// import static org.hamcrest.core.Is.is;
// import static org.junit.Assert.*;

namespace M3U8Parser
{
    public class WriteUtilTest
    {
        [Fact]
        public void writeQuotedStringShouldIgnoreNullTagValueForOptionalFields() // throws Exception 
        {
            String outputString = WriteUtil.writeQuotedString(null, true, "some-key");

            //Assert.That(outputString,is ("\"\""));
            Assert.Equal("\"\"", outputString);
        }

        [Fact]
        public void writeQuotedStringShouldNotIgnoreNullTagValue() // throws Exception 
        {
            Assert.Throws<NullReferenceException>(() =>
                WriteUtil.writeQuotedString(null, "some-key"));
        }

        [Fact]
        public void writeQuotedStringShouldNotIgnoreSuppliedOptionalValue() // throws Exception 
        {
            //Assert.That(WriteUtil.writeQuotedString("blah", "some-key"),is ("\"blah\""));
            Assert.Equal("\"blah\"", WriteUtil.writeQuotedString("blah", "some-key"));
        }
    }
}
