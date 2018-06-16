using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
//import com.iheartradio.m3u8.data.ByteRange;
//import com.iheartradio.m3u8.data.Resolution;
//
//import java.io.UnsupportedEncodingException;
//import java.net.URLDecoder;
//import java.util.*;
//import java.util.regex.Matcher;
//import java.util.regex.Pattern;

namespace M3U8Parser
{
    public sealed class ParseUtil
    {
        public static int parseInt(String str, String tag = null) //throws ParseException 
        {
            try
            {
                return Int32.Parse(str);
            }
            catch (FormatException exception)
            {
                throw ParseException.create(ParseExceptionType.NOT_JAVA_INTEGER, tag, str);
            }
        }

        // THIS WAS ONLY USED IN ONE PLACE. WORKED AROUND IT INSTEAD...
        //public static <T extends Enum<T>> T parseEnum(String str, Class<T> enumType, String tag = null) // throws ParseException 
        //{
        //    try
        //    {
        //        return Enum.valueOf(enumType, str);
        //    }
        //    catch (IllegalArgumentException exception)
        //    {
        //        throw ParseException.create(ParseExceptionType.NOT_JAVA_ENUM, tag, str);
        //    }
        //}

        public static String parseDateTime(String str, String tag = null) // throws ParseException 
        {
            Match match = Constants.EXT_X_PROGRAM_DATE_TIME_PATTERN.Match(str);

            if (!match.Success)
            {
                throw new ParseException(ParseExceptionType.INVALID_DATE_TIME_FORMAT, tag);
            }

            return match.Groups[1].Value;
        }

        public static float parseFloat(String str, String tag = null) // throws ParseException 
        {
            try
            {
                return float.Parse(str);
            }
            catch (FormatException exception)
            {
                throw ParseException.create(ParseExceptionType.NOT_JAVA_FLOAT, tag, str);
            }
        }

        public static List<Byte> parseHexadecimal(String hexString, String tag = null) // throws ParseException 
        {
            List<Byte> bytes = new List<Byte>();
            Match match = Constants.HEXADECIMAL_PATTERN.Match(hexString.ToUpper(CultureInfo.CurrentCulture));

            if (match.Success)
            {
                String valueString = match.Groups[1].Value;
                if (valueString.Length % 2 != 0)
                {
                    throw ParseException.create(ParseExceptionType.INVALID_HEXADECIMAL_STRING, tag, hexString);
                }

                for (int i = 0; i < valueString.Length; i += 2)
                {
                    bytes.Add((byte)(Convert.ToInt16(valueString.Substring(i, 2), 16) & 0xFF));
                }

                return bytes;
            }
            else
            {
                throw ParseException.create(ParseExceptionType.INVALID_HEXADECIMAL_STRING, tag, hexString);
            }
        }

        private static byte hexCharToByte(char hex)
        {
            if (hex >= 'A')
            {
                return (byte)((hex & 0xF) + 9);
            }
            else
            {
                return (byte)(hex & 0xF);
            }
        }

        public static bool parseYesNo(Attribute attribute, String tag = null) // throws ParseException 
        {
            if (attribute.value.Equals(Constants.YES))
            {
                return true;
            }
            else if (attribute.value.Equals(Constants.NO))
            {
                return false;
            }
            else
            {
                throw ParseException.create(ParseExceptionType.NOT_YES_OR_NO, tag, attribute.ToString());
            }
        }

        public static Resolution parseResolution(String resolutionString, String tag = null) // throws ParseException 
        {
            Match match = Constants.RESOLUTION_PATTERN.Match(resolutionString);

            if (!match.Success)
            {
                throw new ParseException(ParseExceptionType.INVALID_RESOLUTION_FORMAT, tag);
            }

            return new Resolution(parseInt(match.Groups[1].Value, tag), parseInt(match.Groups[2].Value, tag));
        }

        public static String parseQuotedString(String quotedString, String tag = null) // throws ParseException 
        {
            StringBuilder builder = new StringBuilder();

            bool isEscaping = false;
            int quotesFound = 0;

            for (int i = 0; i < quotedString.Length; ++i)
            {
                char c = quotedString[i];

                if (i == 0 && c != '"')
                {
                    if (isWhitespace(c))
                    {
                        throw new ParseException(ParseExceptionType.ILLEGAL_WHITESPACE, tag);
                    }
                    else
                    {
                        throw new ParseException(ParseExceptionType.INVALID_QUOTED_STRING, tag);
                    }
                }
                else if (quotesFound == 2)
                {
                    if (isWhitespace(c))
                    {
                        throw new ParseException(ParseExceptionType.ILLEGAL_WHITESPACE, tag);
                    }
                    else
                    {
                        throw new ParseException(ParseExceptionType.INVALID_QUOTED_STRING, tag);
                    }
                }
                else if (i == quotedString.Length - 1)
                {
                    if (c != '"' || isEscaping)
                    {
                        throw new ParseException(ParseExceptionType.UNCLOSED_QUOTED_STRING, tag);
                    }
                }
                else
                {
                    if (isEscaping)
                    {
                        builder.Append(c);
                        isEscaping = false;
                    }
                    else
                    {
                        if (c == '\\')
                        {
                            isEscaping = true;
                        }
                        else if (c == '"')
                        {
                            ++quotesFound;
                        }
                        else
                        {
                            builder.Append(c);
                        }
                    }
                }
            }

            return builder.ToString();
        }

