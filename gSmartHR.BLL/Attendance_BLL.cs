using System;
using System.Collections.Generic;
using gSmartHR.DAL;
using gSmartHR.DAL.Models.Obj;

namespace gSmartHR.BLL
{
    public class Attendance_BLL
    {
        public List<Dictionary<string, object>> GetAttendanceList(int PageNo, int DataPerPage, string EmployeeIdNo, string StrStartDate, string StrEndDate, out int AttendanceListTotal)
        {
        	try
        	{
        		if (EmployeeIdNo == null) EmployeeIdNo = "";
	        	AttendanceListTotal = GetAttendanceListTotal(EmployeeIdNo, StrStartDate, StrEndDate);
				List<string> ParamName = new List<string>();
				List<object> ParamValue = new List<object>();
				ParamName.Add("PageNo");
				ParamValue.Add(PageNo);
				ParamName.Add("DataPerPage");
				ParamValue.Add(DataPerPage);
				ParamName.Add("EmployeeIdNo");
				ParamValue.Add(EmployeeIdNo);
				ParamName.Add("StrStartDate");
				ParamValue.Add(StrStartDate);
				ParamName.Add("StrEndDate");
				ParamValue.Add(StrEndDate);
	        	return SQL_DAL.GetDataSQL("SP_AttendanceList", ParamName, ParamValue, true);
	        }
	        catch
	        {
	        	AttendanceListTotal = 0;
	        	return new List<Dictionary<string, object>>();
	        }
        }

        private int GetAttendanceListTotal(string EmployeeIdNo, string StrStartDate, string StrEndDate)
        {
			List<string> ParamName = new List<string>();
			List<object> ParamValue = new List<object>();
			ParamName.Add("EmployeeIdNo");
			ParamValue.Add(EmployeeIdNo);
			ParamName.Add("StrStartDate");
			ParamValue.Add(StrStartDate);
			ParamName.Add("StrEndDate");
			ParamValue.Add(StrEndDate);
        	var _List = SQL_DAL.GetDataSQL("SP_AttendanceListTotal", ParamName, ParamValue, true);
        	return _List.Count > 0 ? Convert.ToInt32(_List[0]["Total"]) : 0;
        }
    }
}
