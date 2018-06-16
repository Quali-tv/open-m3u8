using System;
using System.Collections.Generic;
using System.Text;
// import com.iheartradio.m3u8.data.*;

// import java.io.IOException;
// import java.util.HashMap;
// import java.util.LinkedHashMap;
// import java.util.Map;

namespace M3U8Parser
{
    public abstract class MediaPlaylistTagWriter : ExtTagWriter
    {
        public override void write(TagWriter tagWriter, Playlist playlist) // throws IOException, ParseException 
        {
            if (playlist.hasMediaPlaylist())
            {
                doWrite(tagWriter, playlist, playlist.getMediaPlaylist());
            }
        }

        public virtual void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException, ParseException 
        {
            tagWriter.writeTag(getTag());
        }

        // media playlist tags

        public static readonly IExtTagWriter EXT_X_ENDLIST = new EXT_X_ENDLIST_CLASS();
        private class EXT_X_ENDLIST_CLASS : MediaPlaylistTagWriter
        {

            public override String getTag()
            {
                return Constants.EXT_X_ENDLIST_TAG;
            }

            public override bool hasData()
            {
                return false;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException 
            {
                if (!mediaPlaylist.isOngoing())
                {
                    tagWriter.writeTag(getTag());
                }
            }
        }

        public static readonly IExtTagWriter EXT_X_I_FRAMES_ONLY = new EXT_X_I_FRAMES_ONLY_CLASS();
        private class EXT_X_I_FRAMES_ONLY_CLASS : MediaPlaylistTagWriter
        {
            public override String getTag()
            {
                return Constants.EXT_X_I_FRAMES_ONLY_TAG;
            }

            public override bool hasData()
            {
                return false;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException 
            {
                if (mediaPlaylist.isIframesOnly())
                {
                    tagWriter.writeTag(getTag());
                }
            }
        }

        public static readonly IExtTagWriter EXT_X_PLAYLIST_TYPE = new EXT_X_PLAYLIST_TYPE_CLASS();
        private class EXT_X_PLAYLIST_TYPE_CLASS : MediaPlaylistTagWriter
        {
            public override String getTag()
            {
                return Constants.EXT_X_PLAYLIST_TYPE_TAG;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException 
            {
                if (mediaPlaylist.getPlaylistType() != null)
                {
                    tagWriter.writeTag(getTag(), mediaPlaylist.getPlaylistType().getValue());
                }
            }
        }

        public static readonly IExtTagWriter EXT_X_START = new EXT_X_START_CLASS();
        private class EXT_X_START_CLASS : MediaPlaylistTagWriter
        {

            public EXT_X_START_CLASS()
            {
                HANDLERS.Add(Constants.TIME_OFFSET, new TIME_OFFSET_AttributeWriter());
                HANDLERS.Add(Constants.PRECISE, new PRECISE_AttributeWriter());
            }

            private readonly Dictionary<String, AttributeWriter<StartData>> HANDLERS =
            new Dictionary<String, AttributeWriter<StartData>>();


            private class TIME_OFFSET_AttributeWriter : AttributeWriter<StartData>
            {
                public bool containsAttribute(StartData attributes)
                {
                    return true;
                }

                public String write(StartData attributes) // throws ParseException
                {
                    return attributes.getTimeOffset().ToString();
                }
            }

            private class PRECISE_AttributeWriter : AttributeWriter<StartData>
            {

                public bool containsAttribute(StartData attributes)
                {
                    return true;
                }

                public String write(StartData attributes) // throws ParseException
                {
                    if (attributes.isPrecise())
                    {
                        return Constants.YES;
                    }
                    else
                    {
                        return Constants.NO;
                    }
                }
            }

            public override String getTag()
            {
                return Constants.EXT_X_START_TAG;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException, ParseException 
            {
                if (mediaPlaylist.hasStartData())
                {
                    StartData startData = mediaPlaylist.getStartData();
                    writeAttributes(tagWriter, startData, HANDLERS);
                }
            }
        }

        public static readonly IExtTagWriter EXT_X_TARGETDURATION = new EXT_X_TARGETDURATION_CLASS();
        private class EXT_X_TARGETDURATION_CLASS : MediaPlaylistTagWriter
        {
            public override String getTag()
            {
                return Constants.EXT_X_TARGETDURATION_TAG;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException, ParseException 
            {
                tagWriter.writeTag(getTag(), mediaPlaylist.getTargetDuration().ToString());
            }
        }

