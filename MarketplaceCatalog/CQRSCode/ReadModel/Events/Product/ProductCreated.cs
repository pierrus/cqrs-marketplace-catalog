using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class ProductCreated : IEvent 
	{
        public readonly string Name;
        public readonly string Description;
        public readonly string EAN;
        public readonly string UPC;
        public readonly Boolean IsActivated;
        public readonly Boolean IsVisible;
        public ProductCreated(Guid id, string name, String description, Boolean isActivated,
                                Boolean isVisible, String ean = null, String upc = null) 
        {
            Id = id;
            Name = name;
            Description = description;
            EAN = ean;
            UPC = upc;
            IsActivated = isActivated;
            IsVisible = isVisible;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}