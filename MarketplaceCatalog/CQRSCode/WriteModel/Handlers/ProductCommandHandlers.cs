using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSCode.WriteModel.Commands;
using CQRSCode.WriteModel.Domain;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace CQRSCode.WriteModel.Handlers
{
    public class ProductCommandHandlers : ICommandHandler<CreateProduct>,
											ICommandHandler<CreateOffer>,
											ICommandHandler<SetProductCategory>,
											ICommandHandler<ActivateMerchantOnOffer>,
											ICommandHandler<DeactivateMerchantOnOffer>,
											ICommandHandler<SetStock>
    {
        private readonly ISession _session;

        public ProductCommandHandlers(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateProduct message)
        {
            var product = new Product(message.Id, message.Name, message.Description, true, false, message.EAN, message.UPC);
            await _session.Add(product);
            await _session.Commit();
        }

        //Loads 2 aggregate roots, but commit only one
        public async Task Handle(CreateOffer message)
        {
            var product = await _session.Get<Product>(message.ProductId);
            var merchant = await _session.Get<Merchant>(message.MerchantId);

            product.CreateOffer(message.Id, message.MerchantId, message.Stock, message.Price, merchant.Activated, merchant.Name, message.SKU);

            await _session.Commit();
        }

        public async Task Handle(SetProductCategory message)
        {
            var product = await _session.Get<Product>(message.Id, message.ExpectedVersion);
            product.SetCategory(message.CategoryId);

            await _session.Commit();
        }

        public async Task Handle(SetStock message)
        {
            var product = await _session.Get<Product>(message.ProductId, message.ExpectedVersion);
            product.SetStock(message.Id, message.Stock);

            await _session.Commit();
        }

        public async Task Handle(DeactivateMerchantOnOffer message)
        {
            var product = await _session.Get<Product>(message.ProductId);
            product.DeactivateMerchant(message.Id, message.MerchantId);

            await _session.Commit();
        }

        public async Task Handle(ActivateMerchantOnOffer message)
        {
            var product = await _session.Get<Product>(message.ProductId);
            product.ActivateMerchant(message.Id, message.MerchantId);

            await _session.Commit();
        }

        public async Task Handle(ActivateCategoryOnProduct message)
        {
            var product = await _session.Get<Product>(message.Id);

            if (product.CategoryId == null)
                return;

            product.ActivateCategory();

            await _session.Commit();
        }
    }
}
