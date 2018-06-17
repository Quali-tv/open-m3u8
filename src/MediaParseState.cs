using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class MediaParseState : PlaylistParseState<MediaPlaylist>
    {
        private List<String> mUnknownTags;
        private StartData mStartData;

        public readonly List<TrackData> tracks = new List<TrackData>();

        public int? targetDuration;
        public int? mediaSequenceNumber;
        public bool isIframesOnly;
        public PlaylistType playlistType;
        public TrackInfo trackInfo;
        public EncryptionData encryptionData;
        public String programDateTime;
        public bool endOfList;
        public bool hasDiscontinuity;
        public MapInfo mapInfo;
        public ByteRange byteRange;

        public PlaylistParseState<MediaPlaylist> setUnknownTags(List<String> unknownTags)
        {
            mUnknownTags = unknownTags;
            return this;
        }

        public PlaylistParseState<MediaPlaylist> setStartData(StartData startData)
        {
            mStartData = startData;
            return this;
        }

        public MediaPlaylist buildPlaylist()
        {
            return new MediaPlaylist.Builder()
                    .withTracks(tracks)
                    .withUnknownTags(mUnknownTags)
                    .withTargetDuration(targetDuration.HasValue ? targetDuration.Value : maximumDuration(tracks, 0))
                    .withIsIframesOnly(isIframesOnly)
                    .withStartData(mStartData)
                    .withIsOngoing(!endOfList)
                    .withMediaSequenceNumber(mediaSequenceNumber.HasValue ? mediaSequenceNumber.Value : 0)
                    .withPlaylistType(playlistType)
                    .build();
        }

        private static int maximumDuration(List<TrackData> tracks, float minValue)
        {
            float max = minValue;

            foreach (TrackData trackData in tracks)
            {
                if (trackData.hasTrackInfo())
                {
                    max = Math.Max(max, trackData.getTrackInfo().duration);
                }
            }

            return 0;
        }
    }
}
