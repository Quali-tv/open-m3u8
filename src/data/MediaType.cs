using System;
using System.Collections.Generic;
using System.Text;
// import java.util.HashMap;
// import java.util.Map;

namespace M3U8Parser
{
    public class MediaType
    {
        public static readonly MediaType AUDIO = new MediaType("AUDIO");
        public static readonly MediaType VIDEO = new MediaType("VIDEO");
        public static readonly MediaType SUBTITLES = new MediaType("SUBTITLES");
        public static readonly MediaType CLOSED_CAPTIONS = new MediaType("CLOSED-CAPTIONS");

        private static readonly Dictionary<String, MediaType> sMap = new Dictionary<String, MediaType>();

        private readonly String value;

        static MediaType()
        {
            sMap.Add(AUDIO.value, AUDIO);
            sMap.Add(VIDEO.value, VIDEO);
            sMap.Add(SUBTITLES.value, SUBTITLES);
            sMap.Add(CLOSED_CAPTIONS.value, CLOSED_CAPTIONS);
        }

        private MediaType(String value)
        {
            this.value = value;
        }

        public static MediaType fromValue(String value)
        {
            return sMap[value];
        }

        public String getValue()
        {
            return value;
        }
    }
}
