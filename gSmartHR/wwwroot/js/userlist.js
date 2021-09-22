var Page = {
	Load: function() {
		Page.Search();
	},
	Search: function() {
        $('#tblDataGrid').datagrid('load', {
            UserName: $('#username').val(),
            Role: $('#role').val()
        });
	}
};
$(document).ready(function(){
	Page.Load();
});