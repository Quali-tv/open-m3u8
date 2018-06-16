using System;
using System.Collections.Generic;
using System.Text;
// import com.iheartradio.m3u8.data.*;

// import java.util.Collections;
// import java.util.HashSet;
// import java.util.List;
// import java.util.Set;

namespace M3U8Parser
{
    public class PlaylistValidation
    {
        private readonly HashSet<PlaylistError> mErrors;

        private PlaylistValidation(HashSet<PlaylistError> errors)
        {
            mErrors = errors; // TODO: Collections.unmodifiableSet(errors);
        }

        public override string ToString()
        {
            return new StringBuilder()
                    .Append("(PlaylistValidation")
                    .Append(" valid=").Append(isValid())
                    .Append(" errors=").Append(mErrors)
                    .Append(")")
                    .ToString();
        }

        public bool isValid()
        {
            return mErrors.isEmpty();
        }

        public HashSet<PlaylistError> getErrors()
        {
            return mErrors;
        }

        /**
         * Equivalent to: PlaylistValidation.from(playlist, ParsingMode.STRICT)
         */
        public static PlaylistValidation from(Playlist playlist)
        {
            return PlaylistValidation.from(playlist, ParsingMode.STRICT);
        }

        public static PlaylistValidation from(Playlist playlist, ParsingMode parsingMode)
        {
            HashSet<PlaylistError> errors = new HashSet<PlaylistError>();

            if (playlist == null)
            {
                errors.Add(PlaylistError.NO_PLAYLIST);
                return new PlaylistValidation(errors);
            }

            if (playlist.getCompatibilityVersion() < Playlist.MIN_COMPATIBILITY_VERSION)
            {
                errors.Add(PlaylistError.COMPATIBILITY_TOO_LOW);
            }

            if (hasNoPlaylistTypes(playlist))
            {
                errors.Add(PlaylistError.NO_MASTER_OR_MEDIA);
            }
            else if (hasBothPlaylistTypes(playlist))
            {
                errors.Add(PlaylistError.BOTH_MASTER_AND_MEDIA);
            }

            if (playlist.hasMasterPlaylist())
            {
                if (!playlist.isExtended())
                {
                    errors.Add(PlaylistError.MASTER_NOT_EXTENDED);
                }

                addMasterPlaylistErrors(playlist.getMasterPlaylist(), errors);
            }

            if (playlist.hasMediaPlaylist())
            {
                addMediaPlaylistErrors(playlist.getMediaPlaylist(), errors, playlist.isExtended(), parsingMode);
            }

            return new PlaylistValidation(errors);
        }

        private static bool hasNoPlaylistTypes(Playlist playlist)
        {
            return !(playlist.hasMasterPlaylist() || playlist.hasMediaPlaylist());
        }

        private static bool hasBothPlaylistTypes(Playlist playlist)
        {
            return playlist.hasMasterPlaylist() && playlist.hasMediaPlaylist();
        }

        private static void addMasterPlaylistErrors(MasterPlaylist playlist, HashSet<PlaylistError> errors)
        {
            foreach (PlaylistData playlistData in playlist.getPlaylists())
            {
                addPlaylistDataErrors(playlistData, errors);
            }

            foreach (IFrameStreamInfo iFrameStreamInfo in playlist.getIFramePlaylists())
            {
                addIFrameStreamInfoErrors(iFrameStreamInfo, errors);
            }

            foreach (MediaData mediaData in playlist.getMediaData())
            {
                addMediaDataErrors(mediaData, errors);
            }
        }

        private static void addMediaPlaylistErrors(MediaPlaylist playlist, HashSet<PlaylistError> errors, bool isExtended, ParsingMode parsingMode)
        {
            if (isExtended && playlist.hasStartData())
            {
                addStartErrors(playlist.getStartData(), errors);
            }

            addByteRangeErrors(playlist.getTracks(), errors, parsingMode);

            foreach (TrackData trackData in playlist.getTracks())
            {
                addTrackDataErrors(trackData, errors, isExtended, parsingMode);
            }
        }

        private static void addByteRangeErrors(List<TrackData> tracks, HashSet<PlaylistError> errors, ParsingMode parsingMode)
        {
            HashSet<String> knownOffsets = new HashSet<String>();
            foreach (TrackData track in tracks)
            {
                if (!track.hasByteRange())
                {
                    continue;
                }

                if (track.getByteRange().hasOffset())
                {
                    knownOffsets.Add(track.getUri());
                }
                else if (!knownOffsets.Contains(track.getUri()))
                {
                    errors.Add(PlaylistError.BYTERANGE_WITH_UNDEFINED_OFFSET);
                }
            }
        }

