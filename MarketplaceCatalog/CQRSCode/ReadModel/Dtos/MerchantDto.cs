using System;
using System.Collections.Generic;

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
        public Int32 VisibleOffers { get; set; }

        public Int32 TotalOffers { get; set; }

        public List<Guid> OffersIds { get; set; }

        public List<Guid> VisibleOffersIds { get; set; }

        public MerchantDto(Guid id, String name, String email, Boolean isActivated,
                            Boolean isVisible, int visibleOffers, int totalOffers, 
                            List<Guid> offersIds, List<Guid> visibleOffersIds, int version
                            )
        {
            Id = id;
            Name = name;
            Email = email;
            IsActivated = isActivated;
            IsVisible = isVisible;
            VisibleOffers = visibleOffers;
            Version = version;
            TotalOffers = totalOffers;
            OffersIds = offersIds;
            VisibleOffersIds = visibleOffersIds;
        }
    }
}