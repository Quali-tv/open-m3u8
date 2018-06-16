using System;
using System.Text;

namespace M3U8Parser
{
    public interface AttributeWriter<T>
    {
        String write(T attributes); // throws ParseException;
        bool containsAttribute(T attributes);
    }
}
