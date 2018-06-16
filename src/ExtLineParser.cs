using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
//import java.util.HashMap;
//import java.util.Map;
//import java.util.regex.Matcher;
//
//import com.iheartradio.m3u8.data.Playlist;
//import com.iheartradio.m3u8.data.StartData;

namespace M3U8Parser
{
    public class ExtLineParser : LineParser
    {
        private readonly IExtTagParser mTagParser;

        public ExtLineParser(IExtTagParser tagParser)
        {
            mTagParser = tagParser;
        }

        public void parse(String line, ParseState state) // throws ParseException
        {
            if (mTagParser.hasData())
            {
                if (line.IndexOf(Constants.EXT_TAG_END) != mTagParser.getTag().Length + 1)
                {
                    throw ParseException.create(ParseExceptionType.MISSING_EXT_TAG_SEPARATOR, mTagParser.getTag(), line);
                }
            }
        }

        public static readonly IExtTagParser EXTM3U_HANDLER = new EXTM3U_HANDLER_CLASS();
        private class EXTM3U_HANDLER_CLASS : IExtTagParser
        {
            public String getTag()
            {
                return Constants.EXTM3U_TAG;
            }

            public bool hasData()
            {
                return false;
            }

            public void parse(String line, ParseState state) //throws ParseException 
            {
                if (state.isExtended())
                {
                    throw ParseException.create(ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES, getTag(), line);
                }

                state.setExtended();
            }
        }

        public static readonly IExtTagParser EXT_UNKNOWN_HANDLER = new EXT_UNKNOWN_HANDLER_CLASS();
        private class EXT_UNKNOWN_HANDLER_CLASS : IExtTagParser
        {
            public String getTag()
            {
                return null;
            }

            public bool hasData()
            {
                return false;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                state.unknownTags.Add(line);
            }
        }

        public static readonly IExtTagParser EXT_X_VERSION_HANDLER = new EXT_X_VERSION_HANDLER_CLASS();
        private class EXT_X_VERSION_HANDLER_CLASS : IExtTagParser
        {
            private readonly ExtLineParser lineParser; public EXT_X_VERSION_HANDLER_CLASS() { lineParser = new ExtLineParser(this); }

            public String getTag()
            {
                return Constants.EXT_X_VERSION_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException
            {
                lineParser.parse(line, state);

                Match match = ParseUtil.match(Constants.EXT_X_VERSION_PATTERN, line, getTag());

                if (state.getCompatibilityVersion() != ParseState.NONE)
                {
                    throw ParseException.create(ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES, getTag(), line);
                }

                int compatibilityVersion = ParseUtil.parseInt(match.Groups[1].Value, getTag());

                if (compatibilityVersion < Playlist.MIN_COMPATIBILITY_VERSION)
                {
                    throw ParseException.create(ParseExceptionType.INVALID_COMPATIBILITY_VERSION, getTag(), line);
                }

                if (compatibilityVersion > Constants.MAX_COMPATIBILITY_VERSION)
                {
                    throw ParseException.create(ParseExceptionType.UNSUPPORTED_COMPATIBILITY_VERSION, getTag(), line);
                }

                state.setCompatibilityVersion(compatibilityVersion);
            }
        }

        public static readonly IExtTagParser EXT_X_START = new EXT_X_START_CLASS();
        private class EXT_X_START_CLASS : IExtTagParser
        {
            private readonly ExtLineParser lineParser;
            private readonly Dictionary<String, AttributeParser<StartData.Builder>> HANDLERS =
            new Dictionary<String, AttributeParser<StartData.Builder>>();

            public EXT_X_START_CLASS()
            {
                lineParser = new ExtLineParser(this);
                HANDLERS.Add(Constants.TIME_OFFSET, new TIME_OFFSET_AttributeParser());
                HANDLERS.Add(Constants.PRECISE, new PRECISE_AttributeParser());
            }

            private class TIME_OFFSET_AttributeParser : AttributeParser<StartData.Builder>
            {
                public void parse(Attribute attribute, StartData.Builder builder, ParseState state) //throws ParseException
                {
                    builder.withTimeOffset(ParseUtil.parseFloat(attribute.value));
                }
            }

            private class PRECISE_AttributeParser : AttributeParser<StartData.Builder>
            {
                public void parse(Attribute attribute, StartData.Builder builder, ParseState state) //throws ParseException
                {
                    builder.withPrecise(ParseUtil.parseYesNo(attribute));
                }
            }


            public String getTag()
            {
                return Constants.EXT_X_START_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException
            {
                if (state.startData != null)
                {
                    throw ParseException.create(ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES, getTag(), line);
                }

                StartData.Builder builder = new StartData.Builder();

                lineParser.parse(line, state);
                ParseUtil.parseAttributes(line, builder, state, HANDLERS, getTag());
                state.startData = builder.build();
            }
        }
    }
}
