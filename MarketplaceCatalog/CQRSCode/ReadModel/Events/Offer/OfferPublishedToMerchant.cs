using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{

    //Aggregate event as a result of merchant, product, offer business rules on the write side
    public class OfferPublishedToMerchant : IEvent 
	{
        public Guid OfferId { get; set; }

        public Guid MerchantId { get; set; }

        public OfferPublishedToMerchant(Guid id, Guid offerId, Guid merchantId)
        {
            Id = id;
            OfferId = offerId;
            MerchantId = merchantId;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}