using System.Threading.Tasks;
using CQRSCode.ReadModel.Dtos;
using CQRSCode.ReadModel.Events;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Events;
using System.Linq;

namespace CQRSCode.ReadModel.Handlers
{
	public class ProductDetailView : IEventHandler<ProductCreated>,
										IEventHandler<OfferCreated>,
										IEventHandler<OfferStockSet>,
										IEventHandler<ProductDisplayed>,
										IEventHandler<ProductHidden>
    {
        IRepository<ProductDto> _productRepository;

        public ProductDetailView (IRepository<ProductDto> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(ProductCreated message)
        {
            _productRepository.Insert(new ProductDto(message.Id, null, message.Name, message.Description,
                                            message.IsActivated, message.IsVisible, message.Version,
                                            message.EAN, message.UPC));
        }

        public async Task Handle(OfferCreated message)
        {
            ProductDto prod = _productRepository.GetById(message.Id);

            prod.Offers.Add(new OfferDto(message.OfferId, message.MerchantId, message.MerchantName, 
                                            message.IsActivated, message.IsVisible, message.Stock,
                                            message.Price, message.Version, message.SKU));

            _productRepository.Update(prod);
        }

        public async Task Handle(OfferStockSet message)
        {
            ProductDto prod = _productRepository.GetById(message.Id);

            OfferDto offerDto = prod.Offers.Where(o => o.Id == message.OfferId).FirstOrDefault();

            offerDto.Stock = message.Stock;

            _productRepository.Update(prod);
        }

        public async Task Handle(ProductDisplayed message)
        {
            ProductDto prod = _productRepository.GetById(message.Id);

            prod.IsVisible = true;

            _productRepository.Update(prod);
        }

        public async Task Handle(ProductHidden message)
        {
            ProductDto prod = _productRepository.GetById(message.Id);

            prod.IsVisible = false;

            _productRepository.Update(prod);
        }

        public async Task Handle(OfferDisplayed message)
        {
            ProductDto prod = _productRepository.GetById(message.Id);

            OfferDto offer = prod.Offers.Where(o => o.Id == message.OfferId).FirstOrDefault();
            offer.IsVisible = true;

            _productRepository.Update(prod);
        }

        public async Task Handle(OfferHidden message)
        {
            ProductDto prod = _productRepository.GetById(message.Id);

            OfferDto offer = prod.Offers.Where(o => o.Id == message.OfferId).FirstOrDefault();
            offer.IsVisible = false;

            _productRepository.Update(prod);
        }
    }
}