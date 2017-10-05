using CQRSCode.ReadModel;
using CQRSCode.WriteModel.Commands;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Commands;
using CQRSCode.ReadModel.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;

namespace CQRSWeb.Controllers
{
    public class MerchantsController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IRepository<MerchantDto> _merchantsRepository;
        private readonly IMapper _mapper;

        public MerchantsController(ICommandSender commandSender,
                                    IRepository<MerchantDto> merchantsRepository,
                                    IMapper mapper)
        {
            _merchantsRepository = merchantsRepository;
            _commandSender = commandSender;
            _mapper = mapper;
        }

        public ActionResult Index(Int32? startIndex, Int32? limit)
        {
            if (startIndex == null)
                startIndex = 0;

            if (limit == null)
                limit = 10;

            ViewData.Model = _merchantsRepository.SearchFor(p => true, startIndex, limit);

            return View();
        }

        public ActionResult Details(Guid id)
        {
            var mercDto = _merchantsRepository.GetById(id);
            ViewData.Model = _mapper.Map<MerchantDto, Models.Merchant>(mercDto);
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Models.Merchant newMerchant)
        {
            if (!ModelState.IsValid)
                return View();

            Guid newId = Guid.NewGuid();

            _commandSender.Send<CreateMerchant>(new CreateMerchant(newId, newMerchant.Name, newMerchant.Email, newMerchant.Commission));

            return RedirectToAction("Details", new { id = newId });
        }

        [HttpGet]
        public ActionResult Activate(Guid id, Int32 version)
        {
            _commandSender.Send<ActivateMerchant>(new ActivateMerchant(id, version));

            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Deactivate(Guid id, Int32 version)
        {
            _commandSender.Send<DeactivateMerchant>(new DeactivateMerchant(id, version));            

            return RedirectToAction("Details", new { id = id });
        }
    }
}
