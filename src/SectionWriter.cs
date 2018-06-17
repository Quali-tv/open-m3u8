using System;
using System.Text;

namespace M3U8Parser
{
    public interface SectionWriter
    {
        void write(TagWriter tagWriter, Playlist playlist);
    }
}
