using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class OfferCreated : IEvent 
	{
        public readonly Guid OfferId;
        public readonly Guid MerchantId;
        public readonly Int16 Stock;
        public readonly Decimal Price;
        public readonly Boolean MerchantActivated;
        public readonly String SKU;
        public readonly String MerchantName;
        public readonly Boolean IsActivated;
        public readonly Boolean IsVisible;
        
        
        public OfferCreated(Guid id, Guid offerId, Guid merchantId, Int16 stock, Decimal price,
                            Boolean merchantActivated, String sku, String merchantName,
                            Boolean isActivated, Boolean isVisible)
        {
            Id = id;
            OfferId = offerId;
            MerchantId = merchantId;
            Stock = stock;
            Price = price;
            MerchantActivated = merchantActivated;
            SKU = sku;
            MerchantName = merchantName;
            IsVisible = isVisible;
            IsActivated = isActivated;
        }

        // ProductId --> Event applies on product aggregate
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}