using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    abstract class MasterPlaylistTagWriter : ExtTagWriter
    {
        public override void write(TagWriter tagWriter, Playlist playlist)
        {
            if (playlist.hasMasterPlaylist())
            {
                doWrite(tagWriter, playlist, playlist.getMasterPlaylist());
            }
        }

        public virtual void doWrite(TagWriter tagWriter, Playlist playlist, MasterPlaylist masterPlaylist)
        {
            tagWriter.writeTag(getTag());
        }

        // master playlist tags

        public static readonly IExtTagWriter EXT_X_MEDIA = new EXT_X_MEDIA_CLASS();
        private class EXT_X_MEDIA_CLASS : MasterPlaylistTagWriter
        {
            private readonly Dictionary<String, AttributeWriter<MediaData>> HANDLERS =
                new Dictionary<String, AttributeWriter<MediaData>>();

            public EXT_X_MEDIA_CLASS()
            {
                HANDLERS.Add(Constants.TYPE, new TYPE_AttributeWriter());
                HANDLERS.Add(Constants.URI, new URI_AttributeWriter());
                HANDLERS.Add(Constants.GROUP_ID, new GROUP_ID_AttributeWriter());
                HANDLERS.Add(Constants.LANGUAGE, new LANGUAGE_AttributeWriter());
                HANDLERS.Add(Constants.ASSOCIATED_LANGUAGE, new ASSOCIATED_LANGUAGE_AttributeWriter());
                HANDLERS.Add(Constants.NAME, new NAME_AttributeWriter());
                HANDLERS.Add(Constants.DEFAULT, new DEFAULT_AttributeWriter());
                HANDLERS.Add(Constants.AUTO_SELECT, new AUTO_SELECT_AttributeWriter());
                HANDLERS.Add(Constants.FORCED, new FORCED_AttributeWriter());
                HANDLERS.Add(Constants.IN_STREAM_ID, new IN_STREAM_ID_AttributeWriter());
                HANDLERS.Add(Constants.CHARACTERISTICS, new CHARACTERISTICS_AttributeWriter());
            }

            private class TYPE_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return true;
                }

                public String write(MediaData mediaData)
                {
                    return mediaData.getType().getValue();
                }
            }

            private class URI_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return mediaData.hasUri();
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeQuotedString(mediaData.getUri());
                }
            }

            private class GROUP_ID_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return true;
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeQuotedString(mediaData.getGroupId());
                }

            }

            private class LANGUAGE_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return mediaData.hasLanguage();
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeQuotedString(mediaData.getLanguage());
                }
            }

            private class ASSOCIATED_LANGUAGE_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return mediaData.hasAssociatedLanguage();
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeQuotedString(mediaData.getAssociatedLanguage());
                }

            }

            private class NAME_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return true;
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeQuotedString(mediaData.getName());
                }
            }

            private class DEFAULT_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return true;
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeYesNo(mediaData.isDefault());
                }
            }

            private class AUTO_SELECT_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return true;
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeYesNo(mediaData.isAutoSelect());
                }
            }

            private class FORCED_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return true;
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeYesNo(mediaData.isForced());
                }
            }

            private class IN_STREAM_ID_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return mediaData.hasInStreamId();
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeQuotedString(mediaData.getInStreamId());
                }
            }

            private class CHARACTERISTICS_AttributeWriter : AttributeWriter<MediaData>
            {
                public bool containsAttribute(MediaData mediaData)
                {
                    return mediaData.hasCharacteristics();
                }

                public String write(MediaData mediaData)
                {
                    return WriteUtil.writeQuotedString(WriteUtil.join(mediaData.getCharacteristics(), Constants.COMMA));
                }
            }


            public override String getTag()
            {
                return Constants.EXT_X_MEDIA_TAG;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MasterPlaylist masterPlaylist)
            {
                if (masterPlaylist.getMediaData().Count > 0)
                {
                    List<MediaData> mds = masterPlaylist.getMediaData();
                    foreach (MediaData md in mds)
                    {
                        writeAttributes(tagWriter, md, HANDLERS);
                    }
                }
            }
        };

        abstract class EXT_STREAM_INF<T> : MasterPlaylistTagWriter
            where T : IStreamInfo
        {
            public EXT_STREAM_INF()
            {
                HANDLERS.Add(Constants.CODECS, new CODECS_AttributeWriter());
                HANDLERS.Add(Constants.BANDWIDTH, new BANDWIDTH_AttributeWriter());
                HANDLERS.Add(Constants.AVERAGE_BANDWIDTH, new AVERAGE_BANDWIDTH_AttributeWriter());
                HANDLERS.Add(Constants.RESOLUTION, new RESOLUTION_AttributeWriter());
                HANDLERS.Add(Constants.FRAME_RATE, new FRAME_RATE_AttributeWriter());
                HANDLERS.Add(Constants.VIDEO, new VIDEO_AttributeWriter());
                HANDLERS.Add(Constants.PROGRAM_ID, new PROGRAM_ID_AttributeWriter());
            }

            protected Dictionary<String, AttributeWriter<T>> HANDLERS = new Dictionary<String, AttributeWriter<T>>();


            private class BANDWIDTH_AttributeWriter : AttributeWriter<T>
            {
                public bool containsAttribute(T streamInfo)
                {
                    return true;
                }

                public String write(T streamInfo)
                {
                    return streamInfo.getBandwidth().ToString();
                }
            }

            private class AVERAGE_BANDWIDTH_AttributeWriter : AttributeWriter<T>
            {
                public bool containsAttribute(T streamInfo)
                {
                    return streamInfo.hasAverageBandwidth();
                }

                public String write(T streamInfo)
                {
                    return streamInfo.getAverageBandwidth().ToString();
                }
            }

            private class CODECS_AttributeWriter : AttributeWriter<T>
            {
                public bool containsAttribute(T streamInfo)
                {
                    return streamInfo.hasCodecs();
                }

                public String write(T streamInfo)
                {
                    return WriteUtil.writeQuotedString(WriteUtil.join(streamInfo.getCodecs(), Constants.COMMA));
                }
            }

            private class RESOLUTION_AttributeWriter : AttributeWriter<T>
            {
                public bool containsAttribute(T streamInfo)
                {
                    return streamInfo.hasResolution();
                }

                public String write(T streamInfo)
                {
                    return WriteUtil.writeResolution(streamInfo.getResolution());
                }
            }

            private class FRAME_RATE_AttributeWriter : AttributeWriter<T>
            {
                public bool containsAttribute(T streamInfo)
                {
                    return streamInfo.hasFrameRate();
                }

                public String write(T streamInfo)
                {
                    return streamInfo.getFrameRate().ToString();
                }
            }

            private class VIDEO_AttributeWriter : AttributeWriter<T>
            {
                public bool containsAttribute(T streamInfo)
                {
                    return streamInfo.hasVideo();
                }

                public String write(T streamInfo)
                {
                    return WriteUtil.writeQuotedString(streamInfo.getVideo());
                }
            }

            private class PROGRAM_ID_AttributeWriter : AttributeWriter<T>
            {
                public bool containsAttribute(T streamInfo)
                {
                    return false;
                }

                public String write(T streamInfo)
                {
                    // deprecated
                    return "";
                }
            }


            public override bool hasData()
            {
                return true;
            }

            //public abstract void doWrite(TagWriter tagWriter, Playlist playlist, MasterPlaylist masterPlaylist);
        }

        public static readonly IExtTagWriter EXT_X_I_FRAME_STREAM_INF = new EXT_X_I_FRAME_STREAM_INF_CLASS();
        private class EXT_X_I_FRAME_STREAM_INF_CLASS : EXT_STREAM_INF<IFrameStreamInfo>
        {

            public EXT_X_I_FRAME_STREAM_INF_CLASS()
            {
                HANDLERS.Add(Constants.URI, new URI_AttributeWriter());
            }
            private class URI_AttributeWriter : AttributeWriter<IFrameStreamInfo>
            {

                public bool containsAttribute(IFrameStreamInfo streamInfo)
                {
                    return true;
                }


                public String write(IFrameStreamInfo streamInfo)
                {
                    return WriteUtil.writeQuotedString(streamInfo.getUri());
                }
            }


            public override String getTag()
            {
                return Constants.EXT_X_I_FRAME_STREAM_INF_TAG;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MasterPlaylist masterPlaylist)
            {
                foreach (IFrameStreamInfo streamInfo in masterPlaylist.getIFramePlaylists())
                {
                    writeAttributes(tagWriter, streamInfo, HANDLERS);
                }
            }
        };

        public static readonly IExtTagWriter EXT_X_STREAM_INF = new EXT_X_STREAM_INF_CLASS();
        private class EXT_X_STREAM_INF_CLASS : EXT_STREAM_INF<StreamInfo>
        {

            public EXT_X_STREAM_INF_CLASS()
            {
                HANDLERS.Add(Constants.AUDIO, new AUDIO_AttributeWriter());
                HANDLERS.Add(Constants.SUBTITLES, new SUBTITLES_AttributeWriter());
                HANDLERS.Add(Constants.CLOSED_CAPTIONS, new CLOSED_CAPTIONS_AttributeWriter());
            }
            private class AUDIO_AttributeWriter : AttributeWriter<StreamInfo>
            {
                public bool containsAttribute(StreamInfo streamInfo)
                {
                    return streamInfo.hasAudio();
                }

                public String write(StreamInfo streamInfo)
                {
                    return WriteUtil.writeQuotedString(streamInfo.getAudio());
                }
            }

            private class SUBTITLES_AttributeWriter : AttributeWriter<StreamInfo>
            {
                public bool containsAttribute(StreamInfo streamInfo)
                {
                    return streamInfo.hasSubtitles();
                }

                public String write(StreamInfo streamInfo)
                {
                    return WriteUtil.writeQuotedString(streamInfo.getSubtitles());
                }
            }

            private class CLOSED_CAPTIONS_AttributeWriter : AttributeWriter<StreamInfo>
            {
                public bool containsAttribute(StreamInfo streamInfo)
                {
                    return streamInfo.hasClosedCaptions();
                }

                public String write(StreamInfo streamInfo)
                {
                    return WriteUtil.writeQuotedString(streamInfo.getClosedCaptions());
                }
            }


            public override String getTag()
            {
                return Constants.EXT_X_STREAM_INF_TAG;
            }

            public override void doWrite(TagWriter tagWriter, Playlist playlist, MasterPlaylist masterPlaylist)
            {
                foreach (PlaylistData playlistData in masterPlaylist.getPlaylists())
                {
                    if (playlistData.hasStreamInfo())
                    {
                        writeAttributes(tagWriter, playlistData.getStreamInfo(), HANDLERS);
                        tagWriter.writeLine(playlistData.getUri());
                    }
                }
            }
        }
    }
}
