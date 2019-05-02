#pragma warning disable IDE1006 // Private Static Helper Functions Start with Lower Case
using Google.Protobuf;
using Grpc.Core;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph.NetCore
{
    public partial class Client
    {
        private static TransactionID getOrCreateProtoTransactionID(ContextStack context)
        {
            var preExistingTransaction = context.Transaction;
            if (preExistingTransaction == null)
            {
                var transactionId = createNewProtoTransactionID(context.Payer, DateTime.UtcNow);
                var transaction = fromProtoTransactionId(transactionId);
                foreach (var handler in context.GetAll<Action<Transaction>>(nameof(context.OnTransactionCreated)))
                {
                    handler(transaction);
                }
                return transactionId;
            }
            else
            {
                return toProtoTransactionID(preExistingTransaction);
            }
        }

        private static TransactionID createNewProtoTransactionID(Account payer, DateTime dateTime)
        {
            return new TransactionID
            {
                TransactionValidStart = toProtoTimestamp(dateTime),
                AccountID = toProtoAccountID(payer)
            };
        }
        private static TransferList createProtoTransferList(params (Address address, long amount)[] list)
        {
            var transfers = new TransferList();
            foreach (var transfer in list)
            {
                transfers.AccountAmounts.Add(new AccountAmount
                {
                    AccountID = toProtoAccountID(transfer.address),
                    Amount = transfer.amount
                });
            }
            return transfers;
        }
        private static TransactionBody createProtoTransactionBody(ContextStack context, TransferList transfers, TransactionID transactionId, string defaultMemo)
        {
            return new TransactionBody
            {
                TransactionID = transactionId,
                NodeAccountID = toProtoAccountID(context.Gateway),
                TransactionFee = (ulong)context.Fee,
                TransactionValidDuration = toProtoDuration(context.TransactionDuration),
                Memo = context.Memo ?? defaultMemo ?? "",
                CryptoTransfer = new CryptoTransferTransactionBody
                {
                    Transfers = transfers
                }
            };
        }

        private SignatureList signProtoTransactionBody(TransactionBody transactionBody, params ISigner[] signers)
        {
            var signatures = new SignatureList();
            var bytes = toProtoBytes(transactionBody);
            foreach (var signer in signers)
            {
                signatures.Sigs.Add(new Signature { Ed25519 = ByteString.CopyFrom(signer.Sign(bytes)) });
            }
            return signatures;
        }
        private static QueryHeader createProtoQueryHeader(TransactionBody transactionBody, SignatureList signatures)
        {
            return new QueryHeader
            {
                Payment = new Proto.Transaction
                {
                    Body = transactionBody,
                    Sigs = signatures
                }
            };
        }

        private static async Task<TResponse> executeQueryWithRetryAsync<TResponse>(ContextStack context, Query query, Func<Channel, Func<Query, Task<TResponse>>> instantiateQueryNetworkMethod, Func<TResponse, ResponseHeader> extractHeaderFromResponse)
        {
            var channel = new Channel(context.Gateway.Url, ChannelCredentials.Insecure);
            try
            {
                var queryNetwork = instantiateQueryNetworkMethod(channel);
                var response = await queryNetwork(query);
                var header = extractHeaderFromResponse(response);
                if (responseContainsRetryableErrorCode(header))
                {
                    var maxRetries = context.BusyRetryCount;
                    var retryStandoff = context.BusyRetryDelay;
                    for (var retryCount = 0; retryCount < maxRetries && responseContainsRetryableErrorCode(header); retryCount++)
                    {
                        await Task.Delay(retryStandoff);
                        response = await queryNetwork(query);
                        header = extractHeaderFromResponse(response);
                    }
                }
                return response;
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        private static bool responseContainsRetryableErrorCode(ResponseHeader header)
        {
            return
                header.NodeTransactionPrecheckCode == ResponseCodeEnum.Busy ||
                header.NodeTransactionPrecheckCode == ResponseCodeEnum.InvalidTransactionStart;
        }
    }
}
