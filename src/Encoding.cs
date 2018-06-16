using System.Globalization;
using System;
using System.Collections.Generic;
using System.Text;
//import java.util.HashMap;
//import java.util.Locale;
//import java.util.Map;

namespace M3U8Parser
{
    public class Encoding
    {
        public static readonly Encoding UTF_8;
        public static readonly Encoding WINDOWS_1252;

        private static readonly Dictionary<System.Text.Encoding, Encoding> sMap = new Dictionary<System.Text.Encoding, Encoding>();

        public readonly System.Text.Encoding value;
        public readonly bool supportsByteOrderMark;

        static Encoding()
        {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            UTF_8 = new Encoding(System.Text.Encoding.UTF8, true);
            WINDOWS_1252 = new Encoding(System.Text.Encoding.GetEncoding(1252), false);

            sMap.Add(UTF_8.value, UTF_8);
            sMap.Add(WINDOWS_1252.value, WINDOWS_1252);
        }

        private Encoding(System.Text.Encoding value, bool supportsByteOrderMark)
        {
            this.value = value;
            this.supportsByteOrderMark = supportsByteOrderMark;
        }

        /**
         * @return the encoding for the given value if supported, if the encoding is unsupported or null, null will be returned
         */
        public static Encoding fromValue(System.Text.Encoding value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return sMap[value];
            }
        }

        public System.Text.Encoding getValue()
        {
            return value;
        }
    }
}
