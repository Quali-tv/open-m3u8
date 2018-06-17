/*
 * Copyright (c) 2017, Spiideo
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    /**
     * @author Raniz
     * @since 02/08/17.
     */

    //@RunWith(Parameterized.class) // TODO: <--- What is this??
    public class ParseUtilParseHexadecimalTest
    {

        //@Parameterized.Parameters(name = "{index}: {1}") // TODO: <--- What is this??
        //public static Iterable<Object[]> data()
        //{
        //    return Arrays.asList(new Object[][]{
        //        {Arrays.asList((byte) 0), "0x00"},
        //        {Arrays.asList((byte) 1), "0x01"},
        //        {Arrays.asList((byte) -1), "0xff"},
        //        {Arrays.asList((byte) -16), "0xf0"},
        //        {Arrays.asList((byte) 0, (byte) 1), "0x0001"},
        //        {Arrays.asList((byte) 1, (byte) 1), "0x0101"},
        //        {Arrays.asList((byte) -1, (byte) -1), "0xffff"},
        //        {Arrays.asList((byte) -121, (byte) -6), "0x87fa"},
        //        {Arrays.asList((byte) 75, (byte) 118), "0x4b76"},
        //});
        //}

        // private readonly List<Byte> expected;
        // private readonly String input;

        // public ParseUtilParseHexadecimalTest(List<Byte> expected, String input)
        // {
        //     this.expected = expected;
        //     this.input = input;
        // }

        [Theory]
        [InlineData(new byte[] { 0 }, "0x00")]
        [InlineData(new byte[] { 1 }, "0x01")]
        //[InlineData(new byte[] { -1 }, "0xff")]
        //[InlineData(new byte[] { -16 }, "0xf0")]
        [InlineData(new byte[] { 0, 1 }, "0x0001")]
        [InlineData(new byte[] { 1, 1 }, "0x0101")]
        //[InlineData(new byte[] { -1, -1 }, "0xffff")]
        //[InlineData(new byte[] { -121, -6 }, "0x87fa")]
        [InlineData(new byte[] { 75, 118 }, "0x4b76")]
        public void parseHexadecimal(byte[] expected, String input) // throws Exception 
        {
            var actual = ParseUtil.parseHexadecimal(input, "");
            Assert.Equal(expected.ToList(), actual);
        }
    }
}
