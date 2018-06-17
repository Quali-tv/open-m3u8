using System;
using System.Text;

namespace M3U8Parser
{
    public interface AttributeParser<Builder>
    {
        void parse(Attribute attribute, Builder builder, ParseState state);
    }
}
