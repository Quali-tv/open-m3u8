using System;
using System.Text;

namespace M3U8Parser
{
    public class MapInfo
    {
        private readonly String uri;
        private readonly ByteRange byteRange;

        public MapInfo(String uri, ByteRange byteRange)
        {
            this.uri = uri;
            this.byteRange = byteRange;
        }

        public String getUri()
        {
            return uri;
        }

        public bool hasByteRange()
        {
            return byteRange != null;
        }

        public ByteRange getByteRange()
        {
            return byteRange;
        }

        public Builder buildUpon()
        {
            return new Builder(uri, byteRange);
        }

        public override string ToString()
        {
            return "MapInfo{" +
                    "uri='" + uri + '\'' +
                    ", byteRange='" + byteRange + '\'' +
                    '}';
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (o == null || GetType() != o.GetType()) return false;
            MapInfo mapInfo = (MapInfo)o;
            return object.Equals(uri, mapInfo.uri) &&
                    object.Equals(byteRange, mapInfo.byteRange);
        }

        public override int GetHashCode()
        {
            // TODO: Implement
            //return Objects.hash(uri, byteRange);
            return 0;
        }

        public class Builder
        {
            private String mUri;
            private ByteRange mByteRange;

            public Builder()
            {
            }

            public Builder(String uri, ByteRange byteRange)
            {
                this.mUri = uri;
                this.mByteRange = byteRange;
            }

            public Builder withUri(String uri)
            {
                this.mUri = uri;
                return this;
            }

            public Builder withByteRange(ByteRange byteRange)
            {
                this.mByteRange = byteRange;
                return this;
            }

            public MapInfo build()
            {
                return new MapInfo(mUri, mByteRange);
            }
        }
    }
}
