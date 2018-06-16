using System;
using System.Collections.Generic;
using System.Text;
// import java.util.Objects;

namespace M3U8Parser
{
    public class TrackInfo
    {
        public readonly float duration;
        public readonly String title;

        public TrackInfo(float duration, String title)
        {
            this.duration = duration;
            this.title = title ?? string.Empty;
        }

        public override int GetHashCode()
        {
            // TODO: Implement
            //return Objects.hash(duration, title);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (!(o is TrackInfo))
            {
                return false;
            }

            TrackInfo other = (TrackInfo)o;

            return this.duration == other.duration &&
                   object.Equals(this.title, other.title);
        }
    }
}
