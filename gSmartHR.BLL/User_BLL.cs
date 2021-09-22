using System;
using System.Linq;
using System.Collections.Generic;
using gSmartHR.DAL;
using gSmartHR.DAL.Models;
using gSmartHR.DAL.Models.Obj;
using gSmartHR.BLL.Library;

namespace gSmartHR.BLL
{
    public class User_BLL
    {
        private string Message;

        public string Login(string UserName, string Password)
        {
        	try
        	{
	            if (!IsLoginFormValid(UserName, Password)) return Message;
	            User_DAL Helper = new User_DAL();
                if (!Helper.IsUserActive(UserName)) return "User is Inactive";
	            HashSalt _User = Helper.GetPasswordHashSaltByUserName(UserName);
	            if (null == _User) return "User does not exist!";
	            if (!Encryption.VerifyPassword(Password, _User.Salt, _User.Hash)) return "Incorrect password!";
                MarkAsAttended(Helper, UserName);
                Helper.Save();
	            return "success";
	        }
	        catch { return "An error occured"; }
        }

        public string MarkAsUnattended(string UserName)
        {
            try
            {
                User_DAL Helper = new User_DAL();
                int Id = 0, EmployeeId = 0;
                Helper.GetIdEmployeeIdByUserName(UserName, out Id, out EmployeeId);
                Helper.MarkAsUnattended(EmployeeId);
                Helper.Save();
                return "success";
            }
            catch { return "An error occured"; }
        }

        public string Register(string UserName, string FirstName, string LastName, string Password, string ConfirmPassword)
        {
            try
            {
                if (!IsRegisterFormValid(UserName, FirstName, LastName, Password, ConfirmPassword)) return Message;
                HashSalt _HashSalt = Encryption.EncryptPassword(Password);
                User_DAL Helper = new User_DAL();
                if (Helper.IsUserNameDuplicate(UserName)) return "Duplicate User Name Exists!";
                int RoleId = Helper.GetStuffRoleId();
                Employee _Employee = new Employee {
                    EmployeeIdNo = Helper.GetNextEmployeeIdNo(),
                    FirstName = FirstName,
                    LastName = LastName,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };
                Helper.AddUser(new ApplicationUser {
                    RoleId = RoleId,
                    UserName = UserName,
                    PasswordHash = _HashSalt.Hash,
                    PasswordSalt = _HashSalt.Salt,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Employee = _Employee
                });
                Helper.Save();
                return "success";
            }
            catch { return "An error occured"; }
        }

        private void MarkAsAttended(User_DAL Helper, string UserName)
        {
            int Id = 0, EmployeeId = 0;
            Helper.GetIdEmployeeIdByUserName(UserName, out Id, out EmployeeId);
            Helper.AddAttendance(new Attendance {
                EmployeeId = EmployeeId,
                StartDateTime = DateTime.Now,
                IsActive = true,
                CreatedBy = Id,
                CreatedDate = DateTime.Now
            });
        }

        public bool IsUserAdmin(string UserName)
        {
            User_DAL Helper = new User_DAL();
            return Helper.IsUserAdmin(UserName);
        }

        private bool IsLoginFormValid(string UserName, string Password)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                Message = "User Name is Required!";
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                Message = "Password is Required!";
                return false;
            }
            return true;
        }

        private bool IsRegisterFormValid(string UserName, string FirstName, string LastName, string Password, string ConfirmPassword)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                Message = "User Name is Required!";
                return false;
            }
            if (string.IsNullOrEmpty(FirstName))
            {
                Message = "First Name is Required!";
                return false;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                Message = "Last Name is Required!";
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                Message = "Password is Required!";
                return false;
            }
            if (Password.Length < 8)
            {
                Message = "Password must be atleast 8 character long!";
                return false;
            }
            if (Password.All(char.IsDigit))
            {
                Message = "Password must contain atleast one Letter!";
                return false;
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                Message = "Confirm Password is Required!";
                return false;
            }
            if (Password != ConfirmPassword)
            {
                Message = "Password does not match Confirm Password!";
                return false;
            }
            return true;
        }
    }
}
