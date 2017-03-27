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
    public class WhenProductCategoryChanged : Specification<Product, ProductCommandHandlers, SetProductCategory>
    {
        private Guid _id;
        private Guid _merchantId;
        private Guid _productId;
        private Guid _catId;
        private Guid _catId2;
        
        protected override ProductCommandHandlers BuildHandler()
        {
            return new ProductCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _id = Guid.NewGuid();
            _merchantId = Guid.NewGuid();
            _productId = Guid.NewGuid();
            _catId = Guid.NewGuid();
            _catId2 = Guid.NewGuid();

            return new List<IEvent>()
            {
                new MerchantCreated(_merchantId, "myMerchant", "mymerchant@gmail.com") { Version = 1, TimeStamp = DateTimeOffset.Now },
                new ProductCreated(_productId, "myProduct", "myDescription", true, true) { Version = 1, TimeStamp = DateTimeOffset.Now },
                new OfferCreated(_productId, _id, _merchantId, 1, 10, true, null, "myMerchant", true, true) { Version = 2, TimeStamp = DateTimeOffset.Now },
                new ProductDisplayed(_productId) { Version = 3, TimeStamp = DateTimeOffset.Now },
                new OfferDisplayed(_productId, _id) { Version = 4, TimeStamp = DateTimeOffset.Now },
                new OfferPublishedToMerchant(_productId, _id, _merchantId) { Version = 5, TimeStamp = DateTimeOffset.Now },
                new ProductCategoryDefined(_productId, _catId) { Version = 6, TimeStamp = DateTimeOffset.Now },
                new ProductPublishedToCategory(_productId, _catId, true, "myProduct", "myDescription", 1, new List<decimal>() { 10 }) { Version = 7, TimeStamp = DateTimeOffset.Now }
            };
        }

        protected override SetProductCategory When()
        {
            return new SetProductCategory(_productId, _catId2, 7);
        }

        [Then]
        public void Should_create_two_events()
        {
            //ProductUnpublishedFromCategory + ProductCategoryDefined + ProductPublishedToCategory
            Assert.Equal(3, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_events()
        {
            Assert.IsType<ProductUnpublishedFromCategory>(PublishedEvents.First());
            Assert.IsType<ProductCategoryDefined>(PublishedEvents[1]);
            Assert.IsType<ProductPublishedToCategory>(PublishedEvents[2]);
        }

        [Then]
        public void Should_Should_publish_full_product()
        {
            Assert.Equal(_productId, ((ProductUnpublishedFromCategory)PublishedEvents.First()).Id);
            Assert.Equal(_catId, ((ProductUnpublishedFromCategory)PublishedEvents.First()).CategoryId);
            Assert.Equal(_catId2, ((ProductPublishedToCategory)PublishedEvents[2]).CategoryId);
        }
    }
}