        public static ByteRange matchByteRange(Match match)
        {
            long subRangeLength = long.Parse(match.Groups[1].Value);
            long? offset = !string.IsNullOrEmpty(match.Groups[2].Value) ? long.Parse(match.Groups[2].Value) : (long?)null;
            return new ByteRange(subRangeLength, offset);
        }

        public static bool isWhitespace(char c)
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }

        public static String decodeUri(String encodedUri, Encoding encoding) // throws ParseException 
        {
            try
            {
                return System.Net.WebUtility.UrlDecode(encodedUri.Replace("+", "%2B"));
            }
            catch (Exception exception) // TODO: Was UnsupportedEncodingException; c# equivalent?
            {
                throw new ParseException(ParseExceptionType.INTERNAL_ERROR);
            }
        }

        public static Match match(Regex pattern, String line, String tag = null) // throws ParseException 
        {
            Match match = pattern.Match(line);

            if (!match.Success)
            {
                throw ParseException.create(ParseExceptionType.BAD_EXT_TAG_FORMAT, tag, line);
            }

            return match;
        }

        public static void parseAttributes<T>(String line, T builder, ParseState state, Dictionary<String, AttributeParser<T>> handlers, String tag) // throws ParseException 
        {
            foreach (Attribute attribute in parseAttributeList(line, tag))
            {
                if (handlers.ContainsKey(attribute.name))
                {
                    try
                    {
                        handlers[attribute.name].parse(attribute, builder, state);
                    }
                    catch(ParseException ex)
                    {
                        throw ParseException.create(ex.type, tag, ex.getMessageSuffix());
                    }
                }
                else
                {
                    throw ParseException.create(ParseExceptionType.INVALID_ATTRIBUTE_NAME, tag, line);
                }
            }
        }

        public static List<Attribute> parseAttributeList(String line, String tag) // throws ParseException 
        {
            List<Attribute> attributes = new List<Attribute>();
            HashSet<String> attributeNames = new HashSet<String>();

            foreach (String str in splitAttributeList(line, tag))
            {
                int separator = str.IndexOf(Constants.ATTRIBUTE_SEPARATOR);
                int quote = str.IndexOf("\"");

                if (separator == -1 || (quote != -1 && quote < separator))
                {
                    throw ParseException.create(ParseExceptionType.MISSING_ATTRIBUTE_SEPARATOR, tag, attributes.ToString());
                }
                else
                {
                    //Even Apple playlists have sometimes spaces after a ,
                    String name = str.Substring(0, separator).Trim();
                    String value = str.Substring(separator + 1);

                    if (name.isEmpty())
                    {
                        throw ParseException.create(ParseExceptionType.MISSING_ATTRIBUTE_NAME, tag, attributes.ToString());
                    }

                    if (value.isEmpty())
                    {
                        throw ParseException.create(ParseExceptionType.MISSING_ATTRIBUTE_VALUE, tag, attributes.ToString());
                    }

                    if (!attributeNames.Add(name))
                    {
                        throw ParseException.create(ParseExceptionType.MULTIPLE_ATTRIBUTE_NAME_INSTANCES, tag, attributes.ToString());
                    }

                    attributes.Add(new Attribute(name, value));
                }
            }

            return attributes;
        }

        public static List<String> splitAttributeList(String line, String tag) // throws ParseException 
        {
            List<int> splitIndices = new List<int>();
            List<String> attributes = new List<String>();

            int startIndex = line.IndexOf(Constants.EXT_TAG_END) + 1;
            bool isQuotedString = false;
            bool isEscaping = false;

            for (int i = startIndex; i < line.Length; i++)
            {
                if (isQuotedString)
                {
                    if (isEscaping)
                    {
                        isEscaping = false;
                    }
                    else
                    {
                        char c = line[i];

                        if (c == '\\')
                        {
                            isEscaping = true;
                        }
                        else if (c == '"')
                        {
                            isQuotedString = false;
                        }
                    }
                }
                else
                {
                    char c = line[i];

                    if (c == Constants.COMMA_CHAR)
                    {
                        splitIndices.Add(i);
                    }
                    else if (c == '"')
                    {
                        isQuotedString = true;
                    }
                }
            }

            if (isQuotedString)
            {
                throw new ParseException(ParseExceptionType.UNCLOSED_QUOTED_STRING, tag);
            }

            foreach (int splitIndex in splitIndices)
            {
                attributes.Add(line.Substring(startIndex, splitIndex - startIndex));
                startIndex = splitIndex + 1;
            }

            attributes.Add(line.Substring(startIndex));
            return attributes;
        }
    }
}
