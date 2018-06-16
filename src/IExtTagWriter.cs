using System;
using System.Text;

namespace M3U8Parser
{
    public interface IExtTagWriter : SectionWriter
    {
        String getTag();
    }
}
