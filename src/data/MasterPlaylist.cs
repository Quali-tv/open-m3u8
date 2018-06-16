using System;
using System.Collections.Generic;
using System.Text;


//import java.util.List;
//import java.util.Objects;
namespace M3U8Parser
{

public class MasterPlaylist {
    private readonly List<PlaylistData> mPlaylists;
    private readonly List<IFrameStreamInfo> mIFramePlaylists;
    private readonly List<MediaData> mMediaData;
    private readonly List<String> mUnknownTags;
    private readonly StartData mStartData;

    private MasterPlaylist(List<PlaylistData> playlists, List<IFrameStreamInfo> iFramePlaylists, List<MediaData> mediaData, List<String> unknownTags, StartData startData) {
        mPlaylists = DataUtil.emptyOrUnmodifiable(playlists);
        mIFramePlaylists = DataUtil.emptyOrUnmodifiable(iFramePlaylists);
        mMediaData = DataUtil.emptyOrUnmodifiable(mediaData);
        mUnknownTags = DataUtil.emptyOrUnmodifiable(unknownTags);
        mStartData = startData;
    }

    public List<PlaylistData> getPlaylists() {
        return mPlaylists;
    }

    public List<IFrameStreamInfo> getIFramePlaylists() {
        return mIFramePlaylists;
    }

    public List<MediaData> getMediaData() {
        return mMediaData;
    }
   
    public bool hasUnknownTags() {
        return mUnknownTags.Count > 0;
    }
    
    public List<String> getUnknownTags() {
        return mUnknownTags;
    }

    public bool hasStartData() {
        return mStartData != null;
    }

    public StartData getStartData() {
        return mStartData;
    }

    public Builder buildUpon() {
        return new Builder(mPlaylists, mIFramePlaylists, mMediaData, mUnknownTags);
    }
    
    public override int GetHashCode() {
        // TODO: Implement
        //return Objects.hash(mMediaData, mPlaylists, mIFramePlaylists, mUnknownTags, mStartData);
        return 0;
    }

    public override bool Equals(object o) {
        if (!(o is MasterPlaylist)) {
            return false;
        }

        MasterPlaylist other = (MasterPlaylist) o;
        
        return mMediaData.SequenceEquals(other.mMediaData) &&
               mPlaylists.SequenceEquals(other.mPlaylists) &&
               mIFramePlaylists.SequenceEquals(other.mIFramePlaylists) &&
               mUnknownTags.SequenceEquals(other.mUnknownTags) &&
               object.Equals(mStartData, other.mStartData);
    }

    public override string ToString() {
        return new StringBuilder()
                .Append("(MasterPlaylist")
                .Append(" mPlaylists=").Append(mPlaylists.ToString())
                .Append(" mIFramePlaylists=").Append(mIFramePlaylists.ToString())
                .Append(" mMediaData=").Append(mMediaData.ToString())
                .Append(" mUnknownTags=").Append(mUnknownTags.ToString())
                .Append(" mStartData=").Append(mStartData.ToString())
                .Append(")")
                .ToString();
    }

    public class Builder {
        private List<PlaylistData> mPlaylists;
        private List<IFrameStreamInfo> mIFramePlaylists;
        private List<MediaData> mMediaData;
        private List<String> mUnknownTags;
        private StartData mStartData;

        public Builder() {
        }

        public Builder(List<PlaylistData> playlists, List<IFrameStreamInfo> iFramePlaylists, List<MediaData> mediaData, List<String> unknownTags) {
            mPlaylists = playlists;
            mIFramePlaylists = iFramePlaylists;
            mMediaData = mediaData;
            mUnknownTags = unknownTags;
        }

        public Builder(List<PlaylistData> playlists, List<MediaData> mediaData) {
            mPlaylists = playlists;
            mMediaData = mediaData;
        }

        public Builder withPlaylists(List<PlaylistData> playlists) {
            mPlaylists = playlists;
            return this;
        }

        public Builder withIFramePlaylists(List<IFrameStreamInfo> iFramePlaylists) {
            mIFramePlaylists = iFramePlaylists;
            return this;
        }

        public Builder withMediaData(List<MediaData> mediaData) {
            mMediaData = mediaData;
            return this;
        }
        
        public Builder withUnknownTags(List<String> unknownTags) {
            mUnknownTags = unknownTags;
            return this;
        }

        public Builder withStartData(StartData startData) {
            mStartData = startData;
            return this;
        }

        public MasterPlaylist build() {
            return new MasterPlaylist(mPlaylists, mIFramePlaylists, mMediaData, mUnknownTags, mStartData);
        }
    }
}
}
