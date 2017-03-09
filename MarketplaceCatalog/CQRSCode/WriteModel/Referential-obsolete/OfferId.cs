using System;

namespace CQRSCode.WriteModel.Referential
{
    public class OfferId
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid MerchandId { get; set; }

        public String SKU { get; set; }
    }
}