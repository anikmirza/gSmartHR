using System;
using System.Collections.Generic;
using gSmartHR.BLL;

namespace gSmartHR.Service
{
    public class MockUserRepository : IUserRepository
    {
        public string Login(string UserName, string Password)
        {
            User_BLL Helper = new User_BLL();
            return Helper.Login(UserName, Password);
        }

        public string Register(string UserName, string FirstName, string LastName, string Password, string ConfirmPassword)
        {
            User_BLL Helper = new User_BLL();
            return Helper.Register(UserName, FirstName, LastName, Password, ConfirmPassword);
        }

        public string MarkAsUnattended(string UserName)
        {
            User_BLL Helper = new User_BLL();
            return Helper.MarkAsUnattended(UserName);
        }

        public bool IsUserAdmin(string UserName)
        {
            User_BLL Helper = new User_BLL();
            return Helper.IsUserAdmin(UserName);
        }
    }
}
