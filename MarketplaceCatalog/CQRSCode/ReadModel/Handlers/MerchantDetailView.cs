using CQRSCode.ReadModel.Dtos;
using CQRSCode.ReadModel.Events;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Events;
using System.Linq;

namespace CQRSCode.ReadModel.Handlers
{
	public class MerchantDetailView : IEventHandler<MerchantCreated>,
                                        IEventHandler<OfferPublishedToMerchant>,
                                        IEventHandler<OfferUnpublishedFromMerchant>
    {
        IRepository<MerchantDto> _merchantRepository;

        public MerchantDetailView (IRepository<MerchantDto> merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public void Handle(MerchantCreated message)
        {
        }

        public void Handle(OfferPublishedToMerchant message)
        {
        }

        public void Handle(OfferUnpublishedFromMerchant message)
        {
        }
    }
}