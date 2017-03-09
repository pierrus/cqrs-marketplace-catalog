using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class OfferMerchantActivated : IEvent 
	{
        public readonly Guid OfferId;
        public readonly Guid MerchantId;
        
        
        public OfferMerchantActivated(Guid id, Guid offerId, Guid merchantId) 
        {
            Id = id;
            OfferId = offerId;
            MerchantId = merchantId;
        }

        // ProductId --> Event applies on product aggregate
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}