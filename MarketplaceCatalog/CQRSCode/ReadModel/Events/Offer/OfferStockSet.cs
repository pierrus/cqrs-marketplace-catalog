using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class OfferStockSet : IEvent 
	{
        public Guid OfferId { get; set; }
        public Int16 Stock { get; set; }
        
        public OfferStockSet(Guid id, Guid offerId, Int16 stock) 
        {
            Id = id;
            OfferId = offerId;
            Stock = stock;
        }

        // ProductId --> Event applies on product aggregate
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}