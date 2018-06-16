using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
// import java.util.Collections;
// import java.util.List;
// import java.util.Objects;

namespace M3U8Parser
{
    public class EncryptionData
    {
        private readonly EncryptionMethod mMethod;
        private readonly String mUri;
        private readonly List<Byte> mInitializationVector;
        private readonly String mKeyFormat;
        private readonly List<int> mKeyFormatVersions;

        private EncryptionData(EncryptionMethod method, String uri, List<Byte> initializationVector, String keyFormat, List<int> keyFormats)
        {
            mMethod = method;
            mUri = uri;
            mInitializationVector = initializationVector; // TODO: == null ? null : Collections.unmodifiableList(initializationVector);
            mKeyFormat = keyFormat;
            mKeyFormatVersions = keyFormats; // TODO: == null ? null : Collections.unmodifiableList(keyFormats);
        }

        public EncryptionMethod getMethod()
        {
            return mMethod;
        }

        public bool hasUri()
        {
            return !string.IsNullOrEmpty(mUri);
        }

        public String getUri()
        {
            return mUri;
        }

        public bool hasInitializationVector()
        {
            return mInitializationVector != null;
        }

        public List<Byte> getInitializationVector()
        {
            return mInitializationVector;
        }

        public bool hasKeyFormat()
        {
            return mKeyFormat != null;
        }

        public String getKeyFormat()
        {
            return mKeyFormat;
        }

        public bool hasKeyFormatVersions()
        {
            return mKeyFormatVersions != null;
        }

        public List<int> getKeyFormatVersions()
        {
            return mKeyFormatVersions;
        }

        public Builder buildUpon()
        {
            return new Builder(mMethod, mUri, mInitializationVector, mKeyFormat, mKeyFormatVersions);
        }

        public override int GetHashCode()
        {
            // TODO: Implement 
            //return Objects.hash(mInitializationVector, mKeyFormat, mKeyFormatVersions, mMethod, mUri);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (!(o is EncryptionData))
            {
                return false;
            }

            EncryptionData other = (EncryptionData)o;

            return this.mInitializationVector.SequenceEquals(other.mInitializationVector) &&
                   object.Equals(this.mKeyFormat, other.mKeyFormat) &&
                   this.mKeyFormatVersions.SequenceEquals(other.mKeyFormatVersions) &&
                   object.Equals(this.mMethod, other.mMethod) &&
                   object.Equals(this.mUri, other.mUri);
        }

        public class Builder
        {
            private EncryptionMethod mMethod;
            private String mUri;
            private List<Byte> mInitializationVector;
            private String mKeyFormat;
            private List<int> mKeyFormatVersions;

            public Builder()
            {
            }

            public Builder(EncryptionMethod method, String uri, List<Byte> initializationVector, String keyFormat, List<int> keyFormatVersions)
            {
                mMethod = method;
                mUri = uri;
                mInitializationVector = initializationVector;
                mKeyFormat = keyFormat;
                mKeyFormatVersions = keyFormatVersions;
            }

            public Builder withMethod(EncryptionMethod method)
            {
                mMethod = method;
                return this;
            }

            public Builder withUri(String uri)
            {
                mUri = uri;
                return this;
            }

            public Builder withInitializationVector(List<Byte> initializationVector)
            {
                mInitializationVector = initializationVector;
                return this;
            }

            public Builder withKeyFormat(String keyFormat)
            {
                mKeyFormat = keyFormat;
                return this;
            }

            public Builder withKeyFormatVersions(List<int> keyFormatVersions)
            {
                mKeyFormatVersions = keyFormatVersions;
                return this;
            }

            public EncryptionData build()
            {
                return new EncryptionData(mMethod, mUri, mInitializationVector, mKeyFormat, mKeyFormatVersions);
            }
        }
    }
}
