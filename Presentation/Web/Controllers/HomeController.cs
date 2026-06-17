using Microsoft.AspNetCore.Mvc;
using Services;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserServices _userServices;
        public HomeController(ILogger<HomeController> logger, IUserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        public IActionResult Index()
        {
            var data = _userServices.Count();
            if(data.Status == "00")
            {
                return View(data.Data);
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