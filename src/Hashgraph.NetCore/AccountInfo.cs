﻿using System;

namespace Hashgraph.NetCore
{
    public class AccountInfo
    {
        /// <summary>
        /// The Hedera address of this account.
        /// </summary>
        public Address Address { get; private set; }
        /// <summary>
        /// The identity of the Hedera Account in a form to be
        /// used with smart contracts.  This can also be the
        /// ID of a smart contract instance if this is the account
        /// associated with a smart contract.
        /// </summary>
        public string SmartContractId { get; private set; }
        /// <summary>
        /// <code>True</code> if this account has been deleted.
        /// Its existence in the network will cease after the expiration
        /// date for the account lapses.  It cannot participate in
        /// transactions except to extend the expiration/removal date.
        /// </summary>
        public bool Deleted { get; private set; }
        /// <summary>
        /// The Address of the Account to which this account has staked.
        /// </summary>
        public Address Proxy { get; private set; }
        /// <summary>
        /// Metric indicating the fraction of payments earned by
        /// the proxy node that are shared with this account,
        /// with <code>proxyFraction / 10000</code> going to this account.
        /// </summary>
        public int ProxyShareFraction { get; private set; }
        /// <summary>
        /// The total number of tinybars that are proxy staked to this account.
        /// </summary>
        public long ProxiedToAccount { get; private set; }
        /// <summary>
        /// Constructor, used internally.
        /// </summary>
        public string PublicKeyInHex { get; private set; }
        /// <summary>
        /// Account Balance in Tinybars
        /// </summary>
        public ulong Balance { get; private set; }
        /// <summary>
        /// Threshold in tinybars at which withdraws larger than
        /// this value will automatically trigger the creation of a record 
        /// for the transaction. This account will be charged for the 
        /// record creation.
        /// </summary>
        public ulong SendThresholdCreateRecord { get; private set; }
        /// <summary>
        /// Threshold in tinybars at which deposits larger than
        /// this value will automatically trigger the creation of a 
        /// record for the transaction.  This account will be charged
        /// for the record creation.
        /// </summary>
        public ulong ReceiveThresholdCreateRecord { get; private set; }
        /// <summary>
        /// <code>True</code> if any receipt of funds require
        /// a signature from this account.
        /// </summary>
        public bool ReceiveSignatureRequired { get; private set; }
        /// <summary>
        /// Date this account expires if not renewed.
        /// </summary>
        public DateTime Expiration { get; private set; }
        /// <summary>
        /// Incremental period for auto-renewal of the account. If
        /// account does not have sufficient funds to renew at the
        /// expiration time, it will be renewed for a period of time
        /// the remaining funds can support.  If no funds remain, the
        /// account will be deleted.
        /// </summary>
        public TimeSpan AutoRenewPeriod { get; private set; }
        /// <summary>
        /// Constructor, for internal use.
        /// </summary>
        internal AccountInfo(Address address, string accountContractId, bool deleted, Address proxy, int proxyShareFraction, long proxiedToAccount, string publicKeyInHex, ulong balance, ulong generateSendRecordThreshold, ulong generateReceiveRecordThreshold, bool receiveSignatureRequired, DateTime expiration, TimeSpan autoRenewPeriod)
        {
            Address = address;
            SmartContractId = accountContractId;
            Deleted = deleted;
            Proxy = proxy;
            ProxyShareFraction = proxyShareFraction;
            ProxiedToAccount = proxiedToAccount;
            PublicKeyInHex = publicKeyInHex;
            Balance = balance;
            SendThresholdCreateRecord = generateSendRecordThreshold;
            ReceiveThresholdCreateRecord = generateReceiveRecordThreshold;
            ReceiveSignatureRequired = receiveSignatureRequired;
            Expiration = expiration;
            AutoRenewPeriod = autoRenewPeriod;
        }
    }
}
