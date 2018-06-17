using System;
using System.Text;

namespace M3U8Parser
{
    class TrackLineParser : LineParser
    {
        public void parse(String line, ParseState state) // throws ParseException 
        {
            TrackData.Builder builder = new TrackData.Builder();
            MediaParseState mediaState = state.getMedia();

            if (state.isExtended() && mediaState.trackInfo == null)
            {
                throw ParseException.create(ParseExceptionType.MISSING_TRACK_INFO, line);
            }

            mediaState.tracks.Add(builder
                    .withUri(line)
                    .withTrackInfo(mediaState.trackInfo)
                    .withEncryptionData(mediaState.encryptionData)
                    .withProgramDateTime(mediaState.programDateTime)
                    .withDiscontinuity(mediaState.hasDiscontinuity)
                    .withMapInfo(mediaState.mapInfo)
                    .withByteRange(mediaState.byteRange)
                    .build());

            mediaState.trackInfo = null;
            mediaState.programDateTime = null;
            mediaState.hasDiscontinuity = false;
            mediaState.mapInfo = null;
            mediaState.byteRange = null;
        }
    }
}
