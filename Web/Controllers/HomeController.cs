using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Web.Models;
using Web.Repository;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ItemRepository _itemRepository;
        public HomeController()
        {
            _itemRepository = new ItemRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Items()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var items = _itemRepository.GetAllByUser(userId);

            return PartialView(items);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Create(Item item)
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            _itemRepository.Save(item, userId);

            return Json(true);
        }

        [HttpGet]
        public JsonResult Delete(int itemId)
        {
            _itemRepository.Detele(itemId);
            return Json(true);
        }

        [HttpGet]
        public ActionResult ItemDetail(int itemId)
        {
            var itemDetails = _itemRepository.GetById(itemId);

            return PartialView(itemDetails);
        }
    }
}