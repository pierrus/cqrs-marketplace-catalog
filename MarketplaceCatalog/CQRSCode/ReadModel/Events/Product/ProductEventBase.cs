using System;
using CQRSlite.Events;
using System.Collections.Generic;

namespace CQRSCode.ReadModel.Events
{
    public abstract class ProductEventBase : IEvent 
	{
        public Guid CategoryId;

        public Boolean IsVisible { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Int32 VisibleOffers { get; set; }

        public List<Decimal> Prices { get; set; }
        
        public ProductEventBase(Guid id, Guid categoryId, Boolean isVisible, String name,
                                        String description, Int32 visibleOffers, List<Decimal> prices) 
        {
            Id = id;
            CategoryId = categoryId;
            IsVisible = isVisible;
            Name = name;
            Description = description;
            VisibleOffers = visibleOffers;
            Prices = prices;

        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}