        private static void addStartErrors(StartData startData, HashSet<PlaylistError> errors)
        {
            if (float.IsNaN(startData.getTimeOffset()))
            {
                errors.Add(PlaylistError.START_DATA_WITHOUT_TIME_OFFSET);
            }
        }

        private static void addPlaylistDataErrors(PlaylistData playlistData, HashSet<PlaylistError> errors)
        {
            if (playlistData.getUri() == null || playlistData.getUri().isEmpty())
            {
                errors.Add(PlaylistError.PLAYLIST_DATA_WITHOUT_URI);
            }


            if (playlistData.hasStreamInfo())
            {
                if (playlistData.getStreamInfo().getBandwidth() == StreamInfo.NO_BANDWIDTH)
                {
                    errors.Add(PlaylistError.STREAM_INFO_WITH_NO_BANDWIDTH);
                }

                if (playlistData.getStreamInfo().getAverageBandwidth() < StreamInfo.NO_BANDWIDTH)
                {
                    errors.Add(PlaylistError.STREAM_INFO_WITH_INVALID_AVERAGE_BANDWIDTH);
                }
            }
        }

        private static void addIFrameStreamInfoErrors(IFrameStreamInfo streamInfo, HashSet<PlaylistError> errors)
        {
            if (streamInfo.getUri() == null || streamInfo.getUri().isEmpty())
            {
                errors.Add(PlaylistError.I_FRAME_STREAM_WITHOUT_URI);
            }

            if (streamInfo.getBandwidth() == StreamInfo.NO_BANDWIDTH)
            {
                errors.Add(PlaylistError.I_FRAME_STREAM_WITH_NO_BANDWIDTH);
            }

            if (streamInfo.getAverageBandwidth() < StreamInfo.NO_BANDWIDTH)
            {
                errors.Add(PlaylistError.I_FRAME_STREAM_WITH_INVALID_AVERAGE_BANDWIDTH);
            }
        }

        private static void addMediaDataErrors(MediaData mediaData, HashSet<PlaylistError> errors)
        {
            if (mediaData.getType() == null)
            {
                errors.Add(PlaylistError.MEDIA_DATA_WITHOUT_TYPE);
            }

            if (mediaData.getGroupId() == null)
            {
                errors.Add(PlaylistError.MEDIA_DATA_WITHOUT_GROUP_ID);
            }

            if (mediaData.getName() == null)
            {
                errors.Add(PlaylistError.MEDIA_DATA_WITHOUT_NAME);
            }

            if (mediaData.getType() == MediaType.CLOSED_CAPTIONS)
            {
                if (mediaData.hasUri())
                {
                    errors.Add(PlaylistError.CLOSE_CAPTIONS_WITH_URI);
                }

                if (mediaData.getInStreamId() == null)
                {
                    errors.Add(PlaylistError.CLOSE_CAPTIONS_WITHOUT_IN_STREAM_ID);
                }
            }
            else
            {
                if (mediaData.getType() != MediaType.CLOSED_CAPTIONS && mediaData.getInStreamId() != null)
                {
                    errors.Add(PlaylistError.IN_STREAM_ID_WITHOUT_CLOSE_CAPTIONS);
                }
            }

            if (mediaData.isDefault() && !mediaData.isAutoSelect())
            {
                errors.Add(PlaylistError.DEFAULT_WITHOUT_AUTO_SELECT);
            }

            if (mediaData.getType() != MediaType.SUBTITLES && mediaData.isForced())
            {
                errors.Add(PlaylistError.FORCED_WITHOUT_SUBTITLES);
            }
        }

        private static void addTrackDataErrors(TrackData trackData, HashSet<PlaylistError> errors, bool isExtended, ParsingMode parsingMode)
        {
            if (trackData.getUri() == null || trackData.getUri().isEmpty())
            {
                errors.Add(PlaylistError.TRACK_DATA_WITHOUT_URI);
            }

            if (isExtended && !trackData.hasTrackInfo())
            {
                errors.Add(PlaylistError.EXTENDED_TRACK_DATA_WITHOUT_TRACK_INFO);
            }

            if (trackData.hasEncryptionData())
            {
                if (trackData.getEncryptionData().getMethod() == null)
                {
                    errors.Add(PlaylistError.ENCRYPTION_DATA_WITHOUT_METHOD);
                }
            }

            if (trackData.hasTrackInfo())
            {
                if (!parsingMode.allowNegativeNumbers && trackData.getTrackInfo().duration < 0)
                {
                    errors.Add(PlaylistError.TRACK_INFO_WITH_NEGATIVE_DURATION);
                }
            }

            if (trackData.hasMapInfo())
            {
                if (trackData.getMapInfo().getUri() == null || trackData.getMapInfo().getUri().isEmpty())
                {
                    errors.Add(PlaylistError.MAP_INFO_WITHOUT_URI);
                }
            }
        }
    }
}
