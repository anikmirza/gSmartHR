using System;
using System.Collections.Generic;

namespace gSmartHR.Service
{
    public interface IAttendanceRepository
    {
        public List<Dictionary<string, object>> GetAttendanceList(int PageNo, int DataPerPage, string EmployeeIdNo, string StrStartDate, string StrEndDate, out int AttendanceListTotal);
    }
}
