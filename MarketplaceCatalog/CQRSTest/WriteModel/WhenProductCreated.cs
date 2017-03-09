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
    public class WhenProductCreated : Specification<Product, ProductCommandHandlers, CreateProduct>
    {
        private Guid _id;
        protected override ProductCommandHandlers BuildHandler()
        {
            return new ProductCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _id = Guid.NewGuid();
            return new List<IEvent>();
        }

        protected override CreateProduct When()
        {
            return new CreateProduct(_id, "myproduct", "a nice myproduct");
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<ProductCreated>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_name()
        {
            Assert.Equal("myproduct", ((ProductCreated)PublishedEvents.First()).Name);
        }
    }
}