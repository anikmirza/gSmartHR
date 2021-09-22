
using System;
using System.Collections.Generic;
using gSmartHR.DAL.Models.Obj;

namespace gSmartHR.Service
{
    public interface IEmployeeRepository
    {
        public EmployeeProfile GetProfileInfo(string UserName);
        public string UpdateProfile(string UserName, string FirstName, string LastName, string Email, string ContactNo, string NationalId, string ImagePath);
        public List<Dictionary<string, object>> GetUserList(int PageNo, int DataPerPage, string UserName, string Role, out int ListTotal);
        public List<Dictionary<string, object>> GetEmployeeList(int PageNo, int DataPerPage, string Code, string Name, string Email, string Department, string Designation, string Cellphone, string OfficeName, out int ListTotal);
    }
}
