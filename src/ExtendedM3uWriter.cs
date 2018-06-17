using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace M3U8Parser
{
    class ExtendedM3uWriter : Writer
    {
        private List<SectionWriter> mExtTagWriter = new List<SectionWriter>();

        public ExtendedM3uWriter(Stream outputStream, Encoding encoding) :
            base(outputStream, encoding)
        {
            // Order influences output in file!
            putWriters(
                    ExtTagWriter.EXTM3U_HANDLER,
                    ExtTagWriter.EXT_X_VERSION_HANDLER,
                    MediaPlaylistTagWriter.EXT_X_PLAYLIST_TYPE,
                    MediaPlaylistTagWriter.EXT_X_TARGETDURATION,
                    MediaPlaylistTagWriter.EXT_X_START,
                    MediaPlaylistTagWriter.EXT_X_MEDIA_SEQUENCE,
                    MediaPlaylistTagWriter.EXT_X_I_FRAMES_ONLY,
                    MasterPlaylistTagWriter.EXT_X_MEDIA,
                    MediaPlaylistTagWriter.EXT_X_ALLOW_CACHE,
                    MasterPlaylistTagWriter.EXT_X_STREAM_INF,
                    MasterPlaylistTagWriter.EXT_X_I_FRAME_STREAM_INF,
                    MediaPlaylistTagWriter.MEDIA_SEGMENTS,
                    MediaPlaylistTagWriter.EXT_X_ENDLIST
            );
        }

        private void putWriters(params SectionWriter[] writers)
        {
            if (writers != null)
            {
                mExtTagWriter.AddRange(writers);
            }
        }

        public override void doWrite(Playlist playlist)
        { // throws IOException, ParseException, PlaylistException
            foreach (SectionWriter singleTagWriter in mExtTagWriter)
            {
                singleTagWriter.write(tagWriter, playlist);
            }
        }
    }
}
