using System;
using System.IO;
using System.Text;  namespace M3U8Parser {

// import java.io.IOException;
// import java.io.OutputStream;

// import com.iheartradio.m3u8.data.Playlist;

// import static com.iheartradio.m3u8.Constants.UTF_8_BOM_BYTES;

public class PlaylistWriter {
    private readonly Writer mWriter;
    private readonly Stream mOutputStream;
    private readonly bool mShouldWriteByteOrderMark;

    private bool mFirstWrite = true;

    /**
     * This writes the given Playlist to the given OutputStream with a prepended BOM (Byte Order Mark).
     * This exists for convenience if you absolutely need a BOM.
     * From the specification: "They (playlist files) MUST NOT contain any byte order mark (BOM); Clients SHOULD reject Playlists which contain a BOM..."
     *
     * @param outputStream OutputStream playlists will be written to
     * @param format format in which to write the playlist
     * @param encoding encoding in which to write the playlist
     */
    public PlaylistWriter(Stream outputStream, Format format, Encoding encoding) :
        this(outputStream, format, encoding, false)    {    }

    private PlaylistWriter(Stream outputStream, Format format, Encoding encoding, bool useByteOrderMark) {
        if (outputStream == null) {
            throw new ArgumentNullException("outputStream is null");
        }

        if (format == null) {
            throw new ArgumentNullException("format is null");
        }

        if (encoding == null) {
            throw new ArgumentNullException("encoding is null");
        }

        mOutputStream = outputStream;
        mShouldWriteByteOrderMark = encoding.supportsByteOrderMark && useByteOrderMark;

        switch (format) {
            case Format.M3U:
                mWriter = new M3uWriter(outputStream, encoding);
                break;
            case Format.EXT_M3U:
                mWriter = new ExtendedM3uWriter(outputStream, encoding);
                break;
            default:
                throw new Exception("unsupported format detected, this should be impossible: " + format);
        }
    }

    /**
     * Writes the given Playlist to the contained OutputStream.
     *
     * @throws IOException
     * @throws ParseException if the data is improperly formatted
     * @throws PlaylistException if the representation of the playlist is invalid,
     *                           that is, if PlaylistValidation.from(playlist).isValid() == false
     */
    public void write(Playlist playlist) // throws IOException, ParseException, PlaylistException 
    {
        PlaylistValidation validation = PlaylistValidation.from(playlist);

        if (!validation.isValid()) {
            throw new PlaylistException("", validation.getErrors());
        }

        writeByteOrderMark();
        mWriter.write(playlist);
        mFirstWrite = false;
    }

    private void writeByteOrderMark() // throws IOException 
    {
        if (mShouldWriteByteOrderMark && mFirstWrite) {
            for (int i = 0; i < Constants.UTF_8_BOM_BYTES.Length; ++i) {
                mOutputStream.WriteByte(Constants.UTF_8_BOM_BYTES[i]);
            }
        }
    }

    public class Builder {
        private Stream mOutputStream;
        private Format mFormat;
        private Encoding mEncoding;
        private bool mUseByteOrderMark;

        /**
         * @param outputStream OutputStream playlists will be written to
         */
        public Builder withOutputStream(Stream outputStream) {
            mOutputStream = outputStream;
            return this;
        }

        /**
         * @param format format in which to write the playlist
         */
        public Builder withFormat(Format format) {
            mFormat = format;
            return this;
        }

        /**
         * @param encoding encoding in which to write the playlist
         */
        public Builder withEncoding(Encoding encoding) {
            mEncoding = encoding;
            return this;
        }

        /**
         * When using a BOM, the first call to write will prepend the BOM character to the playlist. Subsequent
         * calls to write will not prepend the BOM to support writing multiple ENDLIST delimited playlists.
         *
         * @deprecated The specification explicitly says that playlists MUST NOT contain a byte order mark (BOM)
         */
        public Builder useByteOrderMark() {
            mUseByteOrderMark = true;
            return this;
        }

        public PlaylistWriter build() {
            return new PlaylistWriter(mOutputStream, mFormat, mEncoding, mUseByteOrderMark);
        }
    }
}
}
