using System;
using System.Text;

namespace M3U8Parser
{
    public interface LineParser
    {
        void parse(String line, ParseState state); //throws ParseException
    }
}
