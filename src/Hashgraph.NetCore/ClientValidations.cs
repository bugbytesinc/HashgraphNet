#pragma warning disable IDE1006 // Private Static Helper Functions Start with Lower Case
using Proto;
using System;

namespace Hashgraph.NetCore
{
    public partial class Client
    {
        private static void requirePayerInContext(ContextStack context)
        {
            if (context.Payer == null)
            {
                throw new InvalidOperationException("The Payer account has not been configured. Please check that 'Payer' is set in the Client context.");
            }
        }

        private static void requireGatewayInContext(ContextStack context)
        {
            if (context.Gateway == null)
            {
                throw new InvalidOperationException("Network Gateway Node has not been configured. Please check that 'Gateway' is set in the Client context.");
            }
        }

        private static void requireAddressArgument(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Account Address is is missing. Please check that it is not null.");
            }
        }

        private void validatePreCheckResult(ResponseHeader header)
        {
            if (header.NodeTransactionPrecheckCode == ResponseCodeEnum.Ok)
            {
                return;
            }
            throw new GatewayException($"Failed Pre-Check: {header.NodeTransactionPrecheckCode}", (PrecheckResponseCode)header.NodeTransactionPrecheckCode);
        }

    }
}
