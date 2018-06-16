using System.Globalization;
using System;
using System.Collections.Generic;
using System.Text;
//import java.util.HashMap;
//import java.util.Locale;
//import java.util.Map;

namespace M3U8Parser
{
    public class Extension
    {
        public static readonly Extension M3U = new Extension("m3u", Encoding.WINDOWS_1252);
        public static readonly Extension M3U8 = new Extension("m3u8", Encoding.UTF_8);

        private static readonly Dictionary<String, Extension> sMap = new Dictionary<String, Extension>();

        static Extension()
        {
            sMap.Add(M3U.value, M3U);
            sMap.Add(M3U8.value, M3U8);
        }

        public readonly String value;
        public readonly Encoding encoding;

        private Extension(String value, Encoding encoding)
        {
            this.value = value;
            this.encoding = encoding;
        }

        /**
         * @return the extension for the given value if supported, if the extension is unsupported or null, null will be returned
         */
        public static Extension fromValue(String value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return sMap[value.ToLower(CultureInfo.CurrentCulture)];
            }
        }

        public String getValue()
        {
            return value;
        }

        public Encoding getEncoding()
        {
            return encoding;
        }
    }
}