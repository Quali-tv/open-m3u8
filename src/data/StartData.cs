using System;
using System.Collections.Generic;
using System.Text;
//import java.util.Objects;

namespace M3U8Parser
{
    public class StartData
    {
        private readonly float mTimeOffset;
        private readonly bool mPrecise;

        public StartData(float timeOffset, bool precise)
        {
            mTimeOffset = timeOffset;
            mPrecise = precise;
        }

        public float getTimeOffset()
        {
            return mTimeOffset;
        }

        public bool isPrecise()
        {
            return mPrecise;
        }

        public Builder buildUpon()
        {
            return new Builder(mTimeOffset, mPrecise);
        }

        public override int GetHashCode()
        {
            // TODO: Implement
            //return Objects.hash(mPrecise, mTimeOffset);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (!(o is StartData))
            {
                return false;
            }

            StartData other = (StartData)o;

            return this.mPrecise == other.mPrecise &&
                   this.mTimeOffset == other.mTimeOffset;
        }

        public class Builder
        {
            private float mTimeOffset = float.NaN;
            private bool mPrecise;

            public Builder()
            {
            }

            public Builder(float timeOffset, bool precise)
            {
                mTimeOffset = timeOffset;
                mPrecise = precise;
            }

            public Builder withTimeOffset(float timeOffset)
            {
                mTimeOffset = timeOffset;
                return this;
            }

            public Builder withPrecise(bool precise)
            {
                mPrecise = precise;
                return this;
            }

            public StartData build()
            {
                return new StartData(mTimeOffset, mPrecise);
            }
        }
    }
}
