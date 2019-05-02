using Grpc.Core;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph.NetCore
{
    public partial class Client
    {
        public async Task<ulong> GetAccountBalanceAsync(Address address, Action<IContext>? configure = null)
        {
            requireAddressArgument(address);
            var context = createChildContext(configure);
            requireGatewayInContext(context);
            requirePayerInContext(context);
            var transfers = createProtoTransferList((context.Payer, -context.Fee), (context.Gateway, context.Fee));
            var transactionId = getOrCreateProtoTransactionID(context);
            var transactionBody = createProtoTransactionBody(context, transfers, transactionId, "Get Account Balance");
            var signatures = signProtoTransactionBody(transactionBody, context.Payer);
            var query = new Query
            {
                CryptogetAccountBalance = new CryptoGetAccountBalanceQuery
                {
                    Header = createProtoQueryHeader(transactionBody, signatures),
                    AccountID = toProtoAccountID(address)
                }
            };
            var data = await executeQueryWithRetryAsync(context, query, instantiateExecuteCryptoGetBalanceAsyncMethod, extractCryptoAccountBalanceResponseHeader);
            validatePreCheckResult(data.Header);
            return data.Balance;

            Func<Query, Task<CryptoGetAccountBalanceResponse>> instantiateExecuteCryptoGetBalanceAsyncMethod(Channel channel)
            {
                var client = new CryptoService.CryptoServiceClient(channel);
                return executeCryptoGetBalanceAsync;

                async Task<CryptoGetAccountBalanceResponse> executeCryptoGetBalanceAsync(Query query)
                {
                    return (await client.cryptoGetBalanceAsync(query)).CryptogetAccountBalance;
                }
            }

            static ResponseHeader extractCryptoAccountBalanceResponseHeader(CryptoGetAccountBalanceResponse response)
            {
                return response.Header;
            }
        }
    }
}
