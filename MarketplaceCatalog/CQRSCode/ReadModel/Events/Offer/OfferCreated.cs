using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class OfferCreated : IEvent
	{
        public Guid OfferId { get; set; }
        public Guid MerchantId { get; set; }
        public Int16 Stock { get; set; }
        public Decimal Price { get; set; }
        public Boolean MerchantActivated { get; set; }
        public String SKU { get; set; }
        public String MerchantName { get; set; }
        public Boolean IsActivated { get; set; }
        public Boolean IsVisible { get; set; }
        
        
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