using System;
using System.Collections.Generic;
using System.Text;
//import java.io.IOException;
//import java.util.Collections;
//import java.util.List;
//import java.util.Map;
//
//import com.iheartradio.m3u8.data.Playlist;

namespace M3U8Parser
{
    public abstract class ExtTagWriter : IExtTagWriter
    {
        public virtual void write(TagWriter tagWriter, Playlist playlist) //throws IOException, ParseException
        {
            if (!hasData())
            {
                tagWriter.writeTag(getTag());
            }
        }

        public abstract string getTag();

        public abstract bool hasData();

        protected void writeAttributes<T>(TagWriter tagWriter, T attributes, Dictionary<String, AttributeWriter<T>> attributeWriters) //throws IOException, ParseException 
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<String, AttributeWriter<T>> entry in attributeWriters)
            {
                AttributeWriter<T> handler = entry.Value;
                String attribute = entry.Key;
                if (handler.containsAttribute(attributes))
                {
                    String value = null;
                    try
                    {
                        value = handler.write(attributes);
                    }
                    catch(ParseException ex)
                    {
                        throw ParseException.create(ex.type, getTag(), ex.getMessageSuffix());
                    }
                    sb.Append(attribute).Append(Constants.ATTRIBUTE_SEPARATOR).Append(value);
                    sb.Append(Constants.ATTRIBUTE_LIST_SEPARATOR);
                }
            }
            sb.Remove(sb.Length - 1, 1);

            tagWriter.writeTag(getTag(), sb.ToString());
        }


        public static readonly IExtTagWriter EXTM3U_HANDLER = new EXTM3U_HANDLER_CLASS();
        private class EXTM3U_HANDLER_CLASS : ExtTagWriter
        {
            public override String getTag()
            {
                return Constants.EXTM3U_TAG;
            }

            public override bool hasData()
            {
                return false;
            }
        }

        public static readonly IExtTagWriter EXT_UNKNOWN_HANDLER = new EXT_UNKNOWN_HANDLER_CLASS();
        private class EXT_UNKNOWN_HANDLER_CLASS : ExtTagWriter
        {
            public override String getTag()
            {
                return null;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void write(TagWriter tagWriter, Playlist playlist)
            { // throws IOException
                List<String> unknownTags;
                if (playlist.hasMasterPlaylist() && playlist.getMasterPlaylist().hasUnknownTags())
                {
                    unknownTags = playlist.getMasterPlaylist().getUnknownTags();
                }
                else if (playlist.getMediaPlaylist().hasUnknownTags())
                {
                    unknownTags = playlist.getMediaPlaylist().getUnknownTags();
                }
                else
                {
                    unknownTags = new List<String>();
                }
                foreach (String line in unknownTags)
                {
                    tagWriter.writeLine(line);
                }
            }
        }

        public static readonly IExtTagWriter EXT_X_VERSION_HANDLER = new EXT_X_VERSION_HANDLER_CLASS();
        private class EXT_X_VERSION_HANDLER_CLASS : ExtTagWriter
        {

            public override String getTag()
            {
                return Constants.EXT_X_VERSION_TAG;
            }

            public override bool hasData()
            {
                return true;
            }

            public override void write(TagWriter tagWriter, Playlist playlist) // throws IOException 
            {
                tagWriter.writeTag(getTag(), playlist.getCompatibilityVersion().ToString());
            }
        }
    }
}
