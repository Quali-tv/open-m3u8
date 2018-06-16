using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
// import com.iheartradio.m3u8.data.*;
// import com.iheartradio.m3u8.data.EncryptionData.Builder;

// import java.util.ArrayList;
// import java.util.HashMap;
// import java.util.List;
// import java.util.Map;
// import java.util.regex.Matcher;

namespace M3U8Parser
{
    public class MediaPlaylistLineParser : LineParser
    {
        private readonly IExtTagParser tagParser;
        private readonly LineParser lineParser;

        MediaPlaylistLineParser(IExtTagParser parser) :
            this(parser, new ExtLineParser(parser))
        { }

        MediaPlaylistLineParser(IExtTagParser tagParser, LineParser lineParser)
        {
            this.tagParser = tagParser;
            this.lineParser = lineParser;
        }

        public void parse(String line, ParseState state) // throws ParseException 
        {
            if (state.isMaster())
            {
                throw ParseException.create(ParseExceptionType.MEDIA_IN_MASTER, tagParser.getTag());
            }

            state.setMedia();
            lineParser.parse(line, state);
        }

        // media playlist tags

        public static readonly IExtTagParser EXT_X_ENDLIST = new EXT_X_ENDLIST_CLASS();
        private class EXT_X_ENDLIST_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_ENDLIST_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_ENDLIST_TAG;
            }

