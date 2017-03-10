using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class ProductUnpublishedFromCategory : IEvent 
	{
        public Guid CategoryId { get; set; }

        public ProductUnpublishedFromCategory(Guid id, Guid categoryId) 
        {
            Id = id;
            CategoryId = categoryId;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}