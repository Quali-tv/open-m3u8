using System;
using System.Text;

namespace M3U8Parser
{
    public class ParseExceptionType
    {
        public static readonly ParseExceptionType AUTO_SELECT_DISABLED_FOR_DEFAULT = new ParseExceptionType("default media data must be auto selected");
        public static readonly ParseExceptionType BAD_EXT_TAG_FORMAT = new ParseExceptionType("bad format found for an EXT tag");
        public static readonly ParseExceptionType EMPTY_MEDIA_CHANNELS = new ParseExceptionType("CHANNELS is empty");
        public static readonly ParseExceptionType EMPTY_MEDIA_CHARACTERISTICS = new ParseExceptionType("CHARACTERISTICS is empty");
        public static readonly ParseExceptionType EMPTY_MEDIA_GROUP_ID = new ParseExceptionType("GROUP-ID is empty");
        public static readonly ParseExceptionType EMPTY_MEDIA_NAME = new ParseExceptionType("NAME is empty");
        public static readonly ParseExceptionType ILLEGAL_WHITESPACE = new ParseExceptionType("found illegal whitespace");
        public static readonly ParseExceptionType INTERNAL_ERROR = new ParseExceptionType("there was an unrecoverable problem");
        public static readonly ParseExceptionType INVALID_ATTRIBUTE_NAME = new ParseExceptionType("invalid attribute name");
        public static readonly ParseExceptionType INVALID_COMPATIBILITY_VERSION = new ParseExceptionType("invalid compatibility version");
        public static readonly ParseExceptionType INVALID_ENCRYPTION_METHOD = new ParseExceptionType("invalid encryption method");
        public static readonly ParseExceptionType INVALID_HEXADECIMAL_STRING = new ParseExceptionType("a hexadecimal string was not properly formatted");
        public static readonly ParseExceptionType INVALID_IV_SIZE = new ParseExceptionType("the initialization vector is the wrong size");
        public static readonly ParseExceptionType INVALID_KEY_FORMAT_VERSIONS = new ParseExceptionType("invalid KEYFORMATVERSIONS");
        public static readonly ParseExceptionType INVALID_MEDIA_IN_STREAM_ID = new ParseExceptionType("invalid media INSTREAM-ID");
        public static readonly ParseExceptionType INVALID_MEDIA_TYPE = new ParseExceptionType("invalid media TYPE");
        public static readonly ParseExceptionType INVALID_RESOLUTION_FORMAT = new ParseExceptionType("a resolution was not formatted properly");
        public static readonly ParseExceptionType INVALID_QUOTED_STRING = new ParseExceptionType("a quoted string was not properly formatted");
        public static readonly ParseExceptionType INVALID_DATE_TIME_FORMAT = new ParseExceptionType("a date-time string was not properly formatted");
        public static readonly ParseExceptionType INVALID_BYTERANGE_FORMAT = new ParseExceptionType("a byte range string was not properly formatted");
        public static readonly ParseExceptionType MASTER_IN_MEDIA = new ParseExceptionType("master playlist tags we found in a media playlist");
        public static readonly ParseExceptionType MEDIA_IN_MASTER = new ParseExceptionType("media playlist tags we found in a master playlist");
        public static readonly ParseExceptionType MISSING_ATTRIBUTE_NAME = new ParseExceptionType("missing the name of an attribute");
        public static readonly ParseExceptionType MISSING_ATTRIBUTE_VALUE = new ParseExceptionType("missing the value of an attribute");
        public static readonly ParseExceptionType MISSING_ATTRIBUTE_SEPARATOR = new ParseExceptionType("missing the separator in an attribute");
        public static readonly ParseExceptionType MISSING_ENCRYPTION_URI = new ParseExceptionType("missing the URI for encrypted media segments");
        public static readonly ParseExceptionType MISSING_EXT_TAG_SEPARATOR = new ParseExceptionType("missing the colon after an EXT tag");
        public static readonly ParseExceptionType MISSING_TRACK_INFO = new ParseExceptionType("missing EXTINF for a track in an extended media playlist");
        public static readonly ParseExceptionType MULTIPLE_ATTRIBUTE_NAME_INSTANCES = new ParseExceptionType("multiple instances of an attribute name found in an attribute list");
        public static readonly ParseExceptionType MULTIPLE_EXT_TAG_INSTANCES = new ParseExceptionType("multiple instances of an EXT tag found for which only one is allowed");
        public static readonly ParseExceptionType NOT_JAVA_INTEGER = new ParseExceptionType("only java integers are supported");
        public static readonly ParseExceptionType NOT_JAVA_ENUM = new ParseExceptionType("only specific values are supported");
        public static readonly ParseExceptionType NOT_JAVA_FLOAT = new ParseExceptionType("only java floats are supported");
        public static readonly ParseExceptionType NOT_YES_OR_NO = new ParseExceptionType("the only valid values are YES and NO");
        public static readonly ParseExceptionType UNCLOSED_QUOTED_STRING = new ParseExceptionType("a quoted string was not closed");
        public static readonly ParseExceptionType UNKNOWN_PLAYLIST_TYPE = new ParseExceptionType("unable to determine playlist type");
        public static readonly ParseExceptionType UNSUPPORTED_COMPATIBILITY_VERSION = new ParseExceptionType("open m3u8 does not support this version");
        public static readonly ParseExceptionType UNSUPPORTED_EXT_TAG_DETECTED = new ParseExceptionType("unsupported ext tag detected");
        public static readonly ParseExceptionType WHITESPACE_IN_TRACK = new ParseExceptionType("whitespace was found surrounding a track");
        public static readonly ParseExceptionType REQUIRES_PROTOCOL_VERSION_4_OR_HIGHER = new ParseExceptionType("A Media Playlist REQUIRES protocol version 4 or higher");

        public readonly String message;

        private ParseExceptionType(String message)
        {
            this.message = message;
        }
    }
}
