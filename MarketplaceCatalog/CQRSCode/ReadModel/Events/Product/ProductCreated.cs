using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class ProductCreated : IEvent 
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public string EAN { get; set; }
        public string UPC { get; set; }
        public Boolean IsActivated { get; set; }
        public Boolean IsVisible { get; set; }
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