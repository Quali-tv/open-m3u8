using System;
using System.Collections.Generic;
using System.Text;
// import java.util.HashMap;
// import java.util.Map;

namespace M3U8Parser
{
    public class EncryptionMethod
    {
        public static readonly EncryptionMethod NONE = new EncryptionMethod("NONE");
        public static readonly EncryptionMethod AES = new EncryptionMethod("AES-128");
        public static readonly EncryptionMethod SAMPLE_AES = new EncryptionMethod("SAMPLE-AES");

        private static readonly Dictionary<String, EncryptionMethod> sMap = new Dictionary<String, EncryptionMethod>();

        private readonly String value;

        static EncryptionMethod()
        {
            sMap.Add(NONE.value, NONE);
            sMap.Add(AES.value, AES);
            sMap.Add(SAMPLE_AES.value, SAMPLE_AES);
        }

        private EncryptionMethod(String value)
        {
            this.value = value;
        }

        public static EncryptionMethod fromValue(String value)
        {
            return sMap[value];
        }

        public String getValue()
        {
            return this.value;
        }
    }
}
