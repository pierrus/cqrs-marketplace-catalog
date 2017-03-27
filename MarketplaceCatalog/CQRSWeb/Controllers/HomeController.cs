using CQRSCode.ReadModel;
using CQRSCode.WriteModel.Commands;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Commands;
using CQRSCode.ReadModel.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CQRSWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IRepository<ProductDto> _productsRepository;

        public HomeController(ICommandSender commandSender, IRepository<ProductDto> productsRepository)
        {
            _productsRepository = productsRepository;
            _commandSender = commandSender;
        }

        public ActionResult Index()
        {
            //ViewData.Model = _readmodel.GetInventoryItems();
            return View();
        }

        // public ActionResult Details(Guid id)
        // {
        //     ViewData.Model = _readmodel.GetInventoryItemDetails(id);
        //     return View();
        // }

        // public ActionResult Add()
        // {
        //     return View();
        // }

        // [HttpPost]
        // public ActionResult Add(string name)
        // {
        //     _commandSender.Send(new CreateInventoryItem(Guid.NewGuid(), name));
        //     return RedirectToAction("Index");
        // }

        // public ActionResult ChangeName(Guid id)
        // {
        //     ViewData.Model = _readmodel.GetInventoryItemDetails(id);
        //     return View();
        // }

        // [HttpPost]
        // public ActionResult ChangeName(Guid id, string name, int version)
        // {
        //     _commandSender.Send(new RenameInventoryItem(id, name, version));
        //     return RedirectToAction("Index");
        // }

        // public ActionResult Deactivate(Guid id, int version)
        // {
        //     _commandSender.Send(new DeactivateInventoryItem(id, version));
        //     return RedirectToAction("Index");
        // }

        // public ActionResult CheckIn(Guid id)
        // {
        //     ViewData.Model = _readmodel.GetInventoryItemDetails(id);
        //     return View();
        // }

        // [HttpPost]
        // public ActionResult CheckIn(Guid id, int number, int version)
        // {
        //     _commandSender.Send(new CheckInItemsToInventory(id, number, version));
        //     return RedirectToAction("Index");
        // }

        // public ActionResult Remove(Guid id)
        // {
        //     ViewData.Model = _readmodel.GetInventoryItemDetails(id);
        //     return View();
        // }

        // [HttpPost]
        // public ActionResult Remove(Guid id, int number, int version)
        // {
        //     _commandSender.Send(new RemoveItemsFromInventory(id, number, version));
        //     return RedirectToAction("Index");
        // }
    }
}
