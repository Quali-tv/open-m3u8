using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// import java.util.List;
// import java.util.Objects;

namespace M3U8Parser
{
    public class MediaData
    {
        public const int NO_CHANNELS = -1;

        private readonly MediaType mType;
        private readonly String mUri;
        private readonly String mGroupId;
        private readonly String mLanguage;
        private readonly String mAssociatedLanguage;
        private readonly String mName;
        private readonly bool mDefault;
        private readonly bool mAutoSelect;
        private readonly bool mForced;
        private readonly String mInStreamId;
        private readonly List<String> mCharacteristics;
        private readonly int mChannels;

        private MediaData(
                MediaType type,
                String uri,
                String groupId,
                String language,
                String associatedLanguage,
                String name,
                bool isDefault,
                bool isAutoSelect,
                bool isForced,
                String inStreamId,
                List<String> characteristics,
                int channels)
        {
            mType = type;
            mUri = uri;
            mGroupId = groupId;
            mLanguage = language;
            mAssociatedLanguage = associatedLanguage;
            mName = name;
            mDefault = isDefault;
            mAutoSelect = isAutoSelect;
            mForced = isForced;
            mInStreamId = inStreamId;
            mCharacteristics = DataUtil.emptyOrUnmodifiable(characteristics);
            mChannels = channels;
        }

        public MediaType getType()
        {
            return mType;
        }

        public bool hasUri()
        {
            return !string.IsNullOrEmpty(mUri);
        }

        public String getUri()
        {
            return mUri;
        }

        public String getGroupId()
        {
            return mGroupId;
        }

        public bool hasLanguage()
        {
            return mLanguage != null;
        }

        public String getLanguage()
        {
            return mLanguage;
        }

        public bool hasAssociatedLanguage()
        {
            return mAssociatedLanguage != null;
        }

        public String getAssociatedLanguage()
        {
            return mAssociatedLanguage;
        }

        public String getName()
        {
            return mName;
        }

        public bool isDefault()
        {
            return mDefault;
        }

        public bool isAutoSelect()
        {
            return mAutoSelect;
        }

        public bool isForced()
        {
            return mForced;
        }

        public bool hasInStreamId()
        {
            return mInStreamId != null;
        }

        public String getInStreamId()
        {
            return mInStreamId;
        }

        public bool hasCharacteristics()
        {
            return !mCharacteristics.isEmpty();
        }

        public List<String> getCharacteristics()
        {
            return mCharacteristics;
        }

        public bool hasChannels()
        {
            return mChannels != NO_CHANNELS;
        }

        public int getChannels()
        {
            return mChannels;
        }

        public Builder buildUpon()
        {
            return new Builder(
                    mType,
                    mUri,
                    mGroupId,
                    mLanguage,
                    mAssociatedLanguage,
                    mName,
                    mDefault,
                    mAutoSelect,
                    mForced,
                    mInStreamId,
                    mCharacteristics,
                    mChannels);
        }

        public override int GetHashCode()
        {
            // TODO: Implement 
            //return Objects.hash(
            //        mAssociatedLanguage,
            //        mAutoSelect,
            //        mCharacteristics,
            //        mDefault,
            //        mForced,
            //        mGroupId,
            //        mInStreamId,
            //        mLanguage,
            //        mName,
            //        mType,
            //        mUri,
            //        mChannels);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (!(o is MediaData))
            {
                return false;
            }

            MediaData other = (MediaData)o;

            return mType == other.mType &&
                   object.Equals(mUri, other.mUri) &&
                   object.Equals(mGroupId, other.mGroupId) &&
                   object.Equals(mLanguage, other.mLanguage) &&
                   object.Equals(mAssociatedLanguage, other.mAssociatedLanguage) &&
                   object.Equals(mName, other.mName) &&
                   mDefault == other.mDefault &&
                   mAutoSelect == other.mAutoSelect &&
                   mForced == other.mForced &&
                   object.Equals(mInStreamId, other.mInStreamId) &&
                   mCharacteristics.SequenceEquals(other.mCharacteristics) &&
                   mChannels == other.mChannels;
        }

        public class Builder
        {
            private MediaType mType;
            private String mUri;
            private String mGroupId;
            private String mLanguage;
            private String mAssociatedLanguage;
            private String mName;
            private bool mDefault;
            private bool mAutoSelect;
            private bool mForced;
            private String mInStreamId;
            private List<String> mCharacteristics;
            private int mChannels = NO_CHANNELS;

            public Builder()
            {
            }

            public Builder(
                    MediaType type,
                    String uri,
                    String groupId,
                    String language,
                    String associatedLanguage,
                    String name,
                    bool isDefault,
                    bool autoSelect,
                    bool forced,
                    String inStreamId,
                    List<String> characteristics,
                    int channels)
            {
                mType = type;
                mUri = uri;
                mGroupId = groupId;
                mLanguage = language;
                mAssociatedLanguage = associatedLanguage;
                mName = name;
                mDefault = isDefault;
                mAutoSelect = autoSelect;
                mForced = forced;
                mInStreamId = inStreamId;
                mCharacteristics = characteristics;
                mChannels = channels;
            }

            public Builder withType(MediaType type)
            {
                mType = type;
                return this;
            }

            public Builder withUri(String uri)
            {
                mUri = uri;
                return this;
            }

            public Builder withGroupId(String groupId)
            {
                mGroupId = groupId;
                return this;
            }

            public Builder withLanguage(String language)
            {
                mLanguage = language;
                return this;
            }

            public Builder withAssociatedLanguage(String associatedLanguage)
            {
                mAssociatedLanguage = associatedLanguage;
                return this;
            }

            public Builder withName(String name)
            {
                mName = name;
                return this;
            }

            public Builder withDefault(bool isDefault)
            {
                mDefault = isDefault;
                return this;
            }

            public Builder withAutoSelect(bool isAutoSelect)
            {
                mAutoSelect = isAutoSelect;
                return this;
            }

            public Builder withForced(bool isForced)
            {
                mForced = isForced;
                return this;
            }

            public Builder withInStreamId(String inStreamId)
            {
                mInStreamId = inStreamId;
                return this;
            }

            public Builder withCharacteristics(List<String> characteristics)
            {
                mCharacteristics = characteristics;
                return this;
            }

            public Builder withChannels(int channels)
            {
                mChannels = channels;
                return this;
            }

            public MediaData build()
            {
                return new MediaData(
                        mType,
                        mUri,
                        mGroupId,
                        mLanguage,
                        mAssociatedLanguage,
                        mName,
                        mDefault,
                        mAutoSelect,
                        mForced,
                        mInStreamId,
                        mCharacteristics,
                        mChannels);
            }
        }

        public override string ToString()
        {
            return "MediaData [mType=" + mType + ", mUri=" + mUri + ", mGroupId="
                    + mGroupId + ", mLanguage=" + mLanguage
                    + ", mAssociatedLanguage=" + mAssociatedLanguage + ", mName="
                    + mName + ", mDefault=" + mDefault + ", mAutoSelect="
                    + mAutoSelect + ", mForced=" + mForced + ", mInStreamId="
                    + mInStreamId + ", mCharacteristics=" + mCharacteristics
                    + ", mChannels=" + mChannels + "]";
        }
    }
}
