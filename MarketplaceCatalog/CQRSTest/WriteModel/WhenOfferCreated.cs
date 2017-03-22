using System;
using System.Collections.Generic;
using System.Linq;
using CQRSCode.ReadModel.Events;
using CQRSCode.WriteModel.Commands;
using CQRSCode.WriteModel.Domain;
using CQRSCode.WriteModel.Handlers;
using CQRSlite.Events;
using CQRSlite.Tests.Extensions.TestHelpers;
using Xunit;

namespace CQRSTests.WriteModel
{
    public class WhenOfferCreated : Specification<Product, ProductCommandHandlers, CreateOffer>
    {
        private Guid _id;
        private Guid _merchantId;
        private Guid _productId;
        protected override ProductCommandHandlers BuildHandler()
        {
            return new ProductCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _id = Guid.NewGuid();
            _merchantId = Guid.NewGuid();
            _productId = Guid.NewGuid();

            return new List<IEvent>()
            {
                new MerchantCreated(_merchantId, "myMerchant", "mymerchant@gmail.com") { Version = 1, TimeStamp = DateTimeOffset.Now },
                new ProductCreated(_productId, "myProduct", "myDescription", true, true) { Version = 1, TimeStamp = DateTimeOffset.Now }
            };
        }

        protected override CreateOffer When()
        {
            return new CreateOffer(_id, _productId, _merchantId, 35m, 10) { ExpectedVersion = 1 };
        }

        [Then]
        public void Should_create_two_events()
        {
            //OfferCreated + ProductDisplayed
            Assert.Equal(4, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<OfferCreated>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_name()
        {
            Assert.Equal(_productId, ((OfferCreated)PublishedEvents.First()).Id);
        }
    }
}