using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    ///
    /// Creates or find a product
    /// Creates or updates an offer
    ///
    public class CreateOffer : ICommand 
	{
        public readonly Guid OfferId;
        
        public readonly Guid ProductId;
        
        public readonly Guid MerchantId;

        public readonly String SKU;
        
        public readonly Decimal Price;
        
        public readonly Int16 Stock;

        public CreateOffer(Guid offerId, Guid productId, Guid merchantId,
                        Decimal price, Int16 stock, String SKU = null) 
		{
            Id = offerId;
            ProductId = productId;
            OfferId = offerId;
            Price = price;
            Stock = stock;
            MerchantId = merchantId;
        }

        public Guid Id { get; set; }

        public int ExpectedVersion { get; set; }
	}
}