using CQRSCode.ReadModel;
using CQRSCode.WriteModel.Commands;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Commands;
using CQRSCode.ReadModel.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using System;

namespace CQRSWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IRepository<ProductDto> _productsRepository;
        private readonly IRepository<ProductSummaryDto> _productsSummaryRepository;
        private readonly IRepository<MerchantDto> _merchantsRepository;
        private readonly IMapper _mapper;

        public ProductsController(ICommandSender commandSender,
                                    IRepository<ProductDto> productsRepository,
                                    IRepository<ProductSummaryDto> productsSummaryRepository,
                                    IRepository<MerchantDto> merchantsRepository,
                                    IMapper mapper)
        {
            _productsRepository = productsRepository;
            _productsSummaryRepository = productsSummaryRepository;
            _commandSender = commandSender;
            _merchantsRepository = merchantsRepository;
            _mapper = mapper;
        }

        public ActionResult Index(Int32? startIndex, Int32? limit)
        {
            if (startIndex == null)
                startIndex = 0;

            if (limit == null)
                limit = 10;

            ViewData.Model = _productsRepository.SearchFor(p => true, startIndex, limit);

            return View();
        }

        public ActionResult Details(Guid id)
        {
            ViewData.Model = _productsRepository.GetById(id);

            if (ViewData.Model == null)
                return RedirectToAction("Index");

            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Models.Product newProduct)
        {
            if (!ModelState.IsValid)
                return View();

            Guid newId = Guid.NewGuid();

            _commandSender.Send<CreateProduct>(new CreateProduct(newId, newProduct.Name, newProduct.Description, newProduct.EAN, newProduct.UPC));

            return RedirectToAction("Details", new { id = newId });
        }

        [HttpGet]
        public ActionResult AddOffer(Guid productId)
        {
            var prod = _productsRepository.GetById(productId);
            var merchants = _merchantsRepository.SearchFor(mDto => true);

            var offer = new Models.Offer { ProductId = prod.Id, ProductName = prod.Name };
            offer.Merchants = _mapper.Map<IList<MerchantDto>, IList<Models.Merchant>>(merchants);
            ViewData.Model = offer;

            return View();
        }

        [HttpPost]
        public ActionResult AddOffer(Models.Offer newOffer)
        {
            if (!ModelState.IsValid)
                return View(newOffer);

            Guid newId = Guid.NewGuid();

            _commandSender.Send<CreateOffer>(
                    new CreateOffer(
                        newId, newOffer.ProductId, newOffer.MerchantId,
                        newOffer.Price, newOffer.Stock, newOffer.SKU
                      ));

            return RedirectToAction("Details", new { id = newOffer.ProductId });
        }
    }
}
