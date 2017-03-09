using System;

namespace CQRSCode.WriteModel.Referential
{
    public class ProductId
    {
        public Guid Id { get; set; }

        public String EAN { get; set; }

        public String ISBN { get; set; }

        public String Title { get; set; }

        public String Brand { get; set; }
    }
}