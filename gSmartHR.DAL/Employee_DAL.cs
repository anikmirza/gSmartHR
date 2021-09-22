using System;
using System.Linq;
using System.Collections.Generic;
using gSmartHR.DAL.Models;
using gSmartHR.DAL.Models.Obj;

namespace gSmartHR.DAL
{
    public class Employee_DAL
    {
        public DBSContext DB;

        public Employee_DAL()
        {
            DB = new DBSContext();
        }

        public EmployeeProfile GetProfileInfo(string UserName)
        {
            var Data = (from U in DB.ApplicationUsers
                join E in DB.Employees on U.EmployeeId equals E.Id
                where U.UserName == UserName
                select new EmployeeProfile {
                    FirstName = E.FirstName,
                    LastName = E.LastName,
                    Email = E.Email,
                    ContactNo = E.ContactNo,
                    NationalId = E.NationalId,
                    EmployeeImagePath = E.EmployeeImagePath
                }).FirstOrDefault();
            return Data;
        }

        public void UpdateProfile(string UserName, string FirstName, string LastName, string Email, string ContactNo, string NationalId, string ImagePath)
        {
            int Id = (from U in DB.ApplicationUsers
                join E in DB.Employees on U.EmployeeId equals E.Id
                where U.UserName == UserName
                select E.Id).FirstOrDefault();
            Employee _Employee = DB.Employees.Find(Id);
            if (null != _Employee)
            {
                _Employee.FirstName = FirstName;
                _Employee.LastName = LastName;
                _Employee.Email = Email;
                _Employee.ContactNo = ContactNo;
                _Employee.NationalId = NationalId;
                _Employee.EmployeeImagePath = ImagePath;
            }
        }

        public void Save()
        {
            DB.SaveChanges();
        }
    }
}
