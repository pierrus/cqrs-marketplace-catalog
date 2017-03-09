using System;
using CQRSCode.ReadModel.Dtos;
using CQRSCode.ReadModel.Events;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Handlers
{
	public class CategoryDetailView : IEventHandler<CategoryCreated>,
                                        IEventHandler<CategoryActivated>,
                                        IEventHandler<CategoryDeactivated>,
                                        IEventHandler<ProductPublishedToCategory>,
                                        IEventHandler<ProductUnpublishedFromCategory>
    {
        IRepository<CategoryDto> _categoryRepository;

        public CategoryDetailView (IRepository<CategoryDto> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Handle(CategoryCreated message)
        {
            var cat = new CategoryDto(message.Id, message.Name,
                                        message.Activated, false,
                                        0, 0, message.Version, message.ParentId);

            _categoryRepository.Insert(cat);
        }

        // Bubble up visible products to parent categories
        public void Handle(CategoryDeactivated message)
        {
            var cat = _categoryRepository.GetById(message.Id);

            cat.IsActivated = false;

            _categoryRepository.Update(cat);

            Guid? parentId = cat.ParentId;
            while (parentId != null)
            {
                var parentCat = _categoryRepository.GetById(parentId.Value);
                parentCat.TotalVisibleProducts -= cat.TotalVisibleProducts;
                _categoryRepository.Update(parentCat);
                parentId = parentCat.ParentId;
            }
        }

        // Bubble up visible products to parent categories
        public void Handle(CategoryActivated message)
        {
            var cat = _categoryRepository.GetById(message.Id);

            cat.IsActivated = true;

            _categoryRepository.Update(cat);

            Guid? parentId = cat.ParentId;
            while (parentId != null)
            {
                var parentCat = _categoryRepository.GetById(parentId.Value);
                parentCat.TotalVisibleProducts += cat.TotalVisibleProducts;
                _categoryRepository.Update(parentCat);
                parentId = parentCat.ParentId;
            }
        }

        // Garder le compte des produits publiés
        // Mettre à jour le nombre de produits dans la hiérarchie de catégories parentes
        public void Handle(ProductPublishedToCategory message)
        {
            var cat = _categoryRepository.GetById(message.Id);

            cat.VisibleProducts += 1;
            cat.TotalVisibleProducts += 1;            

            _categoryRepository.Update(cat);

            Guid? parentId = cat.ParentId;
            while (parentId != null)
            {
                var parentCat = _categoryRepository.GetById(parentId.Value);
                parentCat.TotalVisibleProducts += 1;
                _categoryRepository.Update(parentCat);
                parentId = parentCat.ParentId;
            }
        }

        // Garder le compte des produits publiés
        // Mettre à jour le nombre de produits dans la hiérarchie de catégories parentes
        public void Handle(ProductUnpublishedFromCategory message)
        {
            var cat = _categoryRepository.GetById(message.Id);

            cat.VisibleProducts -= 1;
            cat.TotalVisibleProducts -= 1;            

            _categoryRepository.Update(cat);

            Guid? parentId = cat.ParentId;
            while (parentId != null)
            {
                var parentCat = _categoryRepository.GetById(parentId.Value);
                parentCat.TotalVisibleProducts -= 1;
                _categoryRepository.Update(parentCat);
                parentId = parentCat.ParentId;
            }
        }
    }
}