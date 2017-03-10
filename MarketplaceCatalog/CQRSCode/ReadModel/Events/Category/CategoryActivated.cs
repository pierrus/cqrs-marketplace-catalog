using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class CategoryActivated : IEvent
	{                
        public CategoryActivated(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}