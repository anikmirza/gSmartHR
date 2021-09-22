using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using gSmartHR.DAL.Models;
using gSmartHR.DAL.Models.Obj;

namespace gSmartHR.DAL
{
    public static class SQL_DAL
    {
        public static List<Dictionary<string, object>> GetDataSQL(string SQL, List<string> ParamName, List<object> ParamValue, bool StoredProcedure = false)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {
                MyResult _MyResult = DataAdapterQueryRequest(SQL, ParamName, ParamValue, StoredProcedure);

                if (!_MyResult.IsSuccess || _MyResult.Data == null)
                {
                    return rows;
                }
                DataSet oDataSet = (DataSet)_MyResult.Data;

                if (oDataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = oDataSet.Tables[0];
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                }
            }
            catch { }
            return rows;
        }

        public static MyResult DataAdapterQueryRequest(string SQL, List<string> ParamName, List<object> ParamValue, bool StoredProcedure = false)
        {
            MyResult oCResult = new MyResult();
            DataSet oDataSet = new DataSet();
            SqlConnection aSqlConnection = new SqlConnection(System.IO.File.ReadAllText("DBConnectionString.txt"));
            SqlCommand oSqlCommand = new SqlCommand(SQL);
            oSqlCommand.Connection = aSqlConnection;

            if (StoredProcedure)
            {
                oSqlCommand.CommandType = CommandType.StoredProcedure;
            }
            for (int i = 0; i < ParamName.Count; i++)
            {
                oSqlCommand.Parameters.AddWithValue(ParamName[i].ToString(), ParamValue[i].ToString());
            }
            SqlDataAdapter oSqlDataAdapter = new SqlDataAdapter(oSqlCommand);
            try
            {
                oSqlDataAdapter.Fill(oDataSet, "Common");
                oCResult.IsSuccess = true;
                oCResult.Message = "Successful";
                oCResult.Data = oDataSet;
            }
            catch (Exception ex)
            {
                oCResult.IsSuccess = false;
                oCResult.Message = ex.ToString();
            }
            if (aSqlConnection.State.ToString() == "Open")
            {
                aSqlConnection.Close();
            }
            return oCResult;
        }
    }
}
