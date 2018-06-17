using System;
using System.Text;

namespace M3U8Parser
{
    public interface AttributeWriter<T>
    {
        String write(T attributes);
        bool containsAttribute(T attributes);
    }
}
