using CQRSCode.ReadModel;
using CQRSCode.WriteModel.Commands;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Commands;
using CQRSCode.ReadModel.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CQRSWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IRepository<ProductDto> _productsRepository;
        private readonly IRepository<ProductSummaryDto> _productsSummaryRepository;

        public ProductsController(ICommandSender commandSender,
                                    IRepository<ProductDto> productsRepository,
                                    IRepository<ProductSummaryDto> productsSummaryRepository)
        {
            _productsRepository = productsRepository;
            _productsSummaryRepository = productsSummaryRepository;
            _commandSender = commandSender;
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

            return View();
        }
    }
}
