using System;
using System.IO;
using System.Text;
using Xunit;
// import com.iheartradio.m3u8.data.Playlist;
// import org.junit.Test;

// import java.io.ByteArrayInputStream;
// import java.io.ByteArrayOutputStream;
// import java.io.IOException;
// import java.io.InputStream;

// import static com.iheartradio.m3u8.TestUtil.inputStreamFromResource;
// import static com.iheartradio.m3u8.Constants.UTF_8_BOM_BYTES;
// import static org.junit.Assert.*;

namespace M3U8Parser
{
    public class ByteOrderMarkTest
    {
        [Fact]
        public void testParsingByteOrderMark() // throws Exception 
        {
            using(Stream inputStream = wrapWithByteOrderMark(TestUtil.inputStreamFromResource("simpleMediaPlaylist.m3u8")))
            {
                PlaylistParser playlistParser = new PlaylistParser(inputStream, Format.EXT_M3U, Encoding.UTF_8);
                Playlist playlist = playlistParser.parse();
                Assert.Equal(10, playlist.getMediaPlaylist().getTargetDuration());
            }
        }

        //@SuppressWarnings("deprecation")
        [Fact]
        public void testWritingByteOrderMark() // throws Exception 
        {
            Playlist playlist1 = null;
            Playlist playlist2 = null;
            String written = String.Empty;

            using(Stream inputStream = TestUtil.inputStreamFromResource("simpleMediaPlaylist.m3u8"))
            {
                playlist1 = new PlaylistParser(inputStream, Format.EXT_M3U, Encoding.UTF_8).parse();
            }

            using(var os = new MemoryStream())
            {
                PlaylistWriter writer = new PlaylistWriter.Builder()
                        .withOutputStream(os)
                        .withFormat(Format.EXT_M3U)
                        .withEncoding(Encoding.UTF_8)
                        .useByteOrderMark()
                        .build();

                writer.write(playlist1);
                
                // written = os.ToString(Encoding.UTF_8.value);
                os.Position = 0;
                using(var sr = new StreamReader(os, System.Text.Encoding.UTF8))
                    written = sr.ReadToEnd();
            }

            Assert.Equal(Constants.UNICODE_BOM, written[0]);

            using(Stream inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(written)))
            {
                playlist2 = new PlaylistParser(inputStream, Format.EXT_M3U, Encoding.UTF_8).parse();
            }

            Assert.Equal(playlist1, playlist2);
        }

        public class WrappedStream : Stream
        {
            private Stream _streamToWrap;
            public WrappedStream(Stream streamToWrap)
            {
                _streamToWrap = streamToWrap;
            }

            private int mNumRead;

            public override bool CanRead => 
                Constants.UTF_8_BOM_BYTES.Length > mNumRead || _streamToWrap.CanRead;

            public override bool CanSeek => _streamToWrap.CanSeek;

            public override bool CanWrite => false;

            public override long Length => _streamToWrap.Length;

            public override long Position { get => _streamToWrap.Position; set => _streamToWrap.Position = value; }

            public override void Flush()
            {
                _streamToWrap.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (count <= 0)
                    return 0;
                    
                if (Constants.UTF_8_BOM_BYTES.Length > mNumRead)
                {
                    buffer[offset] = Constants.UTF_8_BOM_BYTES[mNumRead++];
                    return 1;
                }
                else
                {
                    return _streamToWrap.Read(buffer, offset, count);
                }
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _streamToWrap.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _streamToWrap.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _streamToWrap.Write(buffer, offset, count);
            }
        }

        private static Stream wrapWithByteOrderMark(Stream inputStream)
        {
            return new WrappedStream(inputStream);
        }
    }
}
