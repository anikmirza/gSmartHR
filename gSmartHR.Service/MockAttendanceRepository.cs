using System;
using gSmartHR.BLL;
using System.Collections.Generic;

namespace gSmartHR.Service
{
    public class MockAttendanceRepository : IAttendanceRepository
    {
        public List<Dictionary<string, object>> GetAttendanceList(int PageNo, int DataPerPage, string EmployeeIdNo, string StrStartDate, string StrEndDate, out int AttendanceListTotal)
        {
            AttendanceListTotal = 0;
            Attendance_BLL Helper = new Attendance_BLL();
            return Helper.GetAttendanceList(PageNo, DataPerPage, EmployeeIdNo, StrStartDate, StrEndDate, out AttendanceListTotal);
        }
    }
}
