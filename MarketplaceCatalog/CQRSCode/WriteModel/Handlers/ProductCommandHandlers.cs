using System;
using System.Linq;
using System.Collections.Generic;
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

        public void Handle(CreateProduct message)
        {
            var product = new Product(message.Id, message.Name, message.Description, true, false, message.EAN, message.UPC);
            _session.Add(product);
            _session.Commit();
        }

        //Loads 2 aggregate roots, but commit only one
        public void Handle(CreateOffer message)
        {
            var product = _session.Get<Product>(message.ProductId);
            var merchant = _session.Get<Merchant>(message.MerchantId);

            product.CreateOffer(message.Id, message.ProductId, message.Stock, message.Price, merchant.Activated, merchant.Name, message.SKU);

            _session.Commit();
        }

        public void Handle(SetProductCategory message)
        {
            var product = _session.Get<Product>(message.Id, message.ExpectedVersion);
            product.SetCategory(message.CategoryId);

            _session.Commit();
        }

        public void Handle(SetStock message)
        {
            var product = _session.Get<Product>(message.ProductId, message.ExpectedVersion);
            product.SetStock(message.Id, message.Stock);

            _session.Commit();
        }

        public void Handle(DeactivateMerchantOnOffer message)
        {
            var product = _session.Get<Product>(message.ProductId, message.ExpectedVersion);
            product.DeactivateMerchant(message.Id, message.MerchantId);

            _session.Commit();
        }

        public void Handle(ActivateMerchantOnOffer message)
        {
            var product = _session.Get<Product>(message.ProductId, message.ExpectedVersion);
            product.ActivateMerchant(message.Id, message.MerchantId);

            _session.Commit();
        }

        public void Handle(ActivateCategoryOnProduct message)
        {
            var product = _session.Get<Product>(message.Id);

            if (product.CategoryId == null)
                return;

            product.ActivateCategory();

            _session.Commit();
        }
    }
}
