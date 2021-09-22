using System;
using System.Collections.Generic;
using gSmartHR.DAL;
using gSmartHR.DAL.Models.Obj;

namespace gSmartHR.BLL
{
    public class Employee_BLL
    {
        public EmployeeProfile GetProfileInfo(string UserName)
        {
            Employee_DAL Helper = new Employee_DAL();
            return Helper.GetProfileInfo(UserName);
        }

        public string UpdateProfile(string UserName, string FirstName, string LastName, string Email, string ContactNo, string NationalId, string ImagePath)
        {
        	try
        	{
	            Employee_DAL Helper = new Employee_DAL();
	            Helper.UpdateProfile(UserName, FirstName, LastName, Email, ContactNo, NationalId, ImagePath);
	            Helper.Save();
	            return "success";
            }
            catch { return "An error occured"; }
        }

        public List<Dictionary<string, object>> GetUserList(int PageNo, int DataPerPage, string UserName, string Role, out int ListTotal)
        {
            try
            {
                if (UserName == null) UserName = "";
                if (Role == null) Role = "";
                ListTotal = GetUserListTotal(UserName, Role);
                List<string> ParamName = new List<string>();
                List<object> ParamValue = new List<object>();
                ParamName.Add("PageNo");
                ParamValue.Add(PageNo);
                ParamName.Add("DataPerPage");
                ParamValue.Add(DataPerPage);
                ParamName.Add("UserName");
                ParamValue.Add(UserName);
                ParamName.Add("Role");
                ParamValue.Add(Role);
                return SQL_DAL.GetDataSQL("SP_UserList", ParamName, ParamValue, true);
            }
            catch
            {
                ListTotal = 0;
                return new List<Dictionary<string, object>>();
            }
        }

        private int GetUserListTotal(string UserName, string Role)
        {
            List<string> ParamName = new List<string>();
            List<object> ParamValue = new List<object>();
            ParamName.Add("UserName");
            ParamValue.Add(UserName);
            ParamName.Add("Role");
            ParamValue.Add(Role);
            var _List = SQL_DAL.GetDataSQL("SP_UserListTotal", ParamName, ParamValue, true);
            return _List.Count > 0 ? Convert.ToInt32(_List[0]["Total"]) : 0;
        }

        public List<Dictionary<string, object>> GetEmployeeList(int PageNo, int DataPerPage, string Code, string Name, string Email, string Department, string Designation, string Cellphone, string OfficeName, out int ListTotal)
        {
            try
            {
                if (Code == null) Code = "";
                if (Name == null) Name = "";
                if (Email == null) Email = "";
                if (Department == null) Department = "";
                if (Designation == null) Designation = "";
                if (Cellphone == null) Cellphone = "";
                if (OfficeName == null) OfficeName = "";
                ListTotal = GetEmployeeListTotal(Code, Name, Email, Department, Designation, Cellphone, OfficeName);
                List<string> ParamName = new List<string>();
                List<object> ParamValue = new List<object>();
                ParamName.Add("PageNo");
                ParamValue.Add(PageNo);
                ParamName.Add("DataPerPage");
                ParamValue.Add(DataPerPage);
                ParamName.Add("Code");
                ParamValue.Add(Code);
                ParamName.Add("Name");
                ParamValue.Add(Name);
                ParamName.Add("Email");
                ParamValue.Add(Email);
                ParamName.Add("Department");
                ParamValue.Add(Department);
                ParamName.Add("Designation");
                ParamValue.Add(Designation);
                ParamName.Add("Cellphone");
                ParamValue.Add(Cellphone);
                ParamName.Add("OfficeName");
                ParamValue.Add(OfficeName);
                return SQL_DAL.GetDataSQL("SP_EmployeeList", ParamName, ParamValue, true);
            }
            catch
            {
                ListTotal = 0;
                return new List<Dictionary<string, object>>();
            }
        }

        private int GetEmployeeListTotal(string Code, string Name, string Email, string Department, string Designation, string Cellphone, string OfficeName)
        {
            List<string> ParamName = new List<string>();
            List<object> ParamValue = new List<object>();
            ParamName.Add("Code");
            ParamValue.Add(Code);
            ParamName.Add("Name");
            ParamValue.Add(Name);
            ParamName.Add("Email");
            ParamValue.Add(Email);
            ParamName.Add("Department");
            ParamValue.Add(Department);
            ParamName.Add("Designation");
            ParamValue.Add(Designation);
            ParamName.Add("Cellphone");
            ParamValue.Add(Cellphone);
            ParamName.Add("OfficeName");
            ParamValue.Add(OfficeName);
            var _List = SQL_DAL.GetDataSQL("SP_EmployeeListTotal", ParamName, ParamValue, true);
            return _List.Count > 0 ? Convert.ToInt32(_List[0]["Total"]) : 0;
        }
    }
}
