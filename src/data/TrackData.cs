using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class TrackData
    {
        private readonly String mUri;
        private readonly TrackInfo mTrackInfo;
        private readonly EncryptionData mEncryptionData;
        private readonly String mProgramDateTime;
        private readonly bool mHasDiscontinuity;
        private readonly MapInfo mMapInfo;
        private readonly ByteRange mByteRange;

        private TrackData(String uri, TrackInfo trackInfo, EncryptionData encryptionData, String programDateTime, bool hasDiscontinuity, MapInfo mapInfo, ByteRange byteRange)
        {
            mUri = uri;
            mTrackInfo = trackInfo;
            mEncryptionData = encryptionData;
            mProgramDateTime = programDateTime;
            mHasDiscontinuity = hasDiscontinuity;
            mMapInfo = mapInfo;
            mByteRange = byteRange;
        }

        public String getUri()
        {
            return mUri;
        }

        public bool hasTrackInfo()
        {
            return mTrackInfo != null;
        }

        public TrackInfo getTrackInfo()
        {
            return mTrackInfo;
        }

        public bool hasEncryptionData()
        {
            return mEncryptionData != null;
        }

        public bool isEncrypted()
        {
            return hasEncryptionData() &&
                   mEncryptionData.getMethod() != null &&
                   mEncryptionData.getMethod() != EncryptionMethod.NONE;
        }

        public bool hasProgramDateTime()
        {
            return mProgramDateTime != null && mProgramDateTime.Length > 0;
        }

        public String getProgramDateTime()
        {
            return mProgramDateTime;
        }

        public bool hasDiscontinuity()
        {
            return mHasDiscontinuity;
        }

        public EncryptionData getEncryptionData()
        {
            return mEncryptionData;
        }

        public bool hasMapInfo()
        {
            return mMapInfo != null;
        }

        public MapInfo getMapInfo()
        {
            return mMapInfo;
        }

        public bool hasByteRange()
        {
            return mByteRange != null;
        }

        public ByteRange getByteRange()
        {
            return mByteRange;
        }

        public Builder buildUpon()
        {
            return new Builder(getUri(), mTrackInfo, mEncryptionData, mHasDiscontinuity, mMapInfo, mByteRange);
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (o == null || GetType() != o.GetType()) return false;
            TrackData trackData = (TrackData)o;
            return mHasDiscontinuity == trackData.mHasDiscontinuity &&
                   object.Equals(mUri, trackData.mUri) &&
                   object.Equals(mTrackInfo, trackData.mTrackInfo) &&
                   object.Equals(mEncryptionData, trackData.mEncryptionData) &&
                   object.Equals(mProgramDateTime, trackData.mProgramDateTime) &&
                   object.Equals(mMapInfo, trackData.mMapInfo) &&
                   object.Equals(mByteRange, trackData.mByteRange);
        }

        public override int GetHashCode()
        {
            // TODO: Implement
            //return Objects.hash(mUri, mTrackInfo, mEncryptionData, mProgramDateTime, mHasDiscontinuity, mMapInfo, mByteRange);
            return 0;
        }

        public override string ToString()
        {
            return "TrackData{" +
                    "mUri='" + mUri + '\'' +
                    ", mTrackInfo=" + mTrackInfo +
                    ", mEncryptionData=" + mEncryptionData +
                    ", mProgramDateTime='" + mProgramDateTime + '\'' +
                    ", mHasDiscontinuity=" + mHasDiscontinuity +
                    ", mMapInfo=" + mMapInfo +
                    ", mByteRange=" + mByteRange +
                    '}';
        }

        public class Builder
        {
            private String mUri;
            private TrackInfo mTrackInfo;
            private EncryptionData mEncryptionData;
            private String mProgramDateTime;
            private bool mHasDiscontinuity;
            private MapInfo mMapInfo;
            private ByteRange mByteRange;

            public Builder()
            {
            }

            public Builder(String uri, TrackInfo trackInfo, EncryptionData encryptionData, bool hasDiscontinuity, MapInfo mapInfo, ByteRange byteRange)
            {
                mUri = uri;
                mTrackInfo = trackInfo;
                mEncryptionData = encryptionData;
                mHasDiscontinuity = hasDiscontinuity;
                mMapInfo = mapInfo;
                mByteRange = byteRange;
            }

            public Builder withUri(String url)
            {
                mUri = url;
                return this;
            }

            public Builder withTrackInfo(TrackInfo trackInfo)
            {
                mTrackInfo = trackInfo;
                return this;
            }

            public Builder withEncryptionData(EncryptionData encryptionData)
            {
                mEncryptionData = encryptionData;
                return this;
            }

            public Builder withProgramDateTime(String programDateTime)
            {
                mProgramDateTime = programDateTime;
                return this;
            }

            public Builder withDiscontinuity(bool hasDiscontinuity)
            {
                mHasDiscontinuity = hasDiscontinuity;
                return this;
            }

            public Builder withMapInfo(MapInfo mapInfo)
            {
                mMapInfo = mapInfo;
                return this;
            }

            public Builder withByteRange(ByteRange byteRange)
            {
                mByteRange = byteRange;
                return this;
            }

            public TrackData build()
            {
                return new TrackData(mUri, mTrackInfo, mEncryptionData, mProgramDateTime, mHasDiscontinuity, mMapInfo, mByteRange);
            }
        }
    }
}
