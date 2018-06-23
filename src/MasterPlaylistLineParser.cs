using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class MasterPlaylistLineParser : LineParser
    {
        private readonly IExtTagParser mTagParser;
        private readonly LineParser mLineParser;

        MasterPlaylistLineParser(IExtTagParser parser) : this(parser, new ExtLineParser(parser)) { }

        MasterPlaylistLineParser(IExtTagParser tagParser, LineParser lineParser)
        {
            mTagParser = tagParser;
            mLineParser = lineParser;
        }

        public void parse(String line, ParseState state)
        {
            if (state.isMedia())
            {
                throw ParseException.create(ParseExceptionType.MASTER_IN_MEDIA, mTagParser.getTag());
            }

            state.setMaster();
            mLineParser.parse(line, state);
        }

        // master playlist tags

        public static readonly IExtTagParser EXT_X_MEDIA = new EXT_X_MEDIA_CLASS();
        private class EXT_X_MEDIA_CLASS : IExtTagParser
        {
            private readonly LineParser mLineParser;
            private readonly Dictionary<String, AttributeParser<MediaData.Builder>> HANDLERS =
                new Dictionary<String, AttributeParser<MediaData.Builder>>();

            public EXT_X_MEDIA_CLASS()
            {
                mLineParser = new MasterPlaylistLineParser(this);

                HANDLERS.Add(Constants.TYPE, new TYPE_AttributeParser());
                HANDLERS.Add(Constants.URI, new URI_AttributeParser());
                HANDLERS.Add(Constants.GROUP_ID, new GROUP_ID_AttributeParser());
                HANDLERS.Add(Constants.LANGUAGE, new LANGUAGE_AttributeParser());
                HANDLERS.Add(Constants.ASSOCIATED_LANGUAGE, new ASSOCIATED_LANGUAGE_AttributeParser());
                HANDLERS.Add(Constants.NAME, new NAME_AttributeParser());
                HANDLERS.Add(Constants.DEFAULT, new DEFAULT_AttributeParser());
                HANDLERS.Add(Constants.AUTO_SELECT, new AUTO_SELECT_AttributeParser());
                HANDLERS.Add(Constants.FORCED, new FORCED_AttributeParser());
                HANDLERS.Add(Constants.IN_STREAM_ID, new IN_STREAM_ID_AttributeParser());
                HANDLERS.Add(Constants.CHARACTERISTICS, new CHARACTERISTICS_AttributeParser());
                HANDLERS.Add(Constants.CHANNELS, new CHANNELS_AttributeParser());
            }
            
            private class TYPE_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    MediaType type = MediaType.fromValue(attribute.value);

                    if (type == null)
                    {
                        throw ParseException.create(ParseExceptionType.INVALID_MEDIA_TYPE, tag: null, context: attribute.ToString());
                    }
                    else
                    {
                        builder.withType(type);
                    }
                }
            }

            private class URI_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    builder.withUri(ParseUtil.decodeUri(ParseUtil.parseQuotedString(attribute.value), state.encoding));
                }
            }

            private class GROUP_ID_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    String groupId = ParseUtil.parseQuotedString(attribute.value);

                    if (groupId.isEmpty())
                    {
                        throw ParseException.create(ParseExceptionType.EMPTY_MEDIA_GROUP_ID, tag: null, context: attribute.ToString());
                    }
                    else
                    {
                        builder.withGroupId(groupId);
                    }
                }
            }

            private class LANGUAGE_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    builder.withLanguage(ParseUtil.parseQuotedString(attribute.value));
                }
            }

            private class ASSOCIATED_LANGUAGE_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    builder.withAssociatedLanguage(ParseUtil.parseQuotedString(attribute.value));
                }
            }

            private class NAME_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    String name = ParseUtil.parseQuotedString(attribute.value);

                    if (name.isEmpty())
                    {
                        throw ParseException.create(ParseExceptionType.EMPTY_MEDIA_NAME, tag: null, context: attribute.ToString());
                    }
                    else
                    {
                        builder.withName(name);
                    }
                }
            }

            private class DEFAULT_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    bool isDefault = ParseUtil.parseYesNo(attribute);

                    builder.withDefault(isDefault);
                    state.getMaster().isDefault = isDefault;

                    if (isDefault)
                    {
                        if (state.getMaster().isNotAutoSelect)
                        {
                            throw ParseException.create(ParseExceptionType.AUTO_SELECT_DISABLED_FOR_DEFAULT, tag: null, context: attribute.ToString());
                        }

                        builder.withAutoSelect(true);
                    }
                }
            }

            private class AUTO_SELECT_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    bool isAutoSelect = ParseUtil.parseYesNo(attribute);

                    builder.withAutoSelect(isAutoSelect);
                    state.getMaster().isNotAutoSelect = !isAutoSelect;

                    if (state.getMaster().isDefault && !isAutoSelect)
                    {
                        throw ParseException.create(ParseExceptionType.AUTO_SELECT_DISABLED_FOR_DEFAULT, tag: null, context: attribute.ToString());
                    }
                }
            }

            private class FORCED_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    builder.withForced(ParseUtil.parseYesNo(attribute));
                }
            }

            private class IN_STREAM_ID_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    String inStreamId = ParseUtil.parseQuotedString(attribute.value);

                    if (Constants.EXT_X_MEDIA_IN_STREAM_ID_PATTERN.Match(inStreamId).Success)
                    {
                        builder.withInStreamId(inStreamId);
                    }
                    else
                    {
                        throw ParseException.create(ParseExceptionType.INVALID_MEDIA_IN_STREAM_ID, tag: null, context: attribute.ToString());
                    }
                }
            }

            private class CHARACTERISTICS_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    String[] characteristicStrings = ParseUtil.parseQuotedString(attribute.value).Split(Constants.COMMA_CHAR);

                    if (characteristicStrings.Length == 0)
                    {
                        throw ParseException.create(ParseExceptionType.EMPTY_MEDIA_CHARACTERISTICS, tag: null, context: attribute.ToString());
                    }
                    else
                    {
                        builder.withCharacteristics(characteristicStrings.ToList());
                    }
                }
            }

            private class CHANNELS_AttributeParser : AttributeParser<MediaData.Builder>
            {
                public void parse(Attribute attribute, MediaData.Builder builder, ParseState state)
                {
                    String[] channelsStrings = ParseUtil.parseQuotedString(attribute.value).Split(Constants.LIST_SEPARATOR);

                    if (channelsStrings.Length == 0 || channelsStrings[0].isEmpty())
                    {
                        throw ParseException.create(ParseExceptionType.EMPTY_MEDIA_CHANNELS, tag: null, context: attribute.ToString());
                    }
                    else
                    {
                        int channelsCount = ParseUtil.parseInt(channelsStrings[0]);
                        builder.withChannels(channelsCount);
                    }
                }
            }

            public String getTag()
            {
                return Constants.EXT_X_MEDIA_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state)
            {
                mLineParser.parse(line, state);

                MediaData.Builder builder = new MediaData.Builder();

                state.getMaster().clearMediaDataState();
                ParseUtil.parseAttributes(line, builder, state, HANDLERS, getTag());
                state.getMaster().mediaData.Add(builder.build());
            }
        }

        public static readonly IExtTagParser EXT_X_I_FRAME_STREAM_INF = new EXT_X_I_FRAME_STREAM_INF_CLASS();
        private class EXT_X_I_FRAME_STREAM_INF_CLASS : IExtTagParser
        {
            private readonly LineParser mLineParser;
            private readonly Dictionary<String, AttributeParser<IFrameStreamInfo.Builder>> HANDLERS;

            public EXT_X_I_FRAME_STREAM_INF_CLASS()
            {
                mLineParser = new MasterPlaylistLineParser(this);
                HANDLERS = HandlerMaker<IFrameStreamInfo.Builder>.makeExtStreamInfHandlers(getTag());
                HANDLERS.Add(Constants.URI, new URI_AttributeParser());
            }

            private class URI_AttributeParser : AttributeParser<IFrameStreamInfo.Builder>
            {
                public void parse(Attribute attribute, IFrameStreamInfo.Builder builder, ParseState state)
                {
                    builder.withUri(ParseUtil.parseQuotedString(attribute.value));
                }
            }

            public String getTag()
            {
                return Constants.EXT_X_I_FRAME_STREAM_INF_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) 
            {
                mLineParser.parse(line, state);

                IFrameStreamInfo.Builder builder = new IFrameStreamInfo.Builder();

                ParseUtil.parseAttributes(line, builder, state, HANDLERS, getTag());
                state.getMaster().iFramePlaylists.Add(builder.build());
            }
        }

        public static readonly IExtTagParser EXT_X_STREAM_INF = new EXT_X_STREAM_INF_CLASS();
        private class EXT_X_STREAM_INF_CLASS : IExtTagParser
        {
            private readonly LineParser mLineParser;
            private readonly Dictionary<String, AttributeParser<StreamInfo.Builder>> HANDLERS;

            public EXT_X_STREAM_INF_CLASS()
            {
                mLineParser = new MasterPlaylistLineParser(this);
                HANDLERS = HandlerMaker<StreamInfo.Builder>.makeExtStreamInfHandlers(getTag());

                HANDLERS.Add(Constants.AUDIO, new AUDIO_AttributeParser());
                HANDLERS.Add(Constants.SUBTITLES, new SUBTITLES_AttributeParser());
                HANDLERS.Add(Constants.CLOSED_CAPTIONS, new CLOSED_CAPTIONS_AttributeParser());
            }

            private class AUDIO_AttributeParser : AttributeParser<StreamInfo.Builder>
            {
                public void parse(Attribute attribute, StreamInfo.Builder builder, ParseState state) 
                {
                    builder.withAudio(ParseUtil.parseQuotedString(attribute.value));
                }
            }

            private class SUBTITLES_AttributeParser : AttributeParser<StreamInfo.Builder>
            {
                public void parse(Attribute attribute, StreamInfo.Builder builder, ParseState state) 
                {
                    builder.withSubtitles(ParseUtil.parseQuotedString(attribute.value));
                }
            }

            private class CLOSED_CAPTIONS_AttributeParser : AttributeParser<StreamInfo.Builder>
            {
                public void parse(Attribute attribute, StreamInfo.Builder builder, ParseState state) 
                {
                    if (!attribute.value.Equals(Constants.NO_CLOSED_CAPTIONS))
                    {
                        builder.withClosedCaptions(ParseUtil.parseQuotedString(attribute.value));
                    }
                }
            }

            public String getTag()
            {
                return Constants.EXT_X_STREAM_INF_TAG;
            }

            public bool hasData()
            {
                return true;
            }

            public void parse(String line, ParseState state) 
            {
                mLineParser.parse(line, state);

                StreamInfo.Builder builder = new StreamInfo.Builder();

                ParseUtil.parseAttributes(line, builder, state, HANDLERS, getTag());
                state.getMaster().streamInfo = builder.build();
            }
        };

        public class HandlerMaker<T>
            where T : StreamInfoBuilder
        {
            public static Dictionary<String, AttributeParser<T>> makeExtStreamInfHandlers(String tag)
            {
                Dictionary<String, AttributeParser<T>> handlers = new Dictionary<String, AttributeParser<T>>();

                handlers.Add(Constants.BANDWIDTH, new BANDWIDTH_AttributeParser(tag));
                handlers.Add(Constants.AVERAGE_BANDWIDTH, new AVERAGE_BANDWIDTH_AttributeParser(tag));
                handlers.Add(Constants.CODECS, new CODECS_AttributeParser(tag));
                handlers.Add(Constants.RESOLUTION, new RESOLUTION_AttributeParser(tag));
                handlers.Add(Constants.FRAME_RATE, new FRAME_RATE_AttributeParser(tag));
                handlers.Add(Constants.VIDEO, new VIDEO_AttributeParser(tag));
                handlers.Add(Constants.PROGRAM_ID, new PROGRAM_ID_AttributeParser(tag));
                return handlers;
            }

            abstract class BaseAttributeParser : AttributeParser<T>
            {
                protected String tag;
                public BaseAttributeParser(String sTag)
                {
                    tag = sTag;
                }

                public abstract void parse(Attribute attribute, T builder, ParseState state);
            }

            private class BANDWIDTH_AttributeParser : BaseAttributeParser
            {
                public BANDWIDTH_AttributeParser(String sTag) : base(sTag) { }
                public override void parse(Attribute attribute, T builder, ParseState state) 
                {
                    builder.withBandwidth(ParseUtil.parseInt(attribute.value, tag));
                }
            }

            private class AVERAGE_BANDWIDTH_AttributeParser : BaseAttributeParser
            {
                public AVERAGE_BANDWIDTH_AttributeParser(String sTag) : base(sTag) { }
                public override void parse(Attribute attribute, T builder, ParseState state) 
                {
                    builder.withAverageBandwidth(ParseUtil.parseInt(attribute.value, tag));
                }
            }

            private class CODECS_AttributeParser : BaseAttributeParser
            {
                public CODECS_AttributeParser(String sTag) : base(sTag) { }
                public override void parse(Attribute attribute, T builder, ParseState state) 
                {
                    String[] characteristicStrings = ParseUtil.parseQuotedString(attribute.value, tag).Split(Constants.COMMA_CHAR);

                    if (characteristicStrings.Length > 0)
                    {
                        builder.withCodecs(characteristicStrings.ToList());
                    }
                }
            }

            private class RESOLUTION_AttributeParser : BaseAttributeParser
            {
                public RESOLUTION_AttributeParser(String sTag) : base(sTag) { }
                public override void parse(Attribute attribute, T builder, ParseState state) 
                {
                    builder.withResolution(ParseUtil.parseResolution(attribute.value, tag));
                }
            }

            private class FRAME_RATE_AttributeParser : BaseAttributeParser
            {
                public FRAME_RATE_AttributeParser(String sTag) : base(sTag) { }
                public override void parse(Attribute attribute, T builder, ParseState state) 
                {
                    builder.withFrameRate(ParseUtil.parseFloat(attribute.value, tag));
                }
            }

            private class VIDEO_AttributeParser : BaseAttributeParser
            {
                public VIDEO_AttributeParser(String sTag) : base(sTag) { }
                public override void parse(Attribute attribute, T builder, ParseState state) 
                {
                    builder.withVideo(ParseUtil.parseQuotedString(attribute.value, tag));
                }
            }

            class PROGRAM_ID_AttributeParser : BaseAttributeParser
            {
                public PROGRAM_ID_AttributeParser(string sTag) : base(sTag) { }

                public override void parse(Attribute attribute, T builder, ParseState state) 
                {
                    // deprecated
                }
            }
        }
    }
}
