using System;
using System.Collections.Generic;

namespace CQRSCode.ReadModel.Dtos
{
    public class ProductSummaryDto : EntityBase
    {
        public Int32 Version { get; set; }

        public Guid CategoryId { get; set; }

        public Guid RootCategoryId { get; set; }

        public List<Guid> Categories { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }


        //Product is removed from collection if not visible. Following properties are useless

        //Was it activated by administrator
        //public Boolean IsActivated { get; set; }

        //IsActivated, has visible offers
        //public Boolean IsVisible { get; set; }

        public Int32 VisibleOffers { get; set; }

        public List<Decimal> Prices { get; set; }
    }
}