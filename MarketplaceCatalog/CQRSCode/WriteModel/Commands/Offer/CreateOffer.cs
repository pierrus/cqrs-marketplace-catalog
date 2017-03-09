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

        public readonly String Name;
        
        public readonly String Description;
        
        public readonly Decimal Price;
        
        public readonly Int16 Stock;

        public CreateOffer(Guid offerId, Guid productId, Guid merchantId,
                        String name, String description, Decimal price,
                        Int16 stock, String SKU) 
		{
            Id = offerId;
            ProductId = productId;
            OfferId = offerId;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }

        public Guid Id { get; set; }

        public int ExpectedVersion { get; set; }
	}
}