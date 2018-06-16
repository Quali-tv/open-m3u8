using System;
using System.Text;
namespace M3U8Parser
{
    /**
     * Represents a syntactic error in the input that prevented further parsing.
     */
    public class ParseException : Exception
    {
        private const long serialVersionUID = -2217152001086567983L;

        private readonly String mMessageSuffix;

        public readonly ParseExceptionType type;

        private String mInput;

        public static ParseException create(ParseExceptionType type, String tag)
        {
            return create(type, tag, null);
        }

        public static ParseException create(ParseExceptionType type, String tag, String context)
        {
            StringBuilder builder = new StringBuilder();

            if (tag != null)
            {
                builder.Append(tag);
            }

            if (context != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" - ");
                }

                builder.Append(context);
            }

            if (builder.Length > 0)
            {
                return new ParseException(type, builder.ToString());
            }
            else
            {
                return new ParseException(type);
            }
        }

        public ParseException(ParseExceptionType type) : this(type, null) { }

        public ParseException(ParseExceptionType type, String messageSuffix)
        {
            this.type = type;
            mMessageSuffix = messageSuffix;
        }

        public String getInput()
        {
            return mInput;
        }

        public void setInput(String input)
        {
            mInput = input;
        }

        public String getMessageSuffix()
        {
            return mMessageSuffix;
        }

        public String getMessage()
        {
            if (mMessageSuffix == null)
            {
                return type.message;
            }
            else
            {
                return type.message + ": " + mMessageSuffix;
            }
        }
    }
}
