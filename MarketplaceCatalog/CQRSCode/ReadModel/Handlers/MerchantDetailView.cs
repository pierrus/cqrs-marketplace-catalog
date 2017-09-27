using System.Threading.Tasks;
using CQRSCode.ReadModel.Dtos;
using CQRSCode.ReadModel.Events;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Events;
using System.Linq;

namespace CQRSCode.ReadModel.Handlers
{
	public class MerchantDetailView : IEventHandler<MerchantCreated>,
                                        IEventHandler<OfferPublishedToMerchant>,
                                        IEventHandler<OfferUnpublishedFromMerchant>,
                                        IEventHandler<MerchantActivated>,
                                        IEventHandler<MerchantDeactivated>
    {
        IRepository<MerchantDto> _merchantRepository;

        public MerchantDetailView (IRepository<MerchantDto> merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task Handle(MerchantCreated message)
        {
            _merchantRepository.Insert(new MerchantDto(message.Id, message.Name, message.Email,
                                            true, false, 0,
                                            0, message.Version));
        }

        public async Task Handle(OfferPublishedToMerchant message)
        {
        }

        public async Task Handle(OfferUnpublishedFromMerchant message)
        {
        }

        public async Task Handle(MerchantActivated message)
        {
            MerchantDto mercDto = _merchantRepository.GetById(message.Id);
            mercDto.IsActivated = true;            
            _merchantRepository.Update(mercDto);
        }

        public async Task Handle(MerchantDeactivated message)
        {
            MerchantDto mercDto = _merchantRepository.GetById(message.Id);
            mercDto.IsActivated = false;
            _merchantRepository.Update(mercDto);
        }
        
    }
}