using System;

namespace CQRSCode.WriteModel.Domain
{
    public class Offer
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid MerchantId { get; set; }

        public bool Activated { get; set; }

        public bool Visible { get; set; }

        public Int16 Stock { get; set; }

        public Decimal Price { get; set; }

        public Boolean MerchantActivated { get; set; }

        private Offer(){}
        
        public Offer(Guid id, Guid merchantId, Guid productId, Boolean activated, Int16 stock, Decimal price, Boolean merchantActivated)
        {
            Id = id;
            ProductId = productId;
            MerchantId = merchantId;
            Activated = activated;
            Stock = stock;
            MerchantActivated = MerchantActivated;
            Price = price;
        }

        internal Boolean EvaluateVisibility()
        {
            return Activated && Stock > 0 && Price > 0 && MerchantActivated;
        }
    }
}