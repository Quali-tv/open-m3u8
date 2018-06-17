using System;
using System.Collections.Generic;

namespace M3U8Parser
{
    public interface StreamInfoBuilder
    {
        StreamInfoBuilder withBandwidth(int bandwidth);
        StreamInfoBuilder withAverageBandwidth(int averageBandwidth);
        StreamInfoBuilder withCodecs(List<String> codecs);
        StreamInfoBuilder withResolution(Resolution resolution);
        StreamInfoBuilder withFrameRate(float frameRate);
        StreamInfoBuilder withVideo(String video);
    }
}
