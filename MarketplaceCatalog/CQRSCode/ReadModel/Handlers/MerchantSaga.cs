using CQRSCode.ReadModel.Dtos;
using CQRSCode.ReadModel.Events;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Events;
using CQRSCode.WriteModel.Commands;
using CQRSlite.Commands;
using System.Linq;
using System.Collections.Generic;

namespace CQRSCode.ReadModel.Handlers
{
	public class MerchantSaga : IEventHandler<MerchantActivated>,
                                IEventHandler<MerchantDeactivated>
    {
        private readonly IRepository<ProductDto> _productRepository;

        private readonly ICommandSender _commandSender;

        public MerchantSaga (IRepository<ProductDto> productRepository, ICommandSender commandSender)
        {
            _productRepository = productRepository;
            _commandSender = commandSender;
        }

        public void Handle(MerchantActivated message)
        {
            List<ProductDto> products = _productRepository.SearchFor(p => p.Offers.Any(o => o.MerchantId == message.Id)).ToList();

            foreach (var prod in products)
            {
                var offer = prod.Offers.Where(o => o.MerchantId == message.Id).FirstOrDefault();
                _commandSender.Send(new ActivateMerchantOnOffer(offer.Id, prod.Id, message.Id));
            }
        }

        public void Handle(MerchantDeactivated message)
        {
            List<ProductDto> products = _productRepository.SearchFor(p => p.Offers.Any(o => o.MerchantId == message.Id)).ToList();

            foreach (var prod in products)
            {
                var offer = prod.Offers.Where(o => o.MerchantId == message.Id).FirstOrDefault();
                _commandSender.Send(new DeactivateMerchantOnOffer(offer.Id, prod.Id, message.Id));
            }
        }
    }
}