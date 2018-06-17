using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class ParseState : IParseState<Playlist>
    {
        public const int NONE = -1;

        public readonly Encoding encoding;
        public readonly List<String> unknownTags = new List<String>();

        private MasterParseState mMasterParseState;
        private MediaParseState mMediaParseState;
        private bool mIsExtended;
        private int mCompatibilityVersion = NONE;

        public StartData startData;

        public ParseState(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public bool isMaster()
        {
            return mMasterParseState != null;
        }

        public MasterParseState getMaster()
        {
            return mMasterParseState;
        }

        public void setMaster()
        {
            if (isMedia())
            {
                throw new ParseException(ParseExceptionType.MASTER_IN_MEDIA);
            }

            if (mMasterParseState == null)
            {
                mMasterParseState = new MasterParseState();
            }
        }

        public bool isMedia()
        {
            return mMediaParseState != null;
        }

        public MediaParseState getMedia()
        {
            return mMediaParseState;
        }

        public void setMedia()
        {
            if (mMediaParseState == null)
            {
                mMediaParseState = new MediaParseState();
            }
        }

        public bool isExtended()
        {
            return mIsExtended;
        }

        public void setExtended()
        {
            mIsExtended = true;
        }

        public void setIsIframesOnly()
        {
            if (isMaster())
            {
                throw new ParseException(ParseExceptionType.MEDIA_IN_MASTER);
            }

            getMedia().isIframesOnly = true;
        }

        public int getCompatibilityVersion()
        {
            return mCompatibilityVersion;
        }

        public void setCompatibilityVersion(int compatibilityVersion)
        {
            mCompatibilityVersion = compatibilityVersion;
        }


        public Playlist buildPlaylist()
        {
            Playlist.Builder playlistBuilder = new Playlist.Builder();

            if (isMaster())
            {
                playlistBuilder.withMasterPlaylist(buildInnerPlaylist(getMaster()));
            }
            else if (isMedia())
            {
                playlistBuilder
                        .withMediaPlaylist(buildInnerPlaylist(getMedia()))
                        .withExtended(mIsExtended);
            }
            else
            {
                throw new ParseException(ParseExceptionType.UNKNOWN_PLAYLIST_TYPE);
            }

            return playlistBuilder
                    .withCompatibilityVersion(mCompatibilityVersion == NONE ? Playlist.MIN_COMPATIBILITY_VERSION : mCompatibilityVersion)
                    .build();
        }

        private T buildInnerPlaylist<T>(PlaylistParseState<T> innerParseState)
        {
            return innerParseState
                    .setUnknownTags(unknownTags)
                    .setStartData(startData)
                    .buildPlaylist();
        }
    }
}
