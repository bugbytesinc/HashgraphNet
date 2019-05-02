using System;

namespace Hashgraph.NetCore
{
    public class GatewayException : Exception
    {
        public PrecheckResponseCode PrecheckResponseCode { get; private set; }
        public GatewayException(string message, PrecheckResponseCode code) : base(message)
        {
            PrecheckResponseCode = code;
        }
        public GatewayException(string message, PrecheckResponseCode code, Exception innerException) : base(message, innerException)
        {
            PrecheckResponseCode = code;
        }
    }
}
