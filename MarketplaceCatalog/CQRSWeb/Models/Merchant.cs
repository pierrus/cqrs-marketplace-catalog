using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CQRSWeb.Models
{
    public class Merchant
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Boolean IsActivated { get; set; }

        public Boolean IsVisible { get; set; }

        [Required]
        public UInt16 Commission { get; set; }

        public Int32 VisibleOffers { get; set; }

        public Int32 TotalOffers { get; set; }

        public List<Guid> OffersIds { get; set; }

        public List<Guid> VisibleOffersIds { get; set; }

        public int Version { get; set; }
    }
}