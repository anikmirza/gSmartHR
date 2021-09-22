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
    public class AttendanceController : Controller
    {
        private readonly ILogger<AttendanceController> _logger;
        private IAttendanceRepository _IAttendanceRepository;
        private IUserRepository _IUserRepository;

        public AttendanceController(ILogger<AttendanceController> logger, IAttendanceRepository AttendanceRepository, IUserRepository UserRepository)
        {
            _logger = logger;
            _IAttendanceRepository = AttendanceRepository;
            _IUserRepository = UserRepository;
        }

        public IActionResult Index()
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
        public string LoadData(int? page, int? rows, string sort, string order, string EmployeeIdNo = null, string StrStartDate = null, string StrEndDate = null)
        {
        	int totalData = 0;
        	List<Dictionary<string, object>> _List = _IAttendanceRepository.GetAttendanceList(page.Value, rows.Value, EmployeeIdNo, StrStartDate, StrEndDate, out totalData);

            int SkipRowsCount = (int)((page - 1) * rows);
            int TakeRowsCount = (int)(rows);

            List<dynamic> DataList = new List<dynamic>();
            foreach (var ListItem in _List)
            {
            	DataList.Add(new {
            		EmployeeIdNo = ListItem["EmployeeIdNo"].ToString(),
            		Name = ListItem["Name"].ToString(),
            		StartDateTime = ListItem["StartDateTime"].ToString(),
            		EndDateTime = ListItem["EndDateTime"].ToString()
        		});
            }
            return JsonSerializer.Serialize(new { total = totalData, rows = DataList });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
