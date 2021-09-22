var Page = {
	Load: function() {
		Page.Search();
	},
	Search: function() {
        $('#tblDataGrid').datagrid('load', {
            EmployeeIdNo: $('#employeeidno').val(),
            StrStartDate: $('#fromdate').val(),
            StrEndDate: $('#todate').val()
        });
	}
};
$(document).ready(function(){
	Page.Load();
});