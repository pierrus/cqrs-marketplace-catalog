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
    public class WhenOfferOutOfStock : Specification<Product, ProductCommandHandlers, SetStock>
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
                new ProductCreated(_productId, "myProduct", "myDescription", true, true) { Version = 1, TimeStamp = DateTimeOffset.Now },
                new OfferCreated(_productId, _id, _merchantId, 1, 10, true, null, "myMerchant", true, true) { Version = 2, TimeStamp = DateTimeOffset.Now },
                new ProductDisplayed(_productId) { Version = 3, TimeStamp = DateTimeOffset.Now },
                new OfferDisplayed(_productId, _id) { Version = 4, TimeStamp = DateTimeOffset.Now },
                new OfferPublishedToMerchant(_productId, _id, _merchantId) { Version = 5, TimeStamp = DateTimeOffset.Now }
            };
        }

        protected override SetStock When()
        {
            return new SetStock(_id, _productId, 0, 5);
        }

        [Then]
        public void Should_create_four_events()
        {
            //OfferStockSet + ProductHidden + OfferHidden + OfferUnpublishedFromMerchant
            Assert.Equal(4, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_events()
        {
            Assert.IsType<OfferStockSet>(PublishedEvents.First());
            Assert.IsType<ProductHidden>(PublishedEvents[1]);
            Assert.IsType<OfferHidden>(PublishedEvents[2]);
            Assert.IsType<OfferUnpublishedFromMerchant>(PublishedEvents.Last());
        }

        [Then]
        public void Should_set_stock_to_zero()
        {
            Assert.Equal(_productId, ((OfferStockSet)PublishedEvents.First()).Id);
            Assert.Equal(0, ((OfferStockSet)PublishedEvents.First()).Stock);
        }
    }
}