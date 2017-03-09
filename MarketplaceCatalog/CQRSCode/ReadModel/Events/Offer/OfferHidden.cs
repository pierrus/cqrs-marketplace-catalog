using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{

    //Aggregate event as a result of merchant, product, offer business rules on the write side
    public class OfferHidden : IEvent 
	{
        public Guid OfferId { get; set; }

        public OfferHidden(Guid id, Guid offerId)
        {
            Id = id;
            OfferId = offerId;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}