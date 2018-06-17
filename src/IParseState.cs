using System;
using System.Text;

namespace M3U8Parser
{
    public interface IParseState<T>
    {
        T buildPlaylist();
    }
}