            public bool hasData()
            {
                return false;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                ParseUtil.match(Constants.EXT_X_ENDLIST_PATTERN, line, getTag());
                state.getMedia().endOfList = true;
            }
        };

        public static readonly IExtTagParser EXT_X_I_FRAMES_ONLY = new EXT_X_I_FRAMES_ONLY_CLASS();
        private class EXT_X_I_FRAMES_ONLY_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_I_FRAMES_ONLY_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_I_FRAMES_ONLY_TAG;
            }

            public bool hasData()
            {
                return false;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                ParseUtil.match(Constants.EXT_X_I_FRAMES_ONLY_PATTERN, line, getTag());

                if (state.getCompatibilityVersion() < 4)
                {
                    throw ParseException.create(ParseExceptionType.REQUIRES_PROTOCOL_VERSION_4_OR_HIGHER, getTag());
                }

                state.setIsIframesOnly();
            }
        };

        public static readonly IExtTagParser EXT_X_PLAYLIST_TYPE = new EXT_X_PLAYLIST_TYPE_CLASS();
        private class EXT_X_PLAYLIST_TYPE_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_PLAYLIST_TYPE_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_PLAYLIST_TYPE_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                Match match = ParseUtil.match(Constants.EXT_X_PLAYLIST_TYPE_PATTERN, line, getTag());

                if (state.getMedia().playlistType != null)
                {
                    throw ParseException.create(ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES, getTag(), line);
                }

                //state.getMedia().playlistType = ParseUtil.parseEnum(match.Groups[1].Value, typeof(PlaylistType), getTag());
                state.getMedia().playlistType = PlaylistType.fromValue(match.Groups[1].Value);
            }
        }


        public static readonly IExtTagParser EXT_X_PROGRAM_DATE_TIME = new EXT_X_PROGRAM_DATE_TIME_CLASS();
        private class EXT_X_PROGRAM_DATE_TIME_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_PROGRAM_DATE_TIME_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_PROGRAM_DATE_TIME_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                Match match = ParseUtil.match(Constants.EXT_X_PROGRAM_DATE_TIME_PATTERN, line, getTag());

                if (state.getMedia().programDateTime != null)
                {
                    throw ParseException.create(ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES, getTag(), line);
                }

                state.getMedia().programDateTime = ParseUtil.parseDateTime(line, getTag());
            }
        };

        public static readonly IExtTagParser EXT_X_START = new EXT_X_START_CLASS();
        private class EXT_X_START_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_START_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
                HANDLERS.Add(Constants.TIME_OFFSET, new TIME_OFFSET_AttributeParser());
                HANDLERS.Add(Constants.PRECISE, new PRECISE_AttributeParser());
            }

            private readonly Dictionary<String, AttributeParser<StartData.Builder>> HANDLERS = new Dictionary<String, AttributeParser<StartData.Builder>>();


            private class TIME_OFFSET_AttributeParser : AttributeParser<StartData.Builder>
            {
                public void parse(Attribute attribute, StartData.Builder builder, ParseState state) // throws ParseException 
                {
                    builder.withTimeOffset(ParseUtil.parseFloat(attribute.value));
                }
            }

            private class PRECISE_AttributeParser : AttributeParser<StartData.Builder>
            {
                public void parse(Attribute attribute, StartData.Builder builder, ParseState state) // throws ParseException 
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
                lineParser.parse(line, state);

                StartData.Builder builder = new StartData.Builder();
                ParseUtil.parseAttributes(line, builder, state, HANDLERS, getTag());
                StartData startData = builder.build();

                state.getMedia().setStartData(startData);
            }
        };


        public static readonly IExtTagParser EXT_X_TARGETDURATION = new EXT_X_TARGETDURATION_CLASS();
        private class EXT_X_TARGETDURATION_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_TARGETDURATION_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_TARGETDURATION_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                Match match = ParseUtil.match(Constants.EXT_X_TARGETDURATION_PATTERN, line, getTag());

                if (state.getMedia().targetDuration != null)
                {
                    throw ParseException.create(ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES, getTag(), line);
                }

                state.getMedia().targetDuration = ParseUtil.parseInt(match.Groups[1].Value, getTag());
            }
        };

        public static readonly IExtTagParser EXT_X_MEDIA_SEQUENCE = new EXT_X_MEDIA_SEQUENCE_CLASS();
        private class EXT_X_MEDIA_SEQUENCE_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_MEDIA_SEQUENCE_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_MEDIA_SEQUENCE_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                Match match = ParseUtil.match(Constants.EXT_X_MEDIA_SEQUENCE_PATTERN, line, getTag());

                if (state.getMedia().mediaSequenceNumber != null)
                {
                    throw ParseException.create(ParseExceptionType.MULTIPLE_EXT_TAG_INSTANCES, getTag(), line);
                }

                state.getMedia().mediaSequenceNumber = ParseUtil.parseInt(match.Groups[1].Value, getTag());
            }
        };

        public static readonly IExtTagParser EXT_X_ALLOW_CACHE = new EXT_X_ALLOW_CACHE_CLASS();
        private class EXT_X_ALLOW_CACHE_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_ALLOW_CACHE_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_ALLOW_CACHE_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                // deprecated
            }
        };

        // media segment tags

        public static readonly IExtTagParser EXTINF = new EXTINF_CLASS();
        private class EXTINF_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXTINF_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXTINF_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                Match match = ParseUtil.match(Constants.EXTINF_PATTERN, line, getTag());

                state.getMedia().trackInfo = new TrackInfo(ParseUtil.parseFloat(match.Groups[1].Value, getTag()), match.Groups[2].Value);
            }
        };

        public static readonly IExtTagParser EXT_X_DISCONTINUITY = new EXT_X_DISCONTINUITY_CLASS();
        private class EXT_X_DISCONTINUITY_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_DISCONTINUITY_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_DISCONTINUITY_TAG;
            }

            public bool hasData()
            {
                return false;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);
                Match match = ParseUtil.match(Constants.EXT_X_DISCONTINUITY_PATTERN, line, getTag());
                state.getMedia().hasDiscontinuity = true;
            }
        };

        public static readonly IExtTagParser EXT_X_KEY = new EXT_X_KEY_CLASS();
        private class EXT_X_KEY_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_KEY_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
                HANDLERS.Add(Constants.METHOD, new METHOD_AttributeParser());
                HANDLERS.Add(Constants.URI, new URI_AttributeParser());
                HANDLERS.Add(Constants.IV, new IV_AttributeParser());
                HANDLERS.Add(Constants.KEY_FORMAT, new KEY_FORMAT_AttributeParser());
                HANDLERS.Add(Constants.KEY_FORMAT_VERSIONS, new KEY_FORMAT_VERSIONS_AttributeParser());
            }

            private readonly Dictionary<String, AttributeParser<EncryptionData.Builder>> HANDLERS = new Dictionary<String, AttributeParser<EncryptionData.Builder>>();


            private class METHOD_AttributeParser : AttributeParser<EncryptionData.Builder>
            {
                public void parse(Attribute attribute, EncryptionData.Builder builder, ParseState state) // throws ParseException 
                {
                    EncryptionMethod method = EncryptionMethod.fromValue(attribute.value);

                    if (method == null)
                    {
                        throw ParseException.create(ParseExceptionType.INVALID_ENCRYPTION_METHOD, tag: null, context: attribute.ToString());
                    }
                    else
                    {
                        builder.withMethod(method);
                    }
                }
            }

            private class URI_AttributeParser : AttributeParser<EncryptionData.Builder>
            {
                public void parse(Attribute attribute, EncryptionData.Builder builder, ParseState state) // throws ParseException 
                {
                    builder.withUri(ParseUtil.decodeUri(ParseUtil.parseQuotedString(attribute.value), state.encoding));
                }
            }

            private class IV_AttributeParser : AttributeParser<EncryptionData.Builder>
            {
                public void parse(Attribute attribute, EncryptionData.Builder builder, ParseState state) // throws ParseException 
                {
                    List<Byte> initializationVector = ParseUtil.parseHexadecimal(attribute.value);

                    if ((initializationVector.Count != Constants.IV_SIZE) &&
                        (initializationVector.Count != Constants.IV_SIZE_ALTERNATIVE))
                    {
                        throw ParseException.create(ParseExceptionType.INVALID_IV_SIZE, tag: null, context: attribute.ToString());
                    }

                    builder.withInitializationVector(initializationVector);
                }
            }

            private class KEY_FORMAT_AttributeParser : AttributeParser<EncryptionData.Builder>
            {
                public void parse(Attribute attribute, EncryptionData.Builder builder, ParseState state) // throws ParseException 
                {
                    builder.withKeyFormat(ParseUtil.parseQuotedString(attribute.value));
                }
            }

            private class KEY_FORMAT_VERSIONS_AttributeParser : AttributeParser<EncryptionData.Builder>
            {
                public void parse(Attribute attribute, EncryptionData.Builder builder, ParseState state) // throws ParseException 
                {
                    String[] versionStrings = ParseUtil.parseQuotedString(attribute.value).Split(Constants.LIST_SEPARATOR);
                    List<int> versions = new List<int>();

                    foreach (String version in versionStrings)
                    {
                        try
                        {
                            versions.Add(Int32.Parse(version));
                        }
                        catch (FormatException exception)
                        {
                            throw ParseException.create(ParseExceptionType.INVALID_KEY_FORMAT_VERSIONS, tag: null, context: attribute.ToString());
                        }
                    }

                    builder.withKeyFormatVersions(versions);
                }
            }


            public String getTag()
            {
                return Constants.EXT_X_KEY_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                EncryptionData.Builder builder = new EncryptionData.Builder()
                        .withKeyFormat(Constants.DEFAULT_KEY_FORMAT)
                        .withKeyFormatVersions(Constants.DEFAULT_KEY_FORMAT_VERSIONS);

                ParseUtil.parseAttributes(line, builder, state, HANDLERS, getTag());

                EncryptionData encryptionData = builder.build();

                if (encryptionData.getMethod() != EncryptionMethod.NONE && encryptionData.getUri() == null)
                {
                    throw ParseException.create(ParseExceptionType.MISSING_ENCRYPTION_URI, getTag(), line);
                }

                state.getMedia().encryptionData = encryptionData;
            }
        };

        public static readonly IExtTagParser EXT_X_MAP = new EXT_X_MAP_CLASS();
        private class EXT_X_MAP_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_MAP_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
                HANDLERS.Add(Constants.URI, new URI_AttributeParser());
                HANDLERS.Add(Constants.BYTERANGE, new BYTERANGE_AttributeParser());
            }

            private readonly Dictionary<String, AttributeParser<MapInfo.Builder>> HANDLERS = new Dictionary<String, AttributeParser<MapInfo.Builder>>();


            private class URI_AttributeParser : AttributeParser<MapInfo.Builder>
            {
                public void parse(Attribute attribute, MapInfo.Builder builder, ParseState state) // throws ParseException 
                {
                    builder.withUri(ParseUtil.decodeUri(ParseUtil.parseQuotedString(attribute.value), state.encoding));
                }
            }

            private class BYTERANGE_AttributeParser : AttributeParser<MapInfo.Builder>
            {
                public void parse(Attribute attribute, MapInfo.Builder builder, ParseState state) // throws ParseException 
                {
                    Match match = Constants.EXT_X_BYTERANGE_VALUE_PATTERN.Match(ParseUtil.parseQuotedString(attribute.value));
                    if (!match.Success)
                    {
                        throw ParseException.create(ParseExceptionType.INVALID_BYTERANGE_FORMAT, tag: null, context: attribute.ToString());
                    }

                    builder.withByteRange(ParseUtil.matchByteRange(match));
                }
            }


            public String getTag()
            {
                return Constants.EXT_X_MAP;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);

                MapInfo.Builder builder = new MapInfo.Builder();

                ParseUtil.parseAttributes(line, builder, state, HANDLERS, getTag());
                state.getMedia().mapInfo = builder.build();
            }
        };

        public static readonly IExtTagParser EXT_X_BYTERANGE = new EXT_X_BYTERANGE_CLASS();
        private class EXT_X_BYTERANGE_CLASS : IExtTagParser
        {
            private readonly LineParser lineParser;

            public EXT_X_BYTERANGE_CLASS()
            {
                lineParser = new MediaPlaylistLineParser(this);
            }

            public String getTag()
            {
                return Constants.EXT_X_BYTERANGE_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) // throws ParseException 
            {
                lineParser.parse(line, state);
                Match match = ParseUtil.match(Constants.EXT_X_BYTERANGE_PATTERN, line, getTag());
                state.getMedia().byteRange = ParseUtil.matchByteRange(match);
            }
        };
    }
}
