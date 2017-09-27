using System;
using System.ComponentModel.DataAnnotations;

namespace CQRSWeb.Models
{
    public class Merchant
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Boolean IsActivated { get; set; }

        [Required]
        public UInt16 Commission { get; set; }
                
    }
}