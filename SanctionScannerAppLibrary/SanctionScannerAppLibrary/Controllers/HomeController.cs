using Microsoft.AspNetCore.Mvc;
using SanctionScannerAppLibrary.Models;
using SanctionScannerAppLibrary.Models.Entities;
using System.Diagnostics;

namespace SanctionScannerAppLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (var db=new DBLIBRARYContext())
            {
                var data = db.Categories.ToList();
            }
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}