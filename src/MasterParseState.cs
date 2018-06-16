using System;
using System.Collections.Generic;
using System.Text;
//import java.util.ArrayList;
//import java.util.List;
//
//import com.iheartradio.m3u8.data.IFrameStreamInfo;
//import com.iheartradio.m3u8.data.MasterPlaylist;
//import com.iheartradio.m3u8.data.MediaData;
//import com.iheartradio.m3u8.data.PlaylistData;
//import com.iheartradio.m3u8.data.StartData;
//import com.iheartradio.m3u8.data.StreamInfo;

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

        public MasterPlaylist buildPlaylist() //throws ParseException 
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
