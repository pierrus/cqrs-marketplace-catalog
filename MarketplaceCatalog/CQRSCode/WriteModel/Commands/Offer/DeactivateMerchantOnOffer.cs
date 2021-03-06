﻿using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class DeactivateMerchantOnOffer : ICommand 
	{
        public Guid ProductId { get; set; }

        public Guid MerchantId { get; set; }

        public DeactivateMerchantOnOffer(Guid id, Guid productId, Guid merchantId) 
		{
            Id = id;
            ProductId = productId;
            MerchantId = merchantId;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}