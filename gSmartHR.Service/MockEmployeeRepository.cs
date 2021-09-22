using System;
using System.Collections.Generic;
using gSmartHR.BLL;
using gSmartHR.DAL.Models.Obj;

namespace gSmartHR.Service
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        public EmployeeProfile GetProfileInfo(string UserName)
        {
            Employee_BLL Helper = new Employee_BLL();
            return Helper.GetProfileInfo(UserName);
        }

        public string UpdateProfile(string UserName, string FirstName, string LastName, string Email, string ContactNo, string NationalId, string ImagePath)
        {
            Employee_BLL Helper = new Employee_BLL();
            return Helper.UpdateProfile(UserName, FirstName, LastName, Email, ContactNo, NationalId, ImagePath);
        }

        public List<Dictionary<string, object>> GetUserList(int PageNo, int DataPerPage, string UserName, string Role, out int ListTotal)
        {
            ListTotal = 0;
            Employee_BLL Helper = new Employee_BLL();
            return Helper.GetUserList(PageNo, DataPerPage, UserName, Role, out ListTotal);
        }

        public List<Dictionary<string, object>> GetEmployeeList(int PageNo, int DataPerPage, string Code, string Name, string Email, string Department, string Designation, string Cellphone, string OfficeName, out int ListTotal)
        {
            ListTotal = 0;
            Employee_BLL Helper = new Employee_BLL();
            return Helper.GetEmployeeList(PageNo, DataPerPage, Code, Name, Email, Department, Designation, Cellphone, OfficeName, out ListTotal);
        }
    }
}
