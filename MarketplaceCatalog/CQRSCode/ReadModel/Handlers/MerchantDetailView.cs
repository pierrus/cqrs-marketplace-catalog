using System.Threading.Tasks;
using CQRSCode.ReadModel.Dtos;
using CQRSCode.ReadModel.Events;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Events;
using System.Linq;
using System.Collections.Generic;
using System;

namespace CQRSCode.ReadModel.Handlers
{
	public class MerchantDetailView : IEventHandler<MerchantCreated>,
                                        IEventHandler<OfferPublishedToMerchant>,
                                        IEventHandler<OfferUnpublishedFromMerchant>,
                                        IEventHandler<MerchantActivated>,
                                        IEventHandler<MerchantDeactivated>,
                                        IEventHandler<OfferCreated>
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
                                            0, new List<Guid>(), new List<Guid>(),
                                            message.Version));
        }

        public async Task Handle(OfferPublishedToMerchant message)
        {
            MerchantDto mercDto = _merchantRepository.GetById(message.MerchantId);

            if (!mercDto.VisibleOffersIds.Contains(message.OfferId))
                mercDto.VisibleOffersIds.Add(message.OfferId);

            mercDto.VisibleOffers = mercDto.VisibleOffersIds.Count();
            mercDto.IsVisible = mercDto.VisibleOffers > 0 ? true : false;

            _merchantRepository.Update(mercDto);
        }

        public async Task Handle(OfferUnpublishedFromMerchant message)
        {
            MerchantDto mercDto = _merchantRepository.GetById(message.MerchantId);

            if (mercDto.VisibleOffersIds.Contains(message.OfferId))
                mercDto.VisibleOffersIds.Remove(message.OfferId);

            mercDto.VisibleOffers = mercDto.VisibleOffersIds.Count();
            mercDto.IsVisible = mercDto.VisibleOffers > 0 ? true : false;
            mercDto.Version = message.Version;

            _merchantRepository.Update(mercDto);
        }

        public async Task Handle(MerchantActivated message)
        {
            MerchantDto mercDto = _merchantRepository.GetById(message.Id);
            mercDto.IsActivated = true;
            mercDto.Version = message.Version;
            _merchantRepository.Update(mercDto);
        }

        public async Task Handle(MerchantDeactivated message)
        {
            MerchantDto mercDto = _merchantRepository.GetById(message.Id);
            mercDto.IsActivated = false;
            mercDto.Version = message.Version;
            _merchantRepository.Update(mercDto);
        }

        public async Task Handle(OfferCreated message)
        {
            MerchantDto mercDto = _merchantRepository.GetById(message.MerchantId);

            if (!mercDto.OffersIds.Contains(message.OfferId))
                mercDto.OffersIds.Add(message.OfferId);

            mercDto.TotalOffers = mercDto.OffersIds.Count();
            mercDto.Version = message.Version;

            _merchantRepository.Update(mercDto);
        }
    }
}