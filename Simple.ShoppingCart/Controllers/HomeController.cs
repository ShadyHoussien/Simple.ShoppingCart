using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simple.ShoppingCart.Models;

namespace Simple.ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static Models.ShoppingCartDto ShoppingCart { get; set; }
        public static int WashingMachineQuantity { get; set; } = 10;
        public static bool ItemAdded { get; set; } = false;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Cart = ShoppingCart ?? new Models.ShoppingCartDto();
            ViewBag.ItemAdded = ItemAdded;
            return View(WashingMachineQuantity);

        }

        //public IActionResult Products()
        //{
        //    return View(WashingMachineQuantity);
        //}

        public ActionResult AddToCart()
        {
            ShoppingCart = new Models.ShoppingCartDto
            {
                Price = 8000,
                ProductId = 1,
                ProductName = "LG Washing Machine",
                Quantity = 1
            };
            WashingMachineQuantity--;
            ItemAdded = true;
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            return RedirectToAction("index","Checkout");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
