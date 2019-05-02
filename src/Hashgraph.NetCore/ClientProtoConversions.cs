#pragma warning disable IDE1006 // Private Static Helper Functions Start with Lower Case
using Google.Protobuf;
using Proto;
using System;
using System.Text;

namespace Hashgraph.NetCore
{
    public partial class Client
    {
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly long NanosPerTick = 1_000_000_000L / TimeSpan.TicksPerSecond;

        private static Transaction fromProtoTransactionId(TransactionID transactionId)
        {
            return new Transaction(toProtoBytes(transactionId));
        }

        private static TransactionID toProtoTransactionID(Transaction transaction)
        {
            return TransactionID.Parser.ParseFrom((transaction as IData).Data.ToArray());
        }

        private static AccountID toProtoAccountID(Address address)
        {
            return new AccountID
            {
                RealmNum = address.RealmNum,
                ShardNum = address.ShardNum,
                AccountNum = address.AccountNum
            };
        }
        private static Duration toProtoDuration(TimeSpan timespan)
        {
            long seconds = (long)timespan.TotalSeconds;
            int nanos = (int)((timespan.Ticks - (seconds * TimeSpan.TicksPerSecond)) * NanosPerTick);
            return new Duration
            {
                Seconds = seconds,
                Nanos = nanos
            };
        }
        private static TimeSpan fromProtoDuration(Duration duration)
        {
            return TimeSpan.FromTicks(duration.Seconds * TimeSpan.TicksPerSecond + duration.Nanos / NanosPerTick);
        }
        private static Timestamp toProtoTimestamp(DateTime dateTime)
        {
            TimeSpan timespan = dateTime - EPOCH;
            long seconds = (long)timespan.TotalSeconds;
            int nanos = (int)((timespan.Ticks - (seconds * TimeSpan.TicksPerSecond)) * NanosPerTick);
            return new Timestamp
            {
                Seconds = seconds,
                Nanos = nanos
            };
        }
        private static DateTime fromProtoTimestamp(Timestamp timestamp)
        {
            return EPOCH.AddTicks(timestamp.Seconds * TimeSpan.TicksPerSecond + timestamp.Nanos / NanosPerTick);
        }
        private static ReadOnlySpan<byte> toProtoBytes(IMessage message)
        {
            var size = message.CalculateSize();
            var buffer = new byte[size];
            using (var stream = new CodedOutputStream(buffer))
            {
                message.WriteTo(stream);
            }
            return buffer;
        }
        private static AccountInfo fromAccountInfo(CryptoGetInfoResponse.Types.AccountInfo accountInfo)
        {
            var accountId = accountInfo.AccountID;
            var proxyId = accountInfo.ProxyAccountID;
            return new AccountInfo(
                new Address(accountId.RealmNum, accountId.ShardNum, accountId.AccountNum),
                accountInfo.ContractAccountID,
                accountInfo.Deleted,
                new Address(proxyId.RealmNum, proxyId.ShardNum, proxyId.AccountNum),
                accountInfo.ProxyFraction,
                accountInfo.ProxyReceived,
                encodeByteArrayToHexString(accountInfo.Key.Ed25519.ToByteArray()),
                accountInfo.Balance,
                accountInfo.GenerateSendRecordThreshold,
                accountInfo.GenerateReceiveRecordThreshold,
                accountInfo.ReceiverSigRequired,
                fromProtoTimestamp(accountInfo.ExpirationTime),
                fromProtoDuration(accountInfo.AutoRenewPeriod));
        }

        private static string encodeByteArrayToHexString(ReadOnlySpan<byte> bytes)
        {
            var size = bytes.Length * 2;
            if(size == 0)
            {
                return string.Empty;
            }
            var buff = new StringBuilder(size, size);
            for(int i = 0; i < bytes.Length; i ++ )
            {
                buff.AppendFormat("{0:x2}", bytes[i]);
            }
            return buff.ToString();
        }
    }
}
