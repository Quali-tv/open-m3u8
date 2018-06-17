using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Parser
{
    public class DataUtil
    {
        public static List<T> emptyOrUnmodifiable<T>(List<T> list)
        {
            return list == null ? new List<T>() : list; // TODO: .AsReadOnly();
        }
    }
}
