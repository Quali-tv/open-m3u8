using System;
using System.Collections.Generic;
using System.Text;
// import java.util.List;
// import java.util.Objects;


namespace M3U8Parser
{
public class MediaPlaylist {
    private readonly List<TrackData> mTracks;
    private readonly List<String> mUnknownTags;
    private readonly int mTargetDuration;
    private readonly int mMediaSequenceNumber;
    private readonly bool mIsIframesOnly;
    private readonly bool mIsOngoing;
    private readonly PlaylistType mPlaylistType;
    private readonly StartData mStartData;

    private MediaPlaylist(List<TrackData> tracks, List<String> unknownTags, int targetDuration, StartData startData, int mediaSequenceNumber, bool isIframesOnly, bool isOngoing, PlaylistType playlistType) {
        mTracks = DataUtil.emptyOrUnmodifiable(tracks);
        mUnknownTags = DataUtil.emptyOrUnmodifiable(unknownTags);
        mTargetDuration = targetDuration;
        mMediaSequenceNumber = mediaSequenceNumber;
        mIsIframesOnly = isIframesOnly;
        mIsOngoing = isOngoing;
        mStartData = startData;
        mPlaylistType = playlistType;
    }

    public bool hasTracks() {
        return !mTracks.isEmpty();
    }

    public List<TrackData> getTracks() {
        return mTracks;
    }

    public int getTargetDuration() {
        return mTargetDuration;
    }

    public int getMediaSequenceNumber() {
        return mMediaSequenceNumber;
    }
    
    public bool isIframesOnly() {
        return mIsIframesOnly;
    }

    public bool isOngoing() {
        return mIsOngoing;
    }

    public bool hasUnknownTags() {
        return !mUnknownTags.isEmpty();
    }
    
    public List<String> getUnknownTags() {
        return mUnknownTags;
    }
    
    public StartData getStartData() {
        return mStartData;
    }
    
    public bool hasStartData() {
        return mStartData != null;
    }

    public PlaylistType getPlaylistType() {
        return mPlaylistType;
    }
    
    public bool hasPlaylistType() {
        return mPlaylistType != null;
    }

    public int getDiscontinuitySequenceNumber(int segmentIndex) {
        if (segmentIndex < 0 || segmentIndex >= mTracks.Count) {
            throw new IndexOutOfRangeException();
        }

        int discontinuitySequenceNumber = 0;

        for (int i = 0; i <= segmentIndex; ++i) {
            if (mTracks[i].hasDiscontinuity()) {
                ++discontinuitySequenceNumber;
            }
        }

        return discontinuitySequenceNumber;
    }

    public Builder buildUpon() {
        return new Builder(mTracks, mUnknownTags, mTargetDuration, mMediaSequenceNumber, mIsIframesOnly, mIsOngoing, mPlaylistType, mStartData);
    }
    
    public override int GetHashCode() {
        // TODO: Implement 
        //return Objects.hash(
                // mTracks,
                // mUnknownTags,
                // mTargetDuration,
                // mMediaSequenceNumber,
                // mIsIframesOnly,
                // mIsOngoing,
                // mPlaylistType,
                // mStartData);
        return 0;
    }

    public override bool Equals(object o) {
        if (!(o is MediaPlaylist)) {
            return false;
        }

        MediaPlaylist other = (MediaPlaylist) o;

        return mTracks.SequenceEquals(other.mTracks) &&
               mUnknownTags.SequenceEquals(other.mUnknownTags) &&
               mTargetDuration == other.mTargetDuration &&
               mMediaSequenceNumber == other.mMediaSequenceNumber &&
               mIsIframesOnly == other.mIsIframesOnly &&
               mIsOngoing == other.mIsOngoing &&
               object.Equals(mPlaylistType, other.mPlaylistType) &&
               object.Equals(mStartData, other.mStartData);
    }

    public override string ToString() {
        return new StringBuilder()
                .Append("(MediaPlaylist")
                .Append(" mTracks=").Append(mTracks)
                .Append(" mUnknownTags=").Append(mUnknownTags)
                .Append(" mTargetDuration=").Append(mTargetDuration)
                .Append(" mMediaSequenceNumber=").Append(mMediaSequenceNumber)
                .Append(" mIsIframesOnly=").Append(mIsIframesOnly)
                .Append(" mIsOngoing=").Append(mIsOngoing)
                .Append(" mPlaylistType=").Append(mPlaylistType)
                .Append(" mStartData=").Append(mStartData)
                .Append(")")
                .ToString();
    }

    public class Builder {
        private List<TrackData> mTracks;
        private List<String> mUnknownTags;
        private int mTargetDuration;
        private int mMediaSequenceNumber;
        private bool mIsIframesOnly;
        private bool mIsOngoing;
        private PlaylistType mPlaylistType;
        private StartData mStartData;

        public Builder() {
        }

        public Builder(List<TrackData> tracks, List<String> unknownTags, int targetDuration, int mediaSequenceNumber, bool isIframesOnly, bool isOngoing, PlaylistType playlistType, StartData startData) {
            mTracks = tracks;
            mUnknownTags = unknownTags;
            mTargetDuration = targetDuration;
            mMediaSequenceNumber = mediaSequenceNumber;
            mIsIframesOnly = isIframesOnly;
            mIsOngoing = isOngoing;
            mPlaylistType = playlistType;
            mStartData = startData;
        }

        public Builder withTracks(List<TrackData> tracks) {
            mTracks = tracks;
            return this;
        }
        
        public Builder withUnknownTags(List<String> unknownTags) {
            mUnknownTags = unknownTags;
            return this;
        }

        public Builder withTargetDuration(int targetDuration) {
            mTargetDuration = targetDuration;
            return this;
        }

        public Builder withStartData(StartData startData) {
            mStartData = startData;
            return this;
        }
        
        public Builder withMediaSequenceNumber(int mediaSequenceNumber) {
            mMediaSequenceNumber = mediaSequenceNumber;
            return this;
        }
        
        public Builder withIsIframesOnly(bool isIframesOnly) {
            mIsIframesOnly = isIframesOnly;
            return this;
        }

        public Builder withIsOngoing(bool isOngoing) {
            mIsOngoing = isOngoing;
            return this;
        }

        public Builder withPlaylistType(PlaylistType playlistType) {
            mPlaylistType = playlistType;
            return this;
        }

        public MediaPlaylist build() {
            return new MediaPlaylist(mTracks, mUnknownTags, mTargetDuration, mStartData, mMediaSequenceNumber, mIsIframesOnly, mIsOngoing, mPlaylistType);
        }
    }
}
}
