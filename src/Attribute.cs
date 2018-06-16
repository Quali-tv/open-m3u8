using System;
using System.Text;

namespace M3U8Parser
{
    public class Attribute {
        public readonly String name;
        public readonly String value;

        public Attribute(String name, String value) {
            this.name = name;
            this.value = value;
        }

        public override string ToString() {
            return new StringBuilder()
                    .Append("(Attribute")
                    .Append(" name=").Append(name)
                    .Append(" value=").Append(value)
                    .Append(")").ToString();
        }
    }
}
