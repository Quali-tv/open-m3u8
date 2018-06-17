using System;
using System.Text;

namespace M3U8Parser
{
    public class Playlist
    {
        public static readonly int MIN_COMPATIBILITY_VERSION = 1;

        private readonly MasterPlaylist mMasterPlaylist;
        private readonly MediaPlaylist mMediaPlaylist;
        private readonly bool mIsExtended;
        private readonly int mCompatibilityVersion;

        private Playlist(MasterPlaylist masterPlaylist, MediaPlaylist mediaPlaylist, bool isExtended, int compatibilityVersion)
        {
            mMasterPlaylist = masterPlaylist;
            mMediaPlaylist = mediaPlaylist;
            mIsExtended = isExtended;
            mCompatibilityVersion = compatibilityVersion;
        }

        public bool hasMasterPlaylist()
        {
            return mMasterPlaylist != null;
        }

        public bool hasMediaPlaylist()
        {
            return mMediaPlaylist != null;
        }

        public MasterPlaylist getMasterPlaylist()
        {
            return mMasterPlaylist;
        }

        public MediaPlaylist getMediaPlaylist()
        {
            return mMediaPlaylist;
        }

        public bool isExtended()
        {
            return mIsExtended;
        }

        public int getCompatibilityVersion()
        {
            return mCompatibilityVersion;
        }

        public Builder buildUpon()
        {
            return new Builder(mMasterPlaylist, mMediaPlaylist, mIsExtended, mCompatibilityVersion);
        }

        public override int GetHashCode()
        {
            // TODO: Implement
            //return object.hash(mCompatibilityVersion, mIsExtended, mMasterPlaylist, mMediaPlaylist);
            return 0;
        }

        public override bool Equals(Object o)
        {
            if (!(o is Playlist))
            {
                return false;
            }

            Playlist other = (Playlist)o;

            return object.Equals(mMasterPlaylist, other.mMasterPlaylist) &&
                   object.Equals(mMediaPlaylist, other.mMediaPlaylist) &&
                   mIsExtended == other.mIsExtended &&
                   mCompatibilityVersion == other.mCompatibilityVersion;
        }


        public override String ToString()
        {
            return new StringBuilder()
                    .Append("(Playlist")
                    .Append(" mMasterPlaylist=").Append(mMasterPlaylist)
                    .Append(" mMediaPlaylist=").Append(mMediaPlaylist)
                    .Append(" mIsExtended=").Append(mIsExtended)
                    .Append(" mCompatibilityVersion=").Append(mCompatibilityVersion)
                    .Append(")")
                    .ToString();
        }

        public class Builder
        {
            private MasterPlaylist mMasterPlaylist;
            private MediaPlaylist mMediaPlaylist;
            private bool mIsExtended;
            private int mCompatibilityVersion = MIN_COMPATIBILITY_VERSION;

            public Builder()
            {
            }

            public Builder(MasterPlaylist masterPlaylist, MediaPlaylist mediaPlaylist, bool isExtended, int compatibilityVersion)
            {
                mMasterPlaylist = masterPlaylist;
                mMediaPlaylist = mediaPlaylist;
                mIsExtended = isExtended;
                mCompatibilityVersion = compatibilityVersion;
            }

            public Builder withMasterPlaylist(MasterPlaylist masterPlaylist)
            {
                mMasterPlaylist = masterPlaylist;
                return withExtended(true);
            }

            public Builder withMediaPlaylist(MediaPlaylist mediaPlaylist)
            {
                mMediaPlaylist = mediaPlaylist;
                return this;
            }

            public Builder withExtended(bool isExtended)
            {
                mIsExtended = isExtended;
                return this;
            }

            public Builder withCompatibilityVersion(int version)
            {
                mCompatibilityVersion = version;
                return this;
            }

            public Playlist build()
            {
                return new Playlist(mMasterPlaylist, mMediaPlaylist, mIsExtended, mCompatibilityVersion);
            }
        }
    }
}