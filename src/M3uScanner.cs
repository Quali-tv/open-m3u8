using System;
using System.IO;
using System.Text;
//import java.io.InputStream;
//import java.util.Locale;
//import java.util.Scanner;

namespace M3U8Parser
{
    public class M3uScanner
    {
        private readonly StreamReader mScanner;
        private readonly bool mSupportsByteOrderMark;
        private readonly StringBuilder mInput = new StringBuilder();

        private bool mCheckedByteOrderMark;

        public M3uScanner(Stream inputStream, Encoding encoding)
        {
            mScanner = new StreamReader(inputStream, encoding.value);
            mSupportsByteOrderMark = encoding.supportsByteOrderMark;
        }

        public String getInput()
        {
            return mInput.ToString();
        }

        public bool hasNext()
        {
            return !mScanner.EndOfStream;
        }

        public String next() //throws ParseException 
        {
            String line = mScanner.ReadLine();

            if (mSupportsByteOrderMark && !mCheckedByteOrderMark)
            {
                if (!string.IsNullOrEmpty(line) && line.ToCharArray()[0] == Constants.UNICODE_BOM)
                {
                    line = line.Substring(1);
                }

                mCheckedByteOrderMark = true;
            }

            mInput.Append(line).Append("\n");
            return line;
        }
    }
}
