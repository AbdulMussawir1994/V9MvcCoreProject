jQuery(document).ready(function () {
  //  alert("Test")
});



function ConfirmSavePermissionTemplate() {

    var CancelClick = false;
    if (CancelClick) {
        CancelClick = false;
        return;
    }
    swal.fire({
        title: 'warning',
        text: 'Are you sure you want to create Role Template?',
        type: "warning",

        buttonsStyling: false,

        confirmButtonText: "<i class='la la-check-circle'></i>Confirm",
        confirmButtonClass: "btn btn-primary",

        showCancelButton: true,
        cancelButtonText: "<i class='la la-times-circle-o'></i>Cancel",
        cancelButtonClass: "btn btn-secondary"
    }).then((result) => {
        if (result.value) {
            SavePermissionTemplate();

        }
    })
}
function SavePermissionTemplate() {
    RemoveError();
    var TemplateName = $('#TemplateName').val();
    var IsActive = document.getElementById('IsActive').checked;
    var PermissionTemplateDTO = {};
    var checkedList = [];
    $('#tbodyid tr').each(function () {
        var form = $(this).find('td').find('input[type="text"]').val();
        $(this).find('td').each(function () {

            if ($(this).find('input[type="checkbox"]:checked').val() != undefined) {
                var functionalityList = {}
                functionalityList.FormName = form;
                functionalityList.FunctionalityId = $(this).find('input[type="checkbox"]:checked').val();
                functionalityList.IsAllow = true;
                checkedList.push(functionalityList);
            }

        });
    });

    if (TemplateName == "") {
        ShowError("Please Enter Role Name");
    }
    else if (checkedList.length == 0) {
        ShowError("Please Select Atleast 1 Permission Checkbox");
    }
    else {
        RemoveError();
        PermissionTemplateDTO.TemplateName = TemplateName;
        PermissionTemplateDTO.IsActive = IsActive;
        PermissionTemplateDTO.permissionTemplates = checkedList;

        $.ajax({
            type: "POST",
            url: "/Permission/SavePermissionTemplate",
            data: PermissionTemplateDTO,
            success: function (response) {
                debugger
                console.log("PermTem", response);

                if (response.success) {
                    swal.fire({
                        title: 'Success',
                        text: 'User has been created successfully',
                        type: "success",
                        buttonsStyling: false,
                        allowOutsideClick: false,
                        closeOnClickOutside: false,
                        confirmButtonText: "<i class='la la-check-circle'></i>ok",
                        confirmButtonClass: "btn btn-default",

                        showCancelButton: false
                    }).then((result) => {
                        if (result.value) {
                            var url = "/Permission/PermissionTemplate";
                            window.location.replace(url);
                        }
                    })
                }
                else {
                    swal.fire({
                        title: 'Error',
                        text: response.message,
                        type: "error"
                    });
                }
            },

        });
    }

}


function ShowError(errortext) {
    $('#errordiv').css('display', 'block');
    $('#errordivtext').text(errortext);
}
function RemoveError() {
    $('#errordiv').css('display', 'none');
    $('#errordivtext').text('');
}