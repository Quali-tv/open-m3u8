using System;
using System.Collections.Generic;
using System.Text;

// package com.iheartradio.m3u8.data;

// import java.util.List;
// import java.util.Objects;
namespace M3U8Parser
{


    public class StreamInfo : IStreamInfo
    {
        public const int NO_BANDWIDTH = -1;

        private readonly int mBandwidth;
        private readonly int mAverageBandwidth;
        private readonly List<String> mCodecs;
        private readonly Resolution mResolution;
        private readonly float mFrameRate;
        private readonly String mAudio;
        private readonly String mVideo;
        private readonly String mSubtitles;
        private readonly String mClosedCaptions;

        private StreamInfo(
                int bandwidth,
                int averageBandwidth,
                List<String> codecs,
                Resolution resolution,
                float frameRate,
                String audio,
                String video,
                String subtitles,
                String closedCaptions)
        {
            mBandwidth = bandwidth;
            mAverageBandwidth = averageBandwidth;
            mCodecs = codecs;
            mResolution = resolution;
            mFrameRate = frameRate;
            mAudio = audio;
            mVideo = video;
            mSubtitles = subtitles;
            mClosedCaptions = closedCaptions;
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

        public bool hasAudio()
        {
            return mAudio != null;
        }

        public String getAudio()
        {
            return mAudio;
        }

        public bool hasVideo()
        {
            return mVideo != null;
        }

        public String getVideo()
        {
            return mVideo;
        }

        public bool hasSubtitles()
        {
            return mSubtitles != null;
        }

        public String getSubtitles()
        {
            return mSubtitles;
        }

        public bool hasClosedCaptions()
        {
            return mClosedCaptions != null;
        }

        public String getClosedCaptions()
        {
            return mClosedCaptions;
        }

        public Builder buildUpon()
        {
            return new Builder(
                    mBandwidth,
                    mAverageBandwidth,
                    mCodecs,
                    mResolution,
                    mFrameRate,
                    mAudio,
                    mVideo,
                    mSubtitles,
                    mClosedCaptions);
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
            // mAudio,
            // mVideo,
            // mSubtitles,
            // mClosedCaptions);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (!(o is StreamInfo))
            {
                return false;
            }

            StreamInfo other = (StreamInfo)o;

            return mBandwidth == other.mBandwidth &&
                   mAverageBandwidth == other.mAverageBandwidth &&
                   mCodecs.SequenceEquals(other.mCodecs) &&
                   object.Equals(mResolution, other.mResolution) &&
                   object.Equals(mFrameRate, other.mFrameRate) &&
                   object.Equals(mAudio, other.mAudio) &&
                   object.Equals(mVideo, other.mVideo) &&
                   object.Equals(mSubtitles, other.mSubtitles) &&
                   object.Equals(mClosedCaptions, other.mClosedCaptions);
        }

        public class Builder : StreamInfoBuilder
        {
            private int mBandwidth = NO_BANDWIDTH;
            private int mAverageBandwidth = NO_BANDWIDTH;
            private List<String> mCodecs;
            private Resolution mResolution;
            private float mFrameRate = float.NaN;
            private String mAudio;
            private String mVideo;
            private String mSubtitles;
            private String mClosedCaptions;

            public Builder()
            {
            }

            public Builder(
                    int bandwidth,
                    int averageBandwidth,
                    List<String> codecs,
                    Resolution resolution,
                    float frameRate,
                    String audio,
                    String video,
                    String subtitles,
                    String closedCaptions)
            {
                mBandwidth = bandwidth;
                mAverageBandwidth = averageBandwidth;
                mCodecs = codecs;
                mResolution = resolution;
                mFrameRate = frameRate;
                mAudio = audio;
                mVideo = video;
                mSubtitles = subtitles;
                mClosedCaptions = closedCaptions;
            }

            public Builder withBandwidth(int bandwidth)
            {
                mBandwidth = bandwidth;
                return this;
            }

            public Builder withAverageBandwidth(int averageBandwidth)
            {
                mAverageBandwidth = averageBandwidth;
                return this;
            }

            public Builder withCodecs(List<String> codecs)
            {
                mCodecs = codecs;
                return this;
            }

            public Builder withResolution(Resolution resolution)
            {
                mResolution = resolution;
                return this;
            }

            public Builder withFrameRate(float frameRate)
            {
                mFrameRate = frameRate;
                return this;
            }

            public Builder withAudio(String audio)
            {
                mAudio = audio;
                return this;
            }

            public Builder withVideo(String video)
            {
                mVideo = video;
                return this;
            }

            public Builder withSubtitles(String subtitles)
            {
                mSubtitles = subtitles;
                return this;
            }

            public Builder withClosedCaptions(String closedCaptions)
            {
                mClosedCaptions = closedCaptions;
                return this;
            }

            public StreamInfo build()
            {
                return new StreamInfo(
                        mBandwidth,
                        mAverageBandwidth,
                        mCodecs,
                        mResolution,
                        mFrameRate,
                        mAudio,
                        mVideo,
                        mSubtitles,
                        mClosedCaptions);
            }

            StreamInfoBuilder StreamInfoBuilder.withBandwidth(int bandwidth) 
                => this.withBandwidth(bandwidth);
            
            StreamInfoBuilder StreamInfoBuilder.withAverageBandwidth(int averageBandwidth) 
                => this.withAverageBandwidth(averageBandwidth);

            StreamInfoBuilder StreamInfoBuilder.withCodecs(List<string> codecs) 
                => this.withCodecs(codecs);

            StreamInfoBuilder StreamInfoBuilder.withResolution(Resolution resolution) 
                => this.withResolution(resolution);

            StreamInfoBuilder StreamInfoBuilder.withFrameRate(float frameRate) 
                => this.withFrameRate(frameRate);

            StreamInfoBuilder StreamInfoBuilder.withVideo(string video) 
                => this.withVideo(video);
        }
    }
}
