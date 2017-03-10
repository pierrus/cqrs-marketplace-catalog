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
	public class CategorySaga : IEventHandler<CategoryActivated>
    {
        private readonly IRepository<ProductDto> _productRepository;

        private readonly IRepository<ProductSummaryDto> _productSummaryRepository;

        private readonly ICommandSender _commandSender;

        public CategorySaga (IRepository<ProductSummaryDto> productSummaryRepository,
                                IRepository<ProductDto> productRepository,
                                ICommandSender commandSender)
        {
            _productRepository = productRepository;
            _productSummaryRepository = productSummaryRepository;
            _commandSender = commandSender;
        }

        public void Handle(CategoryActivated message)
        {
            IList<ProductDto> products = _productRepository
                                            .SearchFor(p => p.CategoryId == message.Id)
                                            .ToList();

            foreach (var prod in products)
            {
                _commandSender.Send<ActivateCategoryOnProduct>(new ActivateCategoryOnProduct(prod.Id, message.Id));
            }
        }
    }
}