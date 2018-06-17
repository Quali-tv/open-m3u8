using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class Resolution
    {
        public readonly int width;
        public readonly int height;

        public Resolution(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override int GetHashCode()
        {
            // TODO: Implement
            //return Objects.hash(height, width);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (!(o is Resolution))
            {
                return false;
            }

            Resolution other = (Resolution)o;

            return width == other.width &&
                   height == other.height;
        }
    }
}
