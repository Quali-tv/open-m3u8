using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    public class TestUtil
    {
        public static Stream inputStreamFromResource(String fileName)
        {
            Assert.NotNull(fileName);

            try
            {
                return new FileStream("resources/" + fileName, FileMode.Open);
            }
            catch (FileNotFoundException e)
            {
                throw new Exception("failed to open playlist file: " + fileName);
            }
        }

        public static Playlist parsePlaylistFromResource(String fileName)
        {
            Assert.NotNull(fileName);

            using(Stream ins = inputStreamFromResource(fileName))
            {
                return new PlaylistParser(ins, Format.EXT_M3U, Encoding.UTF_8).parse();
            }
        }
    }
}
