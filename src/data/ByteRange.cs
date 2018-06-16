using System;
using System.Text;

namespace M3U8Parser
{
//package com.iheartradio.m3u8.data;

//import java.util.Objects;

public class ByteRange {
    private readonly long mSubRangeLength;
    private readonly long? mOffset;

    public ByteRange(long subRangeLength, long? offset) {
        this.mSubRangeLength = subRangeLength;
        this.mOffset = offset;
    }

    public ByteRange(long subRangeLength)  : this(subRangeLength, null)    {    }

    public long getSubRangeLength() {
        return mSubRangeLength;
    }

    public long? getOffset() {
        return mOffset;
    }

    public bool hasOffset() {
        return mOffset != null;
    }

    public override String ToString() {
        return "ByteRange{" +
                "mSubRangeLength=" + mSubRangeLength +
                ", mOffset=" + mOffset +
                '}';
    }

    public override bool Equals(Object o) {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;
        ByteRange byteRange = (ByteRange) o;
        return mSubRangeLength == byteRange.mSubRangeLength &&
                object.Equals(mOffset, byteRange.mOffset);
    }

    public override int GetHashCode() {
        // TODO: Implement
        //return Objects.hash(mSubRangeLength, mOffset);
        return 0;
    }
}
}