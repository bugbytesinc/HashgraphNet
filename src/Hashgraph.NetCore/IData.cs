using System;

namespace Hashgraph.NetCore
{
    internal interface IData
    {
        ReadOnlySpan<byte> Data { get; }
    }
}
