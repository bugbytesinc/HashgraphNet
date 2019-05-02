﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hashgraph.Net.Web.Models
{
    public class GetBalanceRequestModel
    {
        [Required(ErrorMessage = "The Test Network Address is required.")]
        public string GatewayName { get; set; }
        [Required(ErrorMessage = "The Test Network Port is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid Test Network Port is required.")]
        public int GatewayPort { get; set; }
        [Required(ErrorMessage = "The Node Realm is Required.")]
        [Range(0, int.MaxValue, ErrorMessage = "A valid Node Realm value is required.")]
        public long GatewayRealmNum { get; set; }
        [Required(ErrorMessage = "The Node Shard is Required.")]
        [Range(0, int.MaxValue, ErrorMessage = "A valid Node Shard value is required.")]
        public long GatewayShardNum { get; set; }
        [Required(ErrorMessage = "The Node Account Number is Required.")]
        [Range(3, int.MaxValue, ErrorMessage = "A valid Node account number value is required.")]
        public long GatewayAccountNum { get; set; }
        [Required(ErrorMessage = "The Payer Realm is Required.")]
        [Range(0, int.MaxValue, ErrorMessage = "A valid Payer Realm value is required.")]
        public long PayerRealmNum { get; set; }
        [Required(ErrorMessage = "The Payer Shard is Required.")]
        [Range(0, int.MaxValue, ErrorMessage = "A valid Payer Shard value is required.")]
        public long PayerShardNum { get; set; }
        [Required(ErrorMessage = "The Payer Account Number is Required.")]
        [Range(3, int.MaxValue, ErrorMessage = "A valid Payer account number value is required.")]
        public long PayerAccountNum { get; set; }
        [Required(ErrorMessage = "The paying account private key is required.")]
        [DataType(DataType.Password)]
        public string PayerPrivateKey { get; set; }
        [Required(ErrorMessage = "The Account Realm is Required.")]
        [Range(0, int.MaxValue, ErrorMessage = "A valid Account Realm value is required.")]
        public long AccountRealmNum { get; set; }
        [Required(ErrorMessage = "The Account Shard is Required.")]
        [Range(0, int.MaxValue, ErrorMessage = "A valid Account Shard value is required.")]
        public long AccountShardNum { get; set; }
        [Required(ErrorMessage = "The Account Number is Required.")]
        [Range(3, int.MaxValue, ErrorMessage = "A valid account number value is required.")]
        public long AccountAccountNum { get; set; }

    }
}
