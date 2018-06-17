using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class WriteUtil
    {
        public static String writeYesNo(bool yes)
        {
            if (yes)
            {
                return Constants.YES;
            }
            else
            {
                return Constants.NO;
            }
        }

        public static String writeHexadecimal(List<Byte> hex)
        {
            if (hex == null || hex.Count == 0)
            {
                throw new ArgumentNullException("hex must not be null or empty!");
            }

            String prefix = "0x";
            StringBuilder builder = new StringBuilder(hex.Count + prefix.Length);
            builder.Append(prefix);
            foreach (Byte b in hex)
            {
                builder.Append(b.ToString("x2")); //String.Format("%02x", b));
            }
            return builder.ToString();
        }

        public static String writeResolution(Resolution r)
        {
            return r.width + "x" + r.height;
        }

        public static String writeQuotedString(String unquotedString, String tag = null)
        {
            return writeQuotedString(unquotedString, false, tag);
        }
        
        public static String writeQuotedString(String unquotedString, bool optional, String tag = null)
        {
            if (unquotedString != null || !optional)
            {
                StringBuilder builder = new StringBuilder(unquotedString.Length + 2);
                builder.Append("\"");

                for (int i = 0; i < unquotedString.Length; ++i)
                {
                    char c = unquotedString[i];

                    if (i == 0 && ParseUtil.isWhitespace(c))
                    {
                        throw new ParseException(ParseExceptionType.ILLEGAL_WHITESPACE, tag);
                    }
                    else if (c == '"')
                    {
                        builder.Append('\\').Append(c);
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }

                builder.Append("\"");
                return builder.ToString();
            }

            return "\"\"";
        }

        public static String encodeUri(String decodedUri)
        {
            try
            {
                return System.Web.HttpUtility.UrlEncode(decodedUri.Replace("%2B", "+"), System.Text.Encoding.UTF8);
            }
            catch (Exception) // TODO: Was UnsupportedEncodingException. c# equivalent?
            {
                throw new ParseException(ParseExceptionType.INTERNAL_ERROR);
            }
        }

        public static String join<T>(List<T> valueList, String separator)
        {
            if (valueList == null || valueList.Count == 0)
            {
                throw new ArgumentNullException("valueList must not be null or empty!");
            }
            if (separator == null)
            {
                throw new ArgumentNullException("separator must not be null!");
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueList.Count; i++)
            {
                sb.Append(valueList[i].ToString());
                if (i + 1 < valueList.Count)
                {
                    sb.Append(separator);
                }
            }
            return sb.ToString();
        }
    }
}
