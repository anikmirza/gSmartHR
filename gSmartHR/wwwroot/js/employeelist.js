var Page = {
	Load: function() {
		Page.Search();
	},
	Search: function() {
        $('#tblDataGrid').datagrid('load', {
            Code: $('#code').val(),
            Name: $('#name').val(),
            Email: $('#email').val(),
            Department: $('#department').val(),
            Designation: $('#designation').val(),
            Cellphone: $('#cellphone').val(),
			OfficeName: $('#officename').val()
        });
	}
};
$(document).ready(function(){
	Page.Load();
});