        public static readonly IExtTagWriter EXT_X_MEDIA_SEQUENCE = new EXT_X_MEDIA_SEQUENCE_CLASS();
        private class EXT_X_MEDIA_SEQUENCE_CLASS : MediaPlaylistTagWriter
        {
            public override String getTag()
            {
                return Constants.EXT_X_MEDIA_SEQUENCE_TAG;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException, ParseException 
            {
                tagWriter.writeTag(getTag(), mediaPlaylist.getMediaSequenceNumber().ToString());
            }
        }

        public static readonly IExtTagWriter EXT_X_ALLOW_CACHE = new EXT_X_ALLOW_CACHE_CLASS();
        private class EXT_X_ALLOW_CACHE_CLASS : MediaPlaylistTagWriter
        {
            public override String getTag()
            {
                return Constants.EXT_X_ALLOW_CACHE_TAG;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist)
            {

                // deprecated
            }
        }

        // media segment tags

        public static readonly SectionWriter MEDIA_SEGMENTS = new MEDIA_SEGMENTS_CLASS();
        private class MEDIA_SEGMENTS_CLASS : SectionWriter
        {
            public void write(TagWriter tagWriter, Playlist playlist) // throws IOException, ParseException 
            {
                if (playlist.hasMediaPlaylist())
                {
                    KeyWriter keyWriter = new KeyWriter();
                    MapInfoWriter mapInfoWriter = new MapInfoWriter();

                    foreach (TrackData trackData in playlist.getMediaPlaylist().getTracks())
                    {
                        if (trackData.hasDiscontinuity())
                        {
                            tagWriter.writeTag(Constants.EXT_X_DISCONTINUITY_TAG);
                        }

                        keyWriter.writeTrackData(tagWriter, playlist, trackData);
                        mapInfoWriter.writeTrackData(tagWriter, playlist, trackData);

                        if (trackData.hasByteRange())
                        {
                            writeByteRange(tagWriter, trackData.getByteRange());
                        }

                        writeExtinf(tagWriter, playlist, trackData);
                        tagWriter.writeLine(trackData.getUri());
                    }
                }
            }
        }

        private static void writeExtinf(TagWriter tagWriter, Playlist playlist, TrackData trackData) // throws IOException
        {
            StringBuilder builder = new StringBuilder();

            if (playlist.getCompatibilityVersion() < 3)
            {
                builder.Append(((int)trackData.getTrackInfo().duration).ToString());
            }
            else
            {
                builder.Append(trackData.getTrackInfo().duration.ToString());
            }

            if (!string.IsNullOrEmpty(trackData.getTrackInfo().title))
            {
                builder.Append(Constants.COMMA);
                builder.Append(trackData.getTrackInfo().title);
            }

            tagWriter.writeTag(Constants.EXTINF_TAG, builder.ToString());
        }

        private static void writeByteRange(TagWriter tagWriter, ByteRange byteRange) // throws IOException
        {
            String value;

            if (byteRange.getOffset() != null)
            {
                value = byteRange.getSubRangeLength().ToString()
                        + '@' + byteRange.getOffset().ToString();
            }
            else
            {
                value = byteRange.getSubRangeLength().ToString();
            }

            tagWriter.writeTag(Constants.EXT_X_BYTERANGE_TAG, value);
        }

        private class KeyWriter : MediaPlaylistTagWriter
        {
            public KeyWriter()
            {
                HANDLERS.Add(Constants.METHOD, new METHOD_AttributeWriter());
                HANDLERS.Add(Constants.URI, new URI_AttributeWriter());
                HANDLERS.Add(Constants.IV, new IV_AttributeWriter());
                HANDLERS.Add(Constants.KEY_FORMAT, new KEY_FORMAT_AttributeWriter());
                HANDLERS.Add(Constants.KEY_FORMAT_VERSIONS, new KEY_FORMAT_VERSIONS_AttributeWriter());
            }

            private readonly Dictionary<String, AttributeWriter<EncryptionData>> HANDLERS =
                new Dictionary<String, AttributeWriter<EncryptionData>>();

            private EncryptionData mEncryptionData;


            private class METHOD_AttributeWriter : AttributeWriter<EncryptionData>
            {
                public bool containsAttribute(EncryptionData attributes)
                {
                    return true;
                }

                public String write(EncryptionData encryptionData)
                {
                    return encryptionData.getMethod().getValue();
                }
            }

