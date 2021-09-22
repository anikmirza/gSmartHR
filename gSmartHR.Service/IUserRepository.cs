using System;
using System.Collections.Generic;

namespace gSmartHR.Service
{
    public interface IUserRepository
    {
        public string Login(string UserName, string Password);
        public string Register(string username, string firstname, string lastname, string password, string confirmpassword);
        public string MarkAsUnattended(string UserName);
        public bool IsUserAdmin(string UserName);
    }
}
