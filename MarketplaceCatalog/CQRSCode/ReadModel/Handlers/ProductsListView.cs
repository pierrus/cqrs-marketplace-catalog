using CQRSCode.ReadModel.Dtos;
using CQRSCode.ReadModel.Events;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Events;
using System.Linq;
using System.Collections.Generic;

namespace CQRSCode.ReadModel.Handlers
{
	public class ProductsListView : IEventHandler<ProductCategoryDefined>,
                                    IEventHandler<ProductPublishedToCategory>,
                                    IEventHandler<ProductUnpublishedFromCategory>,
                                    IEventHandler<CategoryDeactivated>
    {
        IRepository<ProductSummaryDto> _productsRepository;

        public ProductsListView (IRepository<ProductSummaryDto> productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public void Handle(ProductCategoryDefined message)
        {
            var prod = _productsRepository
                        .SearchFor(p => p.Id == message.Id)
                        .FirstOrDefault();

            prod.CategoryId = message.CategoryId;

            _productsRepository.Update(prod);
        }

        public void Handle(ProductPublishedToCategory message)
        {
            var prod = new ProductSummaryDto();

            //Construire la liste des catégories

            _productsRepository.Insert(prod);
        }

        public void Handle(ProductUnpublishedFromCategory message)
        {
            var prod = _productsRepository
                        .SearchFor(p => p.Id == message.Id)
                        .FirstOrDefault();

            _productsRepository.Delete(prod);
        }

        // This operation might take a long time
        // Better to just deactivate the products ? Think about it as a cache
        public void Handle(CategoryDeactivated message)
        {
            var prods = _productsRepository
                        .SearchFor(p => p.Categories.Any(cId => cId == message.Id));

            foreach (var prod in prods)
                _productsRepository.Delete(prod);
        }
    }
}