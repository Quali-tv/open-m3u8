using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public interface PlaylistParseState<T> : IParseState<T>
    {
        PlaylistParseState<T> setUnknownTags(List<String> unknownTags);
        PlaylistParseState<T> setStartData(StartData startData);
    }
}
