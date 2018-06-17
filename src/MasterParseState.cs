using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class MasterParseState : PlaylistParseState<MasterPlaylist>
    {
        private List<String> mUnknownTags;
        private StartData mStartData;

        public readonly List<PlaylistData> playlists = new List<PlaylistData>();
        public readonly List<IFrameStreamInfo> iFramePlaylists = new List<IFrameStreamInfo>();
        public readonly List<MediaData> mediaData = new List<MediaData>();

        public StreamInfo streamInfo;

        public bool isDefault;
        public bool isNotAutoSelect;

        public void clearMediaDataState()
        {
            isDefault = false;
            isNotAutoSelect = false;
        }

        public PlaylistParseState<MasterPlaylist> setUnknownTags(List<String> unknownTags)
        {
            mUnknownTags = unknownTags;
            return this;
        }

        public PlaylistParseState<MasterPlaylist> setStartData(StartData startData)
        {
            mStartData = startData;
            return this;
        }

        public MasterPlaylist buildPlaylist()
        {
            return new MasterPlaylist.Builder()
                    .withPlaylists(playlists)
                    .withIFramePlaylists(iFramePlaylists)
                    .withMediaData(mediaData)
                    .withUnknownTags(mUnknownTags)
                    .withStartData(mStartData)
                    .build();
        }
    }
}
