using System.ComponentModel.DataAnnotations;

namespace CQRSWeb.Models
{
    public class Product
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string EAN { get; set; }

        public string UPC { get; set; }
        
    }
}