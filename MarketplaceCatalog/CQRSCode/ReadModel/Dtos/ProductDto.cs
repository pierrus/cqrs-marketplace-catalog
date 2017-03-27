using System;
using System.Collections.Generic;

namespace CQRSCode.ReadModel.Dtos
{
    public class ProductDto : EntityBase
    {
        public Int32 Version { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? RootCategoryId { get; set; }

        public List<Guid> Categories { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        //Was it activated by administrator
        public Boolean IsActivated { get; set; }

        //IsActivated, has visible offers
        public Boolean IsVisible { get; set; }

        public List<OfferDto> Offers { get; set; }

        public String EAN { get; set; }

        public String UPC { get; set; }

        public ProductDto(Guid id, Guid? categoryId, String name,
                            String description, Boolean isActivated,
                            Boolean isVisible, Int32 version,
                            String ean = null, String upc = null)
        {
            Id = id;
            CategoryId = categoryId;
            Name = name;
            Description = description;
            IsActivated = isActivated;
            IsVisible = isVisible;
            Version = version;
            EAN = ean;
            UPC = upc;
        }
    }
}