            private class URI_AttributeWriter : AttributeWriter<EncryptionData>
            {
                public bool containsAttribute(EncryptionData attributes)
                {
                    return true;
                }

                public String write(EncryptionData encryptionData) // throws ParseException
                {
                    return WriteUtil.writeQuotedString(encryptionData.getUri());
                }
            }

            private class IV_AttributeWriter : AttributeWriter<EncryptionData>
            {
                public bool containsAttribute(EncryptionData attribute)
                {
                    return attribute.hasInitializationVector();
                }

                public String write(EncryptionData encryptionData)
                {
                    return WriteUtil.writeHexadecimal(encryptionData.getInitializationVector());
                }
            }

            private class KEY_FORMAT_AttributeWriter : AttributeWriter<EncryptionData>
            {
                public bool containsAttribute(EncryptionData attributes)
                {
                    return true;
                }

                public String write(EncryptionData encryptionData) // throws ParseException
                {
                    //TODO check for version 5
                    return WriteUtil.writeQuotedString(encryptionData.getKeyFormat(), true);
                }
            }

            private class KEY_FORMAT_VERSIONS_AttributeWriter : AttributeWriter<EncryptionData>
            {
                public bool containsAttribute(EncryptionData attributes)
                {
                    return true;
                }

                public String write(EncryptionData encryptionData) // throws ParseException
                {
                    //TODO check for version 5
                    return WriteUtil.writeQuotedString(WriteUtil.join(encryptionData.getKeyFormatVersions(), Constants.LIST_SEPARATOR), true);
                }
            }


            public override String getTag()
            {
                return Constants.EXT_X_KEY_TAG;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException, ParseException 
            {
                writeAttributes(tagWriter, mEncryptionData, HANDLERS);
            }

            public void writeTrackData(TagWriter tagWriter, Playlist playlist, TrackData trackData) // throws IOException, ParseException 
            {
                if (trackData != null && trackData.hasEncryptionData())
                {
                    EncryptionData encryptionData = trackData.getEncryptionData();

                    if (!encryptionData.Equals(mEncryptionData))
                    {
                        mEncryptionData = encryptionData;
                        write(tagWriter, playlist);
                    }
                }
            }
        }

        private class MapInfoWriter : MediaPlaylistTagWriter
        {
            public MapInfoWriter()
            {
                HANDLERS.Add(Constants.URI, new URI_AttributeWriter());
                HANDLERS.Add(Constants.BYTERANGE, new BYTERANGE_AttributeWriter());
            }

            private readonly Dictionary<String, AttributeWriter<MapInfo>> HANDLERS =
                new Dictionary<String, AttributeWriter<MapInfo>>();

            private MapInfo mMapInfo;


            private class URI_AttributeWriter : AttributeWriter<MapInfo>
            {

                public String write(MapInfo attributes) // throws ParseException
                {
                    return WriteUtil.writeQuotedString(attributes.getUri());
                }


                public bool containsAttribute(MapInfo attributes)
                {
                    return true;
                }
            }

            private class BYTERANGE_AttributeWriter : AttributeWriter<MapInfo>
            {
                public String write(MapInfo attributes) // throws ParseException
                {
                    ByteRange byteRange = attributes.getByteRange();
                    String value;
                    if (byteRange.hasOffset())
                    {
                        value = byteRange.getSubRangeLength().ToString()
                                + '@' + byteRange.getOffset().ToString();
                    }
                    else
                    {
                        value = byteRange.getSubRangeLength().ToString();
                    }

                    return WriteUtil.writeQuotedString(value);
                }

                public bool containsAttribute(MapInfo mapInfo)
                {
                    return mapInfo.hasByteRange();
                }
            }

            public override bool hasData()
            {
                return true;
            }

            public override String getTag()
            {
                return Constants.EXT_X_MAP;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MediaPlaylist mediaPlaylist) // throws IOException, ParseException 
            {
                writeAttributes(tagWriter, mMapInfo, HANDLERS);
            }

            public void writeTrackData(TagWriter tagWriter, Playlist playlist, TrackData trackData) // throws IOException, ParseException 
            {
                if (trackData != null && trackData.getMapInfo() != null)
                {
                    MapInfo mapInfo = trackData.getMapInfo();
                    if (!mapInfo.Equals(mMapInfo))
                    {
                        mMapInfo = mapInfo;
                        write(tagWriter, playlist);
                    }
                }
            }
        }
    }
}
