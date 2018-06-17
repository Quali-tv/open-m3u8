using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class PlaylistType {
        public static readonly PlaylistType EVENT = new PlaylistType("EVENT");
        public static readonly PlaylistType VOD = new PlaylistType("VOD");
        
        private static readonly Dictionary<String, PlaylistType> sMap = new Dictionary<String, PlaylistType>();

        private readonly String value;

        static PlaylistType() {
                sMap.Add(EVENT.value, EVENT);
                sMap.Add(VOD.value, VOD);
        }

        private PlaylistType(String value) {
            this.value = value;
        }

        public static PlaylistType fromValue(String value) {
            return sMap[value];
        }
        
        public String getValue() {
            return value;
        }
    }
}
