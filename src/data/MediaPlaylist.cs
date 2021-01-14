using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class MediaPlaylist
    {
        private readonly List<TrackData> mTracks;
        private readonly List<String> mUnknownTags;
        private readonly int mTargetDuration;
        private readonly int? mDiscontinuitySequenceNumber;
        private readonly int mMediaSequenceNumber;
        private readonly bool mIsIframesOnly;
        private readonly bool mIsOngoing;
        private readonly PlaylistType mPlaylistType;
        private readonly StartData mStartData;

        private MediaPlaylist(List<TrackData> tracks, List<String> unknownTags, int targetDuration, StartData startData, int? discontinuitySequence, int mediaSequenceNumber, bool isIframesOnly, bool isOngoing, PlaylistType playlistType)
        {
            mTracks = DataUtil.emptyOrUnmodifiable(tracks);
            mUnknownTags = DataUtil.emptyOrUnmodifiable(unknownTags);
            mTargetDuration = targetDuration;
            mDiscontinuitySequenceNumber = discontinuitySequence;
            mMediaSequenceNumber = mediaSequenceNumber;
            mIsIframesOnly = isIframesOnly;
            mIsOngoing = isOngoing;
            mStartData = startData;
            mPlaylistType = playlistType;
        }

        public bool hasTracks()
        {
            return !mTracks.isEmpty();
        }

        public List<TrackData> getTracks()
        {
            return mTracks;
        }

        public int getTargetDuration()
        {
            return mTargetDuration;
        }

        public int getDiscontinuitySequenceNumber()
        {
            return mDiscontinuitySequenceNumber.GetValueOrDefault();
        }

        public int getMediaSequenceNumber()
        {
            return mMediaSequenceNumber;
        }

        public bool isIframesOnly()
        {
            return mIsIframesOnly;
        }

        public bool isOngoing()
        {
            return mIsOngoing;
        }

        public bool hasUnknownTags()
        {
            return !mUnknownTags.isEmpty();
        }

        public List<String> getUnknownTags()
        {
            return mUnknownTags;
        }

        public StartData getStartData()
        {
            return mStartData;
        }

        public bool hasStartData()
        {
            return mStartData != null;
        }

        public PlaylistType getPlaylistType()
        {
            return mPlaylistType;
        }

        public bool hasPlaylistType()
        {
            return mPlaylistType != null;
        }

        public Builder buildUpon()
        {
            return new Builder(mTracks, mUnknownTags, mTargetDuration, mMediaSequenceNumber, mIsIframesOnly, mIsOngoing, mPlaylistType, mStartData);
        }

        public override string ToString()
        {
            return new StringBuilder()
                    .Append("(MediaPlaylist")
                    .Append(" mTracks=").Append(mTracks)
                    .Append(" mUnknownTags=").Append(mUnknownTags)
                    .Append(" mTargetDuration=").Append(mTargetDuration)
                    .Append(" mDiscontinuitySequenceNumber=").Append(mDiscontinuitySequenceNumber)
                    .Append(" mMediaSequenceNumber=").Append(mMediaSequenceNumber)
                    .Append(" mIsIframesOnly=").Append(mIsIframesOnly)
                    .Append(" mIsOngoing=").Append(mIsOngoing)
                    .Append(" mPlaylistType=").Append(mPlaylistType)
                    .Append(" mStartData=").Append(mStartData)
                    .Append(")")
                    .ToString();
        }

        public class Builder
        {
            private List<TrackData> mTracks;
            private List<String> mUnknownTags;
            private int mTargetDuration;
            private int mMediaSequenceNumber;
            private int? mDiscontinuitySequenceNumber;
            private bool mIsIframesOnly;
            private bool mIsOngoing;
            private PlaylistType mPlaylistType;
            private StartData mStartData;

            public Builder()
            {
            }

            public Builder(List<TrackData> tracks, List<String> unknownTags, int targetDuration, int mediaSequenceNumber, bool isIframesOnly, bool isOngoing, PlaylistType playlistType, StartData startData)
            {
                mTracks = tracks;
                mUnknownTags = unknownTags;
                mTargetDuration = targetDuration;
                mMediaSequenceNumber = mediaSequenceNumber;
                mIsIframesOnly = isIframesOnly;
                mIsOngoing = isOngoing;
                mPlaylistType = playlistType;
                mStartData = startData;
            }

            public Builder withTracks(List<TrackData> tracks)
            {
                mTracks = tracks;
                return this;
            }

            public Builder withUnknownTags(List<String> unknownTags)
            {
                mUnknownTags = unknownTags;
                return this;
            }

            public Builder withTargetDuration(int targetDuration)
            {
                mTargetDuration = targetDuration;
                return this;
            }

            public Builder withStartData(StartData startData)
            {
                mStartData = startData;
                return this;
            }

            public Builder withMediaSequenceNumber(int mediaSequenceNumber)
            {
                mMediaSequenceNumber = mediaSequenceNumber;
                return this;
            }

            public Builder withDiscontinuitySequenceNumber(int discontinuitySequence)
            {
                mDiscontinuitySequenceNumber = discontinuitySequence;
                return this;
            }

            public Builder withIsIframesOnly(bool isIframesOnly)
            {
                mIsIframesOnly = isIframesOnly;
                return this;
            }

            public Builder withIsOngoing(bool isOngoing)
            {
                mIsOngoing = isOngoing;
                return this;
            }

            public Builder withPlaylistType(PlaylistType playlistType)
            {
                mPlaylistType = playlistType;
                return this;
            }

            public MediaPlaylist build()
            {
                return new MediaPlaylist(mTracks, mUnknownTags, mTargetDuration, mStartData, mDiscontinuitySequenceNumber, mMediaSequenceNumber, mIsIframesOnly, mIsOngoing, mPlaylistType);
            }
        }
    }
}
