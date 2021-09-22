using System.Data;

namespace gSmartHR.DAL.Models.Obj
{
    public class MyResult
    {
        public object Data { set; get; }
        public string Message { set; get; }
        public static DataTable DataTableContainer { set; get; }
        public DataTable DataTableContainerSub { set; get; }
        public DataSet DataSetValue { set; get; }
        public bool IsSuccess { set; get; }
    }
}