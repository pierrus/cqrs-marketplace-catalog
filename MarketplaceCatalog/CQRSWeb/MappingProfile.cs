using AutoMapper;
using CQRSCode.ReadModel.Dtos;
using CQRSWeb.Models;

namespace CQRSWeb
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            // Add as many of these lines as you need to map your objects
            CreateMap<MerchantDto, Merchant>();
        }
    }
}