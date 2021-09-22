using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using gSmartHR.Models;
using gSmartHR.Service;
using gSmartHR.DAL.Models.Obj;
using gSmartHR.Library;

namespace gSmartHR.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private IEmployeeRepository _IEmployeeRepository;
        private IUserRepository _IUserRepository;
        private string AttachmentFolder = "Upload/ProfilePic";
        private string ImagePath = "";

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository EmployeeRepository, IUserRepository UserRepository)
        {
            _logger = logger;
            _IEmployeeRepository = EmployeeRepository;
            _IUserRepository = UserRepository;
        }

        public IActionResult Profile()
        {
            if (Request.Cookies.ContainsKey("UserName") && !string.IsNullOrEmpty(Request.Cookies["UserName"]))
            {
                ViewBag.IsLoggedIn = true;
                string UserName = Request.Cookies["UserName"];
                ViewBag.IsAdmin = _IUserRepository.IsUserAdmin(UserName);
                EmployeeProfile ProfileInfo = _IEmployeeRepository.GetProfileInfo(UserName);
                ViewBag.firstname = ProfileInfo.FirstName;
                ViewBag.lastname = ProfileInfo.LastName;
                ViewBag.email = ProfileInfo.Email;
                ViewBag.contactno = ProfileInfo.ContactNo;
                ViewBag.nationalid = ProfileInfo.NationalId;
                ViewBag.imagepath = ProfileInfo.EmployeeImagePath;
                return View();
            }
            else return RedirectToAction("Login", "Auth");
        }

        public IActionResult UserList()
        {
            if (Request.Cookies.ContainsKey("UserName") && !string.IsNullOrEmpty(Request.Cookies["UserName"]))
            {
                ViewBag.IsLoggedIn = true;
                string UserName = Request.Cookies["UserName"];
                if (_IUserRepository.IsUserAdmin(UserName))
                {
                    ViewBag.IsAdmin = true;
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Auth");
        }

        public IActionResult EmployeeList()
        {
            if (Request.Cookies.ContainsKey("UserName") && !string.IsNullOrEmpty(Request.Cookies["UserName"]))
            {
                ViewBag.IsLoggedIn = true;
                string UserName = Request.Cookies["UserName"];
                if (_IUserRepository.IsUserAdmin(UserName))
                {
                    ViewBag.IsAdmin = true;
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public string UpdateProfile()
        {
            if (Request.Cookies.ContainsKey("UserName") && !string.IsNullOrEmpty(Request.Cookies["UserName"]))
            {
                string UserName = Request.Cookies["UserName"];
                UploadAttachments();
                string firstname = Request.Form["firstname"].ToString();
                string lastname = Request.Form["lastname"].ToString();
                string email = Request.Form["email"].ToString();
                string contactno = Request.Form["contactno"].ToString();
                string nationalid = Request.Form["nationalid"].ToString();
                _IEmployeeRepository.UpdateProfile(UserName, firstname, lastname, email, contactno, nationalid, ImagePath);
                return "/Home/Index";
            }
            else return "/Auth/Login";
        }

        [HttpPost]
        public string LoadUserData(int? page, int? rows, string sort, string order, string UserName = null, string Role = null)
        {
            int totalData = 0;
            List<Dictionary<string, object>> _List = _IEmployeeRepository.GetUserList(page.Value, rows.Value, UserName, Role, out totalData);

            int SkipRowsCount = (int)((page - 1) * rows);
            int TakeRowsCount = (int)(rows);

            List<dynamic> DataList = new List<dynamic>();
            foreach (var ListItem in _List)
            {
                DataList.Add(new {
                    UserName = ListItem["UserName"].ToString(),
                    Role = ListItem["RoleName"].ToString()
                });
            }
            return JsonSerializer.Serialize(new { total = totalData, rows = DataList });
        }

        [HttpPost]
        public string LoadEmployeeData(int? page, int? rows, string sort, string order, string Code = null, string Name = null, string Email = null, string Department = null, string Designation = null, string Cellphone = null, string OfficeName = null)
        {
            int totalData = 0;
            List<Dictionary<string, object>> _List = _IEmployeeRepository.GetEmployeeList(page.Value, rows.Value, Code, Name, Email, Department, Designation, Cellphone, OfficeName, out totalData);

            int SkipRowsCount = (int)((page - 1) * rows);
            int TakeRowsCount = (int)(rows);

            List<dynamic> DataList = new List<dynamic>();
            foreach (var ListItem in _List)
            {
                DataList.Add(new {
                    Id = ListItem["Id"].ToString(),
                    Code = ListItem["EmployeeIdNo"].ToString(),
                    Name = ListItem["FirstName"].ToString() + " " + ListItem["LastName"].ToString(),
                    Email = ListItem["Email"].ToString(),
                    Department = ListItem["Department"].ToString(),
                    Designation = ListItem["Designaton"].ToString(),
                    Cellphone = ListItem["ContactNo"].ToString(),
                    OfficeName = ListItem["OfficeName"].ToString()
                });
            }
            return JsonSerializer.Serialize(new { total = totalData, rows = DataList });
        }

        private void UploadAttachments()
        {
            Random random = new Random();
            string FileNamePrefix = DateTime.Now.ToString("yyyyMMddHHmmss") + Common.RandomString(random, 6) + '-';
            IFormFileCollection _Files = Request.Form.Files;
            string webRootPath = Common.WebRootPath();
            string newPath = Path.Combine(webRootPath, "wwwroot/" + AttachmentFolder);

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            foreach (IFormFile file in _Files)
            {
                if (file.Length > 0)
                {
                    string fullPath = Path.Combine(newPath, FileNamePrefix + file.FileName);
                    ImagePath = "/" + AttachmentFolder + "/" + FileNamePrefix + file.FileName;

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                    }
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
