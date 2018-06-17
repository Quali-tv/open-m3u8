using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace M3U8Parser
{
    public class ExtendedM3uParser : BaseM3uParser
    {
        private readonly ParsingMode mParsingMode;
        private readonly Dictionary<String, IExtTagParser> mExtTagParsers = new Dictionary<String, IExtTagParser>();

        public ExtendedM3uParser(Stream inputStream, Encoding encoding, ParsingMode parsingMode) :
            base(inputStream, encoding)
        {

            mParsingMode = parsingMode;

            // TODO implement the remaining EXT tag handlers and add them here
            putParsers(
                    ExtLineParser.EXTM3U_HANDLER,
                    ExtLineParser.EXT_X_VERSION_HANDLER,
                    ExtLineParser.EXT_X_START,
                    MediaPlaylistLineParser.EXT_X_PLAYLIST_TYPE,
                    MediaPlaylistLineParser.EXT_X_PROGRAM_DATE_TIME,
                    MediaPlaylistLineParser.EXT_X_KEY,
                    MediaPlaylistLineParser.EXT_X_TARGETDURATION,
                    MediaPlaylistLineParser.EXT_X_MEDIA_SEQUENCE,
                    MediaPlaylistLineParser.EXT_X_I_FRAMES_ONLY,
                    MasterPlaylistLineParser.EXT_X_MEDIA,
                    MediaPlaylistLineParser.EXT_X_ALLOW_CACHE,
                    MasterPlaylistLineParser.EXT_X_STREAM_INF,
                    MasterPlaylistLineParser.EXT_X_I_FRAME_STREAM_INF,
                    MediaPlaylistLineParser.EXTINF,
                    MediaPlaylistLineParser.EXT_X_ENDLIST,
                    MediaPlaylistLineParser.EXT_X_DISCONTINUITY,
                    MediaPlaylistLineParser.EXT_X_MAP,
                    MediaPlaylistLineParser.EXT_X_BYTERANGE
            );
        }

        public override Playlist parse()
        { // throws IOException, ParseException, PlaylistException
            validateAvailable();

            ParseState state = new ParseState(mEncoding);
            LineParser playlistParser = new PlaylistLineParser();
            LineParser trackLineParser = new TrackLineParser();

            try
            {
                while (mScanner.hasNext())
                {
                    String line = mScanner.next();
                    checkWhitespace(line);

                    if (line.Length == 0 || isComment(line))
                    {
                        continue;
                    }
                    else
                    {
                        if (isExtTag(line))
                        {
                            String tagKey = getExtTagKey(line);
                            mExtTagParsers.TryGetValue(tagKey, out IExtTagParser tagParser);

                            if (tagParser == null)
                            {
                                //To support forward compatibility, when parsing Playlists, Clients
                                //MUST:
                                //o  ignore any unrecognized tags.
                                if (mParsingMode.allowUnknownTags)
                                {
                                    tagParser = ExtLineParser.EXT_UNKNOWN_HANDLER;
                                }
                                else
                                {
                                    throw ParseException.create(ParseExceptionType.UNSUPPORTED_EXT_TAG_DETECTED, tagKey, line);
                                }
                            }

                            tagParser.parse(line, state);

                            if (state.isMedia() && state.getMedia().endOfList)
                            {
                                break;
                            }
                        }
                        else if (state.isMaster())
                        {
                            playlistParser.parse(line, state);
                        }
                        else if (state.isMedia())
                        {
                            trackLineParser.parse(line, state);
                        }
                        else
                        {
                            throw ParseException.create(ParseExceptionType.UNKNOWN_PLAYLIST_TYPE, line);
                        }
                    }
                }

                Playlist playlist = state.buildPlaylist();
                PlaylistValidation validation = PlaylistValidation.from(playlist, mParsingMode);

                if (validation.isValid())
                {
                    return playlist;
                }
                else
                {
                    throw new PlaylistException(mScanner.getInput(), validation.getErrors());
                }
            }
            catch (ParseException exception)
            {
                exception.setInput(mScanner.getInput());
                throw exception;
            }
        }

        private void putParsers(params IExtTagParser[] parsers)
        {
            if (parsers != null)
            {
                foreach (IExtTagParser parser in parsers)
                {
                    mExtTagParsers.Add(parser.getTag(), parser);
                }
            }
        }

        private void checkWhitespace(String line)
        { //throws ParseException
            if (!isComment(line))
            {
                if (line.Length != line.Trim().Length)
                {
                    throw ParseException.create(ParseExceptionType.WHITESPACE_IN_TRACK, line);
                }
            }
        }

        private bool isComment(String line)
        {
            return line.StartsWith(Constants.COMMENT_PREFIX) && !isExtTag(line);
        }

        private bool isExtTag(String line)
        {
            return line.StartsWith(Constants.EXT_TAG_PREFIX);
        }

        private String getExtTagKey(String line)
        {
            int index = line.IndexOf(Constants.EXT_TAG_END);

            if (index == -1)
            {
                return line.Substring(1);
            }
            else
            {
                return line.Substring(1, index - 1);
            }
        }
    }
}
