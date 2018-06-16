using System;
using System.Collections.Generic;
using System.Text;
// import java.util.List;
// import java.util.Objects;

namespace M3U8Parser
{
    public class IFrameStreamInfo : IStreamInfo
    {
        public const int NO_BANDWIDTH = -1;

        private readonly int mBandwidth;
        private readonly int mAverageBandwidth;
        private readonly List<String> mCodecs;
        private readonly Resolution mResolution;
        private readonly float mFrameRate;
        private readonly String mVideo;
        private readonly String mUri;

        private IFrameStreamInfo(
                int bandwidth,
                int averageBandwidth,
                List<String> codecs,
                Resolution resolution,
                float frameRate,
                String video,
                String uri)
        {
            mBandwidth = bandwidth;
            mAverageBandwidth = averageBandwidth;
            mCodecs = codecs;
            mResolution = resolution;
            mFrameRate = frameRate;
            mVideo = video;
            mUri = uri;
        }

        public int getBandwidth()
        {
            return mBandwidth;
        }

        public bool hasAverageBandwidth()
        {
            return mAverageBandwidth != NO_BANDWIDTH;
        }

        public int getAverageBandwidth()
        {
            return mAverageBandwidth;
        }

        public bool hasCodecs()
        {
            return mCodecs != null;
        }

        public List<String> getCodecs()
        {
            return mCodecs;
        }

        public bool hasResolution()
        {
            return mResolution != null;
        }

        public Resolution getResolution()
        {
            return mResolution;
        }

        public bool hasFrameRate()
        {
            return !float.IsNaN(mFrameRate);
        }

        public float getFrameRate()
        {
            return mFrameRate;
        }

        public bool hasVideo()
        {
            return mVideo != null;
        }

        public String getVideo()
        {
            return mVideo;
        }

        public String getUri()
        {
            return mUri;
        }

        public Builder buildUpon()
        {
            return new Builder(
                    mBandwidth,
                    mAverageBandwidth,
                    mCodecs,
                    mResolution,
                    mFrameRate,
                    mVideo,
                    mUri);
        }

        public override int GetHashCode()
        {
            // TODO: Implement
            //return Objects.hash(
            // mBandwidth,
            // mAverageBandwidth,
            // mCodecs,
            // mResolution,
            // mFrameRate,
            // mVideo,
            // mUri);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (!(o is IFrameStreamInfo))
            {
                return false;
            }

            IFrameStreamInfo other = (IFrameStreamInfo)o;

            return mBandwidth == other.mBandwidth &&
                   mAverageBandwidth == other.mAverageBandwidth &&
                   object.Equals(mCodecs, other.mCodecs) &&
                   object.Equals(mResolution, other.mResolution) &&
                   object.Equals(mFrameRate, other.mFrameRate) &&
                   object.Equals(mVideo, other.mVideo) &&
                   object.Equals(mUri, other.mUri);
        }

        public class Builder : StreamInfoBuilder
        {
            private int mBandwidth = NO_BANDWIDTH;
            private int mAverageBandwidth = NO_BANDWIDTH;
            private List<String> mCodecs;
            private Resolution mResolution;
            private float mFrameRate = float.NaN;
            private String mVideo;
            private String mUri;

            public Builder()
            {
            }

            public Builder(
                    int bandwidth,
                    int averageBandwidth,
                    List<String> codecs,
                    Resolution resolution,
                    float frameRate,
                    String video,
                    String uri)
            {
                mBandwidth = bandwidth;
                mAverageBandwidth = averageBandwidth;
                mCodecs = codecs;
                mResolution = resolution;
                mFrameRate = frameRate;
                mVideo = video;
                mUri = uri;
            }

            public StreamInfoBuilder withBandwidth(int bandwidth)
            {
                mBandwidth = bandwidth;
                return this;
            }

            public StreamInfoBuilder withAverageBandwidth(int averageBandwidth)
            {
                mAverageBandwidth = averageBandwidth;
                return this;
            }

            public StreamInfoBuilder withCodecs(List<String> codecs)
            {
                mCodecs = codecs;
                return this;
            }

            public StreamInfoBuilder withResolution(Resolution resolution)
            {
                mResolution = resolution;
                return this;
            }

            public StreamInfoBuilder withFrameRate(float frameRate)
            {
                mFrameRate = frameRate;
                return this;
            }

            public StreamInfoBuilder withVideo(String video)
            {
                mVideo = video;
                return this;
            }

            public StreamInfoBuilder withUri(String uri)
            {
                mUri = uri;
                return this;
            }

            public IFrameStreamInfo build()
            {
                return new IFrameStreamInfo(
                        mBandwidth,
                        mAverageBandwidth,
                        mCodecs,
                        mResolution,
                        mFrameRate,
                        mVideo,
                        mUri);
            }
        }
    }
}
