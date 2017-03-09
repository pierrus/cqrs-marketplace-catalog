using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    //Aggregate event as a result of merchant, product, offer business rules on the write side
    public class ProductDisplayed : IEvent 
	{
        public ProductDisplayed(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}