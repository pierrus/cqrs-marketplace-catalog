using System;
using CQRSlite.Events;
using System.Collections.Generic;

namespace CQRSCode.ReadModel.Events
{
    public class ProductCategoryDefined : IEvent 
	{
        public Guid CategoryId { get; set; }

        public List<Guid> CategoriesHierarchy { get; set; }
        
        public ProductCategoryDefined(Guid id, Guid categoryId) 
        {
            Id = id;
            CategoryId = categoryId;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}