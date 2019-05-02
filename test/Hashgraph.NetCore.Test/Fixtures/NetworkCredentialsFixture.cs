﻿using Hashgraph.NetCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hashgrap.Net.Test.Fixtures
{
    public class NetworkCredentialsFixture
    {
        private readonly IConfiguration _configuration;

        public string NetworkAddress { get { return _configuration["network:address"]; } }
        public int NetworkPort { get { return getAsInt("network:port"); } }
        public long ServerRealm { get { return getAsInt("server:realm"); } }
        public long ServerShard { get { return getAsInt("server:shard"); } }
        public long ServerNumber { get { return getAsInt("server:number"); } }
        public long AccountRealm { get { return getAsInt("account:realm"); } }
        public long AccountShard { get { return getAsInt("account:shard"); } }
        public long AccountNumber { get { return getAsInt("account:number"); } }
        public string AccountPrivateKey { get { return _configuration["account:privateKey"]; } }
        public string AccountPublicKey { get { return _configuration["account:publicKey"]; } }

        public NetworkCredentialsFixture()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddUserSecrets<NetworkCredentialsFixture>(true)
                .Build();
        }

        public Account CreateDefaultAccount()
        {
            return new Account(AccountRealm, AccountShard, AccountNumber, AccountPrivateKey);
        }

        public Gateway CreateDefaultGateway()
        {
            return new Gateway($"{NetworkAddress}:{NetworkPort}", ServerRealm, ServerShard, ServerNumber);

        }

        public Client CreateClientWithDefaultConfiguration()
        {
            return new Client(ctx =>
            {
                ctx.Gateway = CreateDefaultGateway();
                ctx.Payer = CreateDefaultAccount();
            });
        }

        private int getAsInt(string key)
        {
            var valueAsString = _configuration[key];
            if (int.TryParse(valueAsString, out int value))
            {
                return value;
            }
            throw new InvalidProgramException($"Unable to convert configuration '{key}' of '{valueAsString}' into an integer value.");
        }

        [CollectionDefinition(nameof(NetworkCredentialsFixture))]
        public class FixtureCollection : ICollectionFixture<NetworkCredentialsFixture>
        {
        }
    }
}