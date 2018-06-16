using System;
using System.Collections.Generic;
using System.Linq;

namespace M3U8Parser
{
    public static class ExtensionMethods
    {
        public static bool SequenceEquals<T>(this List<T> theList, List<T> theOtherList){
            return theList == null && theOtherList == null ||
                  (theList != null && theOtherList != null &&
                   Enumerable.SequenceEqual(theList, theOtherList));
        }

        public static bool isEmpty<T>(this List<T> theList){
            return theList.Count == 0;
        }

        public static bool isEmpty<T>(this HashSet<T> theSet){
            return theSet.Count == 0;
        }

        public static bool isEmpty(this string str){
            return string.IsNullOrEmpty(str);
        }
    }
}
