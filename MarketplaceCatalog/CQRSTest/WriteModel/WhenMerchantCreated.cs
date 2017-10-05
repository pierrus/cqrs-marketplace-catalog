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
    public class WhenMerchantCreated : Specification<Merchant, MerchantCommandHandlers, CreateMerchant>
    {
        private Guid _id;
        
        protected override MerchantCommandHandlers BuildHandler()
        {
            return new MerchantCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _id = Guid.NewGuid();

            return new List<IEvent>();
        }

        protected override CreateMerchant When()
        {
            return new CreateMerchant(_id, "My merchant", "merchant@marketplace.com", 0) { ExpectedVersion = 1 };
        }

        [Then]
        public void Should_create_four_events()
        {
            //OfferCreated + ProductDisplayed
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_events()
        {
            Assert.IsType<MerchantCreated>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_name()
        {
            Assert.Equal(_id, ((MerchantCreated)PublishedEvents.First()).Id);
        }
    }
}