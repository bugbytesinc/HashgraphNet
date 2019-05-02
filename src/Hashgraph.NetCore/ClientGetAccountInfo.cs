using Grpc.Core;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph.NetCore
{
    public partial class Client
    {
        public async Task<AccountInfo> GetAccountInfoAsync(Address address, Action<IContext>? configure = null)
        {
            requireAddressArgument(address);
            var context = createChildContext(configure);
            requireGatewayInContext(context);
            requirePayerInContext(context);
            var transfers = createProtoTransferList((context.Payer, -context.Fee), (context.Gateway, context.Fee));
            var transactionId = getOrCreateProtoTransactionID(context);
            var transactionBody = createProtoTransactionBody(context, transfers, transactionId, "Get Account Info");
            var signatures = signProtoTransactionBody(transactionBody, context.Payer);
            var query = new Query
            {
                CryptoGetInfo = new CryptoGetInfoQuery                
                {
                    Header = createProtoQueryHeader(transactionBody, signatures),
                    AccountID = toProtoAccountID(address)
                }
            };
            var data = await executeQueryWithRetryAsync(context, query, instantiateExecuteCryptoGetInfoAsyncMethod, extractCryptoGetInfoResponseHeader);
            validatePreCheckResult(data.Header);
            return fromAccountInfo(data.AccountInfo);

            Func<Query, Task<CryptoGetInfoResponse>> instantiateExecuteCryptoGetInfoAsyncMethod(Channel channel)
            {
                var client = new CryptoService.CryptoServiceClient(channel);
                return executeCryptoGetInfoAsync;

                async Task<CryptoGetInfoResponse> executeCryptoGetInfoAsync(Query query)
                {
                    return (await client.getAccountInfoAsync(query)).CryptoGetInfo;
                }
            }

            static ResponseHeader extractCryptoGetInfoResponseHeader(CryptoGetInfoResponse response)
            {
                return response.Header;
            }
        }
    }
}
