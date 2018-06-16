using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
//import java.util.Arrays;
//import java.util.List;
//import java.util.regex.Pattern;

namespace M3U8Parser
{
    public static class Constants
    {
        public const String MIME_TYPE = "application/vnd.apple.mpegurl";
        public const String MIME_TYPE_COMPATIBILITY = "audio/mpegurl";

        public const String ATTRIBUTE_SEPARATOR = "=";
        public const char COMMA_CHAR = ',';
        public static readonly String COMMA = COMMA_CHAR.ToString();
        public static readonly String ATTRIBUTE_LIST_SEPARATOR = COMMA;
        public const String LIST_SEPARATOR = "/";
        public const String COMMENT_PREFIX = "#";
        public const String EXT_TAG_PREFIX = "#EXT";
        public const String EXT_TAG_END = ":";
        public const String WRITE_NEW_LINE = "\n";
        public const String PARSE_NEW_LINE = "\\r?\\n";

        // extension tags

        public const String EXTM3U_TAG = "EXTM3U";
        public const String EXT_X_VERSION_TAG = "EXT-X-VERSION";

        // master playlist tags

        public const String URI = "URI";
        public const String BYTERANGE = "BYTERANGE";

        public const String EXT_X_MEDIA_TAG = "EXT-X-MEDIA";
        public const String TYPE = "TYPE";
        public const String GROUP_ID = "GROUP-ID";
        public const String LANGUAGE = "LANGUAGE";
        public const String ASSOCIATED_LANGUAGE = "ASSOC-LANGUAGE";
        public const String NAME = "NAME";
        public const String DEFAULT = "DEFAULT";
        public const String AUTO_SELECT = "AUTOSELECT";
        public const String FORCED = "FORCED";
        public const String IN_STREAM_ID = "INSTREAM-ID";
        public const String CHARACTERISTICS = "CHARACTERISTICS";
        public const String CHANNELS = "CHANNELS";

        public const String EXT_X_STREAM_INF_TAG = "EXT-X-STREAM-INF";
        public const String EXT_X_I_FRAME_STREAM_INF_TAG = "EXT-X-I-FRAME-STREAM-INF";
        public const String BANDWIDTH = "BANDWIDTH";
        public const String AVERAGE_BANDWIDTH = "AVERAGE-BANDWIDTH";
        public const String CODECS = "CODECS";
        public const String RESOLUTION = "RESOLUTION";
        public const String FRAME_RATE = "FRAME-RATE";
        public const String VIDEO = "VIDEO";
        public const String PROGRAM_ID = "PROGRAM-ID";

        public const String AUDIO = "AUDIO";
        public const String SUBTITLES = "SUBTITLES";
        public const String CLOSED_CAPTIONS = "CLOSED-CAPTIONS";


        // media playlist tags

        public const String EXT_X_PLAYLIST_TYPE_TAG = "EXT-X-PLAYLIST-TYPE";
        public const String EXT_X_PROGRAM_DATE_TIME_TAG = "EXT-X-PROGRAM-DATE-TIME";
        public const String EXT_X_TARGETDURATION_TAG = "EXT-X-TARGETDURATION";
        public const String EXT_X_START_TAG = "EXT-X-START";
        public const String TIME_OFFSET = "TIME-OFFSET";
        public const String PRECISE = "PRECISE";

        public const String EXT_X_MEDIA_SEQUENCE_TAG = "EXT-X-MEDIA-SEQUENCE";
        public const String EXT_X_ALLOW_CACHE_TAG = "EXT-X-ALLOW-CACHE";
        public const String EXT_X_ENDLIST_TAG = "EXT-X-ENDLIST";
        public const String EXT_X_I_FRAMES_ONLY_TAG = "EXT-X-I-FRAMES-ONLY";
        public const String EXT_X_DISCONTINUITY_TAG = "EXT-X-DISCONTINUITY";

        // media segment tags

        public const String EXTINF_TAG = "EXTINF";
        public const String EXT_X_KEY_TAG = "EXT-X-KEY";
        public const String METHOD = "METHOD";
        public const String IV = "IV";
        public const String KEY_FORMAT = "KEYFORMAT";
        public const String KEY_FORMAT_VERSIONS = "KEYFORMATVERSIONS";
        public const String EXT_X_MAP = "EXT-X-MAP";
        public const String EXT_X_BYTERANGE_TAG = "EXT-X-BYTERANGE";

