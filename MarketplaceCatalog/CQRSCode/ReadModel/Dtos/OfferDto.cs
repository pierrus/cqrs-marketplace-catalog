using System;

namespace CQRSCode.ReadModel.Dtos
{
    public class OfferDto : EntityBase
    {
        public int Version { get; set; }

        public Guid MerchantId { get; set; }

        public String MerchantName { get; set; }

        //Was it activated by administrator
        public Boolean IsActivated { get; set; }

        //IsActivated, is merchantActivated, is stock available
        public Boolean IsVisible { get; set; }

        public Int16 Stock { get; set; }

        public Decimal Price { get; set; }

        public String SKU { get; set; }

        public OfferDto(Guid id, Guid merchantId, String merchantName,
                            Boolean isActivated, Boolean isVisible,
                            Int16 stock, Decimal price, Int32 version, String sku)
        {
            Id = id;
            MerchantId = merchantId;
            MerchantName = merchantName;
            IsActivated = isActivated;
            IsVisible = isVisible;
            Stock = stock;
            Price = price;
            Version = version;
            SKU = sku;
        }
    }
}