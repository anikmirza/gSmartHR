using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using gSmartHR.Models;
using gSmartHR.Service;

namespace gSmartHR.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private IUserRepository _IUserRepository;

        public AuthController(ILogger<AuthController> logger, IUserRepository UserRepository)
        {
            _logger = logger;
            _IUserRepository = UserRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            try
            {
                if (Request.Cookies.ContainsKey("UserName"))
                {
                    string UserName = Request.Cookies["UserName"];
                    string Status = _IUserRepository.MarkAsUnattended(UserName);
                    if ("success" == Status) Response.Cookies.Delete("UserName");
                    else return Ok(Status);
                }
                return RedirectToAction("Login", "Auth");
            }
            catch { return Ok("An error occured"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public IActionResult Login(string username, string password)
        {
            ViewBag.username = username;
            ViewBag.password = password;
            string Status = _IUserRepository.Login(username, password);

            if ("success" != Status)
            {
                ViewBag.ErrorMessage = Status;
                return View();
            }
            CookieOptions COption = new CookieOptions();
            COption.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Append("UserName", username, COption);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public IActionResult Register(string username, string firstname, string lastname, string password, string confirmpassword)
        {
            ViewBag.username = username;
            ViewBag.firstname = firstname;
            ViewBag.lastname = lastname;
            ViewBag.password = password;
            ViewBag.confirmpassword = confirmpassword;
            string Status = _IUserRepository.Register(username, firstname, lastname, password, confirmpassword);

            if ("success" != Status)
            {
                ViewBag.ErrorMessage = Status;
                return View();
            }
            return RedirectToAction("Login", "Auth");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
