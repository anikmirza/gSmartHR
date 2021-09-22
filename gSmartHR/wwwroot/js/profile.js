var Page = {
	Save: function() {
        var fileExtension = ["png", "gif", "jpeg"];
        var filename = $('#fUpload').val();
        if (filename.length == 0) {
            toastr.error("Please select a file.");
            return false;
        }
        else {
            var extension = filename.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                toastr.error("Please select only image files.");
                return false;
            }
        }
        var fdata = new FormData();
        var fileUpload = $("#fUpload").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);

		var token = $("input[name='__RequestVerificationToken']").val();
		fdata.append("__RequestVerificationToken", token);
		fdata.append("firstname", $("#firstname").val());
		fdata.append("lastname", $("#lastname").val());
		fdata.append("email", $("#email").val());
		fdata.append("contactno", $("#contactno").val());
		fdata.append("nationalid", $("#nationalid").val());
        $.ajax({
            url: '/Employee/UpdateProfile',
            type: 'Post',
            data: fdata,
            async: true,
            contentType: false,
            processData: false,
            success: function (result) {
            	window.location.href = result;
            },
            error: function (data, textStatus, jqXHR) {
                toastr.error(data + ": " + textStatus + ": " + jqXHR + 'Error!!!');
            }
        });
	}
};