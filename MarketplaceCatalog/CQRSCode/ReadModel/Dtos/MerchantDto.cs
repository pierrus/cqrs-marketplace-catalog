using System;

namespace CQRSCode.ReadModel.Dtos
{
    public class MerchantDto : EntityBase
    {
        public int Version { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }        

        //Was it activated by administrator
        public Boolean IsActivated { get; set; }

        //IsActivated and has it visible products
        public Boolean IsVisible { get; set; }

        //Visible products directly
        public Int32 VisibleProducts { get; set; }

        public Int32 TotalProducts { get; set; }

        public MerchantDto(Guid id, String name, String email, Boolean isActivated,
                            Boolean isVisible, int visibleProducts, int totalProducts, int version)
        {
            Id = id;
            Name = name;
            Email = email;
            IsActivated = isActivated;
            IsVisible = isVisible;
            VisibleProducts = visibleProducts;
            Version = version;
            TotalProducts = totalProducts;
        }
    }
}