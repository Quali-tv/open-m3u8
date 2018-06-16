using System;
using System.Text;

namespace M3U8Parser
{
    public class ParsingMode
    {
        public static readonly ParsingMode STRICT = new Builder().build();

        public static readonly ParsingMode LENIENT = new Builder()
                .allowUnknownTags()
                .allowNegativeNumbers()
                .build();

        /**
         * If true, unrecognized tags will not throw an exception. Instead, they will be made available in the playlist for custom parsing.
         */
        public readonly bool allowUnknownTags;

        /**
         * If true, negative numbers in violation of the specification will not throw an exception.
         */
        public readonly bool allowNegativeNumbers;

        private ParsingMode(bool allowUnknownTags, bool allowNegativeNumbers)
        {
            this.allowUnknownTags = allowUnknownTags;
            this.allowNegativeNumbers = allowNegativeNumbers;
        }

        public class Builder
        {
            private bool mAllowUnknownTags = false;
            private bool mAllowNegativeNumbers = false;

            /**
             * Call to prevent throwing an exception when parsing unrecognized tags.
             */
            public Builder allowUnknownTags()
            {
                mAllowUnknownTags = true;
                return this;
            }

            /**
             * Call to prevent throwing an exception when parsing negative numbers in violation of the specification.
             */
            public Builder allowNegativeNumbers()
            {
                mAllowNegativeNumbers = true;
                return this;
            }

            public ParsingMode build()
            {
                return new ParsingMode(mAllowUnknownTags, mAllowNegativeNumbers);
            }
        }
    }
}
