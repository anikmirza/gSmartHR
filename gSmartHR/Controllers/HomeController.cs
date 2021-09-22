using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gSmartHR.Models;
using gSmartHR.Service;

namespace gSmartHR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserRepository _IUserRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository UserRepository)
        {
            _logger = logger;
            _IUserRepository = UserRepository;
        }

        public IActionResult Index()
        {
            if (Request.Cookies.ContainsKey("UserName") && !string.IsNullOrEmpty(Request.Cookies["UserName"]))
            {
                string UserName = Request.Cookies["UserName"];
                ViewBag.IsLoggedIn = true;
                ViewBag.IsAdmin = _IUserRepository.IsUserAdmin(UserName);
            }
            else
            {
                ViewBag.IsLoggedIn = false;
                ViewBag.IsAdmin = false;
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
