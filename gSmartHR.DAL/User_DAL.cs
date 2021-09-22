using System;
using System.Linq;
using System.Collections.Generic;
using gSmartHR.DAL.Models;
using gSmartHR.DAL.Models.Obj;

namespace gSmartHR.DAL
{
    public class User_DAL
    {
    	public DBSContext DB;

        public User_DAL()
        {
            DB = new DBSContext();
        }

        public HashSalt GetPasswordHashSaltByUserName(string UserName)
        {
            return DB.ApplicationUsers.Where(x => x.UserName == UserName)
                .Select(x => new HashSalt { Hash = x.PasswordHash, Salt = x.PasswordSalt }).FirstOrDefault();
        }

        public void GetIdEmployeeIdByUserName(string UserName, out int Id, out int EmployeeId)
        {
            var _User = DB.ApplicationUsers.Where(x => x.UserName == UserName)
                .Select(x => new { x.Id, x.EmployeeId }).FirstOrDefault();
            Id = null == _User ? 0 : _User.Id;
            EmployeeId = null == _User ? 0 : _User.EmployeeId;
        }

        public bool IsUserNameDuplicate(string UserName)
        {
            return DB.ApplicationUsers.Where(x => x.UserName == UserName).Count() > 0;
        }

        public bool IsUserActive(string UserName)
        {
            return DB.ApplicationUsers.Where(x => x.UserName == UserName && x.IsActive).Count() > 0;
        }

        public bool IsUserAdmin(string UserName)
        {
            return (from U in DB.ApplicationUsers
                join R in DB.UserRoles on U.RoleId equals R.Id
                where R.RoleName == "Administrator" && U.UserName == UserName
                select U.Id).Count() > 0;
        }

        public int GetStuffRoleId()
        {
            return DB.UserRoles.Where(x => x.RoleName == "Stuff").Select(x => x.Id).FirstOrDefault();
        }

        public string GetNextEmployeeIdNo()
        {
            if (0 == DB.Employees.Count()) return "1";
            int MaxNo = DB.Employees.Select(x => Convert.ToInt32(x.EmployeeIdNo)).Max(x => x);
            return (MaxNo + 1).ToString();
        }

        public void AddUser(ApplicationUser _ApplicationUser)
        {
            DB.ApplicationUsers.Add(_ApplicationUser);
        }

        public void AddAttendance(Attendance _Attendance)
        {
            DB.Attendances.Add(_Attendance);
        }

        public void MarkAsUnattended(long EmployeeId)
        {
            Attendance _Attendance = DB.Attendances.Where(x => x.EmployeeId == EmployeeId)
                .OrderByDescending(x => x.StartDateTime).FirstOrDefault();
            if (null != _Attendance) _Attendance.EndDateTime = DateTime.Now;
        }

        public void Save()
        {
            DB.SaveChanges();
        }
    }
}
