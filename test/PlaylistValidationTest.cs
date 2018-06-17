using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace M3U8Parser
{
    public class PlaylistValidationTest
    {
        [Fact]
        public void testAllowNegativeNumbersValidation() // throws Exception 
        {
            Playlist playlist = null;
            bool found = false;

            try
            {
                Stream inputStream = TestUtil.inputStreamFromResource("negativeDurationMediaPlaylist.m3u8");
                new PlaylistParser(inputStream, Format.EXT_M3U, Encoding.UTF_8).parse();
            }
            catch (PlaylistException exception)
            {
                found = exception.getErrors().Contains(PlaylistError.TRACK_INFO_WITH_NEGATIVE_DURATION);
            }

            Assert.True(found);

            using(Stream inputStream = TestUtil.inputStreamFromResource("negativeDurationMediaPlaylist.m3u8"))
            {
                playlist = new PlaylistParser(inputStream, Format.EXT_M3U, Encoding.UTF_8, ParsingMode.LENIENT).parse();
            }

            Assert.Equal(-1f, playlist.getMediaPlaylist().getTracks()[0].getTrackInfo().duration);
        }

        [Fact]
        public void testInvalidBytRange() // throws Exception 
        {
            List<PlaylistError> errors = new List<PlaylistError>();
            try
            {
                TestUtil.parsePlaylistFromResource("mediaPlaylistWithInvalidByteRanges.m3u8");
            }
            catch (PlaylistException e)
            {
                errors.AddRange(e.getErrors());
            }
            Assert.Equal(new List<PlaylistError>() { PlaylistError.BYTERANGE_WITH_UNDEFINED_OFFSET }, errors);
        }
    }
}
