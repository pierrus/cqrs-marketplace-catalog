using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CQRSWeb.Models
{
    public class Offer
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid MerchantId { get; set; }

        //Was it activated by administrator
        public Boolean IsActivated { get; set; }

        //IsActivated, is merchantActivated, is stock available
        public Boolean IsVisible { get; set; }

        [Required]
        public Int16 Stock { get; set; }

        [Required]
        public Decimal Price { get; set; }

        public String SKU { get; set; }

        public IList<Merchant> Merchants { get; set; }

        public String ProductName { get; set; }
        
    }
}