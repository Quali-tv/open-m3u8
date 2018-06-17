using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    /**
     * Represents a playlist with invalid data.
     */
    public class PlaylistException : Exception
    {
        private const long serialVersionUID = 7426782115004559238L;

        private readonly String mInput;
        private readonly HashSet<PlaylistError> mErrors;

        public PlaylistException(String input, HashSet<PlaylistError> errors)
        {
            mInput = input;
            mErrors = errors;
        }

        public String getInput()
        {
            return mInput;
        }

        public HashSet<PlaylistError> getErrors()
        {
            return mErrors;
        }
    }
}
