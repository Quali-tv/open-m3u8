using System;
using System.Text;

namespace M3U8Parser
{
    public interface IExtTagParser : LineParser
    {
        String getTag();
        bool hasData();
    }
}
