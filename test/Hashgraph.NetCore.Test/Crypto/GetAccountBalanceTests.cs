﻿using Hashgrap.Net.Test.Fixtures;
using Hashgraph.NetCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Hashgrap.Net.Test.Crypto
{
    [Collection(nameof(NetworkCredentialsFixture))]
    public class GetAccountBalanceTests
    {
        private readonly NetworkCredentialsFixture _networkCredentials;
        public GetAccountBalanceTests(NetworkCredentialsFixture networkCredentials)
        {
            _networkCredentials = networkCredentials;
        }
        [Fact(DisplayName = "Get Account Balance: Can Get Balance for Account")]
        public async Task CanGetTinybarBalanceForAccountAsync()
        {
            var client = _networkCredentials.CreateClientWithDefaultConfiguration();
            var account = _networkCredentials.CreateDefaultAccount();
            var balance = await client.GetAccountBalanceAsync(account);
            Assert.True(balance > 0, "Account Balance should be greater than zero.");
        }
        [Fact(DisplayName = "Get Account Balance: Can Get Balance for Server Node")]
        public async Task CanGetTinybarBalanceForGatewayAsync()
        {
            var client = _networkCredentials.CreateClientWithDefaultConfiguration();
            var account = _networkCredentials.CreateDefaultGateway();
            var balance = await client.GetAccountBalanceAsync(account);
            Assert.True(balance > 0, "Gateway Account Balance should be greater than zero.");
        }
        [Fact(DisplayName = "Get Account Balance: Missing Gateway Information Throws Exception")]
        public async Task MissingNodeInformationThrowsException()
        {
            var account = _networkCredentials.CreateDefaultAccount();
            var client = new Client(ctx =>
            {
                ctx.Payer = account;
            });
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var balance = await client.GetAccountBalanceAsync(account);
            });
            Assert.StartsWith("Network Gateway Node has not been configured.", ex.Message);
        }
        [Fact(DisplayName = "Get Account Balance: Missing Payer Account Throws Exception")]
        public async Task MissingPayerAccountThrowsException()
        {
            var account = _networkCredentials.CreateDefaultAccount();
            var client = new Client(ctx =>
            {
                ctx.Gateway = _networkCredentials.CreateDefaultGateway();
            });
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var balance = await client.GetAccountBalanceAsync(account);
            });
            Assert.StartsWith("The Payer account has not been configured.", ex.Message);
        }
        [Fact(DisplayName = "Get Account Balance: Missing Account for Balance Check Throws Exception")]
        public async Task MissingBalanceAccountThrowsException()
        {
            var client = _networkCredentials.CreateClientWithDefaultConfiguration();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var balance = await client.GetAccountBalanceAsync(null);
            });
            Assert.StartsWith("Account Address is is missing.", ex.Message);
        }
        [Fact(DisplayName = "Get Account Balance: Invalid Account Address Throws Exception")]
        public async Task InvalidAccountAddressThrowsException()
        {
            var client = _networkCredentials.CreateClientWithDefaultConfiguration();
            var account = new Address(0, 0, 0);
            var ex = await Assert.ThrowsAsync<GatewayException>(async () =>
            {
                var balance = await client.GetAccountBalanceAsync(account);
            });
            Assert.Equal(PrecheckResponseCode.InvalidAccountId,ex.PrecheckResponseCode);
            Assert.StartsWith("Failed Pre-Check: InvalidAccount", ex.Message);
        }
        [Fact(DisplayName = "Get Account Balance: Invalid Node Account Throws Exception")]
        public async Task InvalidGatewayAddressThrowsException()
        {
            var client = _networkCredentials.CreateClientWithDefaultConfiguration();
            client.Configure(cfg => {
                cfg.Gateway = new Gateway($"{_networkCredentials.NetworkAddress}:{_networkCredentials.NetworkPort}", 0, 0, 0);
            });
            var account = _networkCredentials.CreateDefaultAccount();
            var ex = await Assert.ThrowsAsync<GatewayException>(async () =>
            {
                var balance = await client.GetAccountBalanceAsync(account);
            });
            Assert.Equal(PrecheckResponseCode.InvalidNodeAccount, ex.PrecheckResponseCode);
            Assert.StartsWith("Failed Pre-Check: InvalidNodeAccount", ex.Message);
        }
        [Fact(DisplayName = "Get Account Balance: Insufficient Fees Throw Exception")]
        public async Task InsuficientFeesThrowException()
        {
            var client = _networkCredentials.CreateClientWithDefaultConfiguration();
            client.Configure(cfg => {
                cfg.Fee = 0;
            });
            var account = _networkCredentials.CreateDefaultAccount();
            var ex = await Assert.ThrowsAsync<GatewayException>(async () =>
            {
                var balance = await client.GetAccountBalanceAsync(account);
            });
            Assert.Equal(PrecheckResponseCode.InsufficientTxFee, ex.PrecheckResponseCode);
            Assert.StartsWith("Failed Pre-Check: InsufficientTxFee", ex.Message);
        }
    }
}
