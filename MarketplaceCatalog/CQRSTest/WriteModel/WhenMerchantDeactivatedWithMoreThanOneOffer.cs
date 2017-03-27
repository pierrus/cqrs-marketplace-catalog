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
    public class WhenMerchantDeactivatedWithMoreThanOneOffer : Specification<Product, ProductCommandHandlers, DeactivateMerchantOnOffer>
    {
        private Guid _offerId;
        private Guid _offerId2;
        private Guid _merchantId;
        private Guid _merchantId2;
        private Guid _productId;
        
        protected override ProductCommandHandlers BuildHandler()
        {
            return new ProductCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _offerId = Guid.NewGuid();
            _offerId2 = Guid.NewGuid();
            _merchantId = Guid.NewGuid();
            _merchantId2 = Guid.NewGuid();
            _productId = Guid.NewGuid();

            return new List<IEvent>()
            {
                new MerchantCreated(_merchantId, "myMerchant", "mymerchant@gmail.com") { Version = 1, TimeStamp = DateTimeOffset.Now },
                new MerchantCreated(_merchantId2, "myMerchant2", "mymerchant2@gmail.com") { Version = 1, TimeStamp = DateTimeOffset.Now },
                
                new ProductCreated(_productId, "myProduct", "myDescription", true, true) { Version = 1, TimeStamp = DateTimeOffset.Now },
                
                new OfferCreated(_productId, _offerId, _merchantId, 1, 10, true, null, "myMerchant", true, true) { Version = 2, TimeStamp = DateTimeOffset.Now },
                new ProductDisplayed(_productId) { Version = 3, TimeStamp = DateTimeOffset.Now },
                new OfferDisplayed(_productId, _offerId) { Version = 4, TimeStamp = DateTimeOffset.Now },
                new OfferPublishedToMerchant(_productId, _offerId, _merchantId) { Version = 5, TimeStamp = DateTimeOffset.Now },
                
                new OfferCreated(_productId, _offerId2, _merchantId2, 1, 10, true, null, "myMerchant", true, true) { Version = 6, TimeStamp = DateTimeOffset.Now },
                new OfferPublishedToMerchant(_productId, _offerId2, _merchantId2) { Version = 7, TimeStamp = DateTimeOffset.Now },
                new OfferDisplayed(_productId, _offerId2) { Version = 8, TimeStamp = DateTimeOffset.Now }
            };
        }

        protected override DeactivateMerchantOnOffer When()
        {
            return new DeactivateMerchantOnOffer(_offerId, _productId, _merchantId, 8);
        }

        [Then]
        public void Should_create_three_events()
        {
            //OfferMerchantDeactivated + OfferMerchantDeactivated + OfferMerchantDeactivated
            Assert.Equal(3, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_events()
        {
            Assert.IsType<OfferMerchantDeactivated>(PublishedEvents.First());
            Assert.IsType<OfferHidden>(PublishedEvents[1]);
            Assert.IsType<OfferUnpublishedFromMerchant>(PublishedEvents.Last());
        }

        [Then]
        public void Should_deactivate_offer()
        {
            Assert.Equal(_productId, ((OfferMerchantDeactivated)PublishedEvents.First()).Id);
            Assert.Equal(_merchantId, ((OfferMerchantDeactivated)PublishedEvents.First()).MerchantId);
            Assert.Equal(_offerId, ((OfferMerchantDeactivated)PublishedEvents.First()).OfferId);
        }
    }
}