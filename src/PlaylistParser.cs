using System;
using System.IO;
using System.Text;

namespace M3U8Parser
{
    public class PlaylistParser : IPlaylistParser
    {
        private readonly IPlaylistParser mPlaylistParser;

        /**
         * Equivalent to:
         * <pre>
         *     new PlaylistParser(inputStream, format, filename, ParsingMode.STRICT);
         * </pre>
         * @param inputStream an open input stream positioned at the beginning of the file
         * @param format requires the playlist to be this format
         * @param filename the extension of this filename will be used to determine the encoding required of the playlist
         */
        public PlaylistParser(Stream inputStream, Format format, String filename) :
            this(inputStream, format, parseExtension(filename), ParsingMode.STRICT)
        {
        }

        /**
         * @param inputStream an open input stream positioned at the beginning of the file
         * @param format requires the playlist to be this format
         * @param filename the extension of this filename will be used to determine the encoding required of the playlist
         * @param parsingMode indicates how to handle unknown lines in the input stream
         */
        public PlaylistParser(Stream inputStream, Format format, String filename, ParsingMode parsingMode) :
            this(inputStream, format, parseExtension(filename), parsingMode)
        {
        }

        /**
         * Equivalent to:
         * <pre>
         *     new PlaylistParser(inputStream, format, extension, ParsingMode.STRICT);
         * </pre>
         * @param inputStream an open input stream positioned at the beginning of the file
         * @param format requires the playlist to be this format
         * @param extension requires the playlist be encoded according to this extension {M3U : windows-1252, M3U8 : utf-8}
         */
        public PlaylistParser(Stream inputStream, Format format, Extension extension) :
            this(inputStream, format, extension.encoding, ParsingMode.STRICT)
        {
        }

        /**
         * @param inputStream an open input stream positioned at the beginning of the file
         * @param format requires the playlist to be this format
         * @param extension requires the playlist be encoded according to this extension {M3U : windows-1252, M3U8 : utf-8}
         * @param parsingMode indicates how to handle unknown lines in the input stream
         */
        public PlaylistParser(Stream inputStream, Format format, Extension extension, ParsingMode parsingMode) :
            this(inputStream, format, extension.encoding, parsingMode)
        {
        }

        /**
         * Equivalent to:
         * <pre>
         *     new PlaylistParser(inputStream, format, encoding, ParsingMode.STRICT);
         * </pre>
         * @param inputStream an open input stream positioned at the beginning of the file
         * @param format requires the playlist to be this format
         * @param encoding required encoding for the playlist
         */
        public PlaylistParser(Stream inputStream, Format format, Encoding encoding) :
            this(inputStream, format, encoding, ParsingMode.STRICT)
        {
        }

        /**
         * @param inputStream an open input stream positioned at the beginning of the file
         * @param format requires the playlist to be this format
         * @param encoding required encoding for the playlist
         * @param parsingMode indicates how to handle unknown lines in the input stream
         */
        public PlaylistParser(Stream inputStream, Format format, Encoding encoding, ParsingMode parsingMode)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream is null");
            }

            if (format == null)
            {
                throw new ArgumentNullException("format is null");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding is null");
            }

            if (parsingMode == null && format != Format.M3U)
            {
                throw new ArgumentNullException("parsingMode is null");
            }

            switch (format)
            {
                case Format.M3U:
                    mPlaylistParser = new M3uParser(inputStream, encoding);
                    break;
                case Format.EXT_M3U:
                    mPlaylistParser = new ExtendedM3uParser(inputStream, encoding, parsingMode);
                    break;
                default:
                    throw new Exception("unsupported format detected, this should be impossible: " + format);
            }
        }

        /**
         * This will not close the Stream.
         * @return Playlist which is either a MasterPlaylist or a MediaPlaylist, this will never return null
         * @throws IOException if the Stream throws an IOException
         * @throws java.io.EOFException if there is no data to parse
         * @throws ParseException if there is a syntactic error in the playlist
         * @throws PlaylistException if the data in the parsed playlist is invalid
         */
        public Playlist parse() // throws IOException, ParseException, PlaylistException 
        {
            return mPlaylistParser.parse();
        }

        /**
         * @return true if there is more data to parse, false otherwise
         */
        public bool isAvailable()
        {
            return mPlaylistParser.isAvailable();
        }

        private static Extension parseExtension(String filename)
        {
            if (filename == null)
            {
                throw new ArgumentException("filename is null");
            }

            int index = filename.LastIndexOf(".");

            if (index == -1)
            {
                throw new ArgumentException("filename has no extension: " + filename);
            }
            else
            {
                String extension = filename.Substring(index + 1);

                if (Extension.M3U.value.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Extension.M3U;
                }
                else if (Extension.M3U8.value.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Extension.M3U8;
                }
                else
                {
                    throw new ArgumentException("filename extension should be .m3u or .m3u8: " + filename);
                }
            }
        }
    }
}
