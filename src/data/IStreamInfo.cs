using System;
using System.Collections.Generic;
using System.Text;
// import java.util.List;

namespace M3U8Parser
{
    public interface IStreamInfo
    {
        int getBandwidth();
        bool hasAverageBandwidth();
        int getAverageBandwidth();
        bool hasCodecs();
        List<String> getCodecs();
        bool hasResolution();
        Resolution getResolution();
        bool hasFrameRate();
        float getFrameRate();
        bool hasVideo();
        String getVideo();
    }
}
