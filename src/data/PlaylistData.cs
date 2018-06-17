using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class PlaylistData
    {
        private readonly String mUri;
        private readonly StreamInfo mStreamInfo;

        private PlaylistData(String uri, StreamInfo streamInfo)
        {
            mUri = uri;
            mStreamInfo = streamInfo;
        }

        public String getUri()
        {
            return mUri;
        }

        public bool hasStreamInfo()
        {
            return mStreamInfo != null;
        }

        public StreamInfo getStreamInfo()
        {
            return mStreamInfo;
        }

        public Builder buildUpon()
        {
            return new Builder(mUri, mStreamInfo);
        }

        public override int GetHashCode()
        {
            // TODO: Implement
            //return Objects.hash(mUri, mStreamInfo);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (!(o is PlaylistData))
            {
                return false;
            }

            PlaylistData other = (PlaylistData)o;
            return object.Equals(mUri, other.mUri) && object.Equals(mStreamInfo, other.mStreamInfo);
        }

        public override string ToString()
        {
            return "PlaylistData [mStreamInfo=" + mStreamInfo
                    + ", mUri=" + mUri + "]";
        }

        public class Builder
        {
            private String mUri;
            private StreamInfo mStreamInfo;

            public Builder()
            {
            }

            public Builder(String uri, StreamInfo streamInfo)
            {
                mUri = uri;
                mStreamInfo = streamInfo;
            }

            public Builder withPath(String path)
            {
                mUri = path;
                return this;
            }

            public Builder withUri(String uri)
            {
                mUri = uri;
                return this;
            }

            public Builder withStreamInfo(StreamInfo streamInfo)
            {
                mStreamInfo = streamInfo;
                return this;
            }

            public PlaylistData build()
            {
                return new PlaylistData(mUri, mStreamInfo);
            }
        }
    }
}