        // regular expressions
        public const String YES = "YES";
        public const String NO = "NO";
        private const String INTEGER_REGEX = "\\d+";
        private const String SIGNED_FLOAT_REGEX = "-?\\d*\\.?\\d*";
        private const String TIMESTAMP_REGEX = "\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}(?:\\.\\d{3})?(?:Z?|\\+\\d{2}:\\d{2})?";
        private const String BYTERANGE_REGEX = "(" + INTEGER_REGEX + ")(?:@(" + INTEGER_REGEX + "))?";

        public static readonly Regex HEXADECIMAL_PATTERN = new Regex("^0[x|X]([0-9A-F]+)$");
        public static readonly Regex RESOLUTION_PATTERN = new Regex("^(" + INTEGER_REGEX + ")x(" + INTEGER_REGEX + ")$");

        public static readonly Regex EXT_X_VERSION_PATTERN = new Regex("^#" + EXT_X_VERSION_TAG + EXT_TAG_END + "(" + INTEGER_REGEX + ")$");
        public static readonly Regex EXT_X_TARGETDURATION_PATTERN = new Regex("^#" + EXT_X_TARGETDURATION_TAG + EXT_TAG_END + "(" + INTEGER_REGEX + ")$");
        public static readonly Regex EXT_X_MEDIA_SEQUENCE_PATTERN = new Regex("^#" + EXT_X_MEDIA_SEQUENCE_TAG + EXT_TAG_END + "(" + INTEGER_REGEX + ")$");
        public static readonly Regex EXT_X_PLAYLIST_TYPE_PATTERN = new Regex("^#" + EXT_X_PLAYLIST_TYPE_TAG + EXT_TAG_END + "(EVENT|VOD)$");
        public static readonly Regex EXT_X_PROGRAM_DATE_TIME_PATTERN = new Regex("^#" + EXT_X_PROGRAM_DATE_TIME_TAG + EXT_TAG_END + "(" + TIMESTAMP_REGEX + ")$");
        public static readonly Regex EXT_X_MEDIA_IN_STREAM_ID_PATTERN = new Regex("^CC[1-4]|SERVICE(?:[1-9]|[1-5]\\d|6[0-3])$");
        public static readonly Regex EXTINF_PATTERN = new Regex("^#" + EXTINF_TAG + EXT_TAG_END + "(" + SIGNED_FLOAT_REGEX + ")(?:,(.+)?)?$");
        public static readonly Regex EXT_X_ENDLIST_PATTERN = new Regex("^#" + EXT_X_ENDLIST_TAG + "$");
        public static readonly Regex EXT_X_I_FRAMES_ONLY_PATTERN = new Regex("^#" + EXT_X_I_FRAMES_ONLY_TAG);
        public static readonly Regex EXT_X_DISCONTINUITY_PATTERN = new Regex("^#" + EXT_X_DISCONTINUITY_TAG + "$");
        public static readonly Regex EXT_X_BYTERANGE_PATTERN = new Regex("^#" + EXT_X_BYTERANGE_TAG + EXT_TAG_END + BYTERANGE_REGEX + "$");
        public static readonly Regex EXT_X_BYTERANGE_VALUE_PATTERN = new Regex("^" + BYTERANGE_REGEX + "$");

        // other

        public static readonly byte[] UTF_8_BOM_BYTES = { 0xEF, 0xBB, 0xBF };
        public const char UNICODE_BOM = '\uFEFF';
        public const int MAX_COMPATIBILITY_VERSION = 5;
        public const int IV_SIZE = 16;
        // Against the spec but used by Adobe
        public const int IV_SIZE_ALTERNATIVE = 32;
        public const String DEFAULT_KEY_FORMAT = "identity";
        public const String NO_CLOSED_CAPTIONS = "NONE";
        public static readonly List<int> DEFAULT_KEY_FORMAT_VERSIONS = new List<int> { 1 };
    }
}
