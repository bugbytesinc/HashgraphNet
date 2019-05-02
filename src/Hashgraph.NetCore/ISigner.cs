using System;

namespace Hashgraph.NetCore
{
    internal interface ISigner
    {
        byte[] Sign(ReadOnlySpan<byte> data);
    }
}
