jQuery(document).ready(function () {

});


function ShowPermissionTemplate() {
    if ($('#Templates').val() == "") {
        ShowError('Please select Role')
        return;
    }
    else {
        RemoveError();
        var templateId = $('#Templates').val();
        window.location.href = '/Permission/ChangePermissionTemplate?TempId=' + templateId;
    }
}

function ConfirmUpdatePermissionTemplate() {

    var CancelClick = false;
    if (CancelClick) {
        CancelClick = false;
        return;
    }
    swal.fire({
        title: 'warning',
        text: 'Are you sure you want to update role Template?',
        type: "warning",

        buttonsStyling: false,

        confirmButtonText: "<i class='la la-check-circle'></i>Confirm",
        confirmButtonClass: "btn btn-primary",

        showCancelButton: true,
        cancelButtonText: "<i class='la la-times-circle-o'></i>Cancel",
        cancelButtonClass: "btn btn-secondary"
    }).then((result) => {
        if (result.value) {
            UpdatePermissionTemplate();

        }
    })
}

function UpdatePermissionTemplate() {
    RemoveError();
    var TemplateName = $('#TemplateName').val();
    var TemplateId = $('#Id').val();
    var IsActive = document.getElementById('IsActive').checked == true ? true : false;
    var PermissionTemplateDTO = {};
    var checkedList = [];
    $('#tbodyid tr').each(function () {
        var form = $(this).find('td').find('input[type="text"]').val();
        $(this).find('td').each(function () {

            if ($(this).find('input[type="checkbox"]:checked').val() != undefined) {
                debugger
                var functionalityList = {}
                functionalityList.FormName = form;
                functionalityList.Id = $(this).find('input[type="checkbox"]:checked').attr('id');
                functionalityList.FunctionalityId = $(this).find('input[type="checkbox"]:checked').val();
                functionalityList.IsAllow = true;
                functionalityList.FormDisplayName = $(this).find('input[type="checkbox"]:checked').data('formdisplayname');
                functionalityList.FunctionalityName = $(this).find('input[type="checkbox"]:checked').data('functionalityname');
                checkedList.push(functionalityList);
            }

        });
    });
    debugger
    if (TemplateName == "") {
        ShowError("Please Enter Role Name");
    }
    else if (checkedList.length == 0) {
        ShowError("Please Select Atleast 1 Permission Checkbox");
    }
    else {
        RemoveError();
        PermissionTemplateDTO.Id = TemplateId;
        PermissionTemplateDTO.TemplateName = TemplateName;
        PermissionTemplateDTO.IsActive = IsActive;
        PermissionTemplateDTO.permissionTemplates = checkedList;

        $.ajax({
            type: "POST",
            url: "/Permission/UpdatePermissionTemplate",
            data: PermissionTemplateDTO,
            success: function (response) {

                if (response.success) {
                    swal.fire({
                        title: 'Success',
                        text: 'Request has been created Successfully',
                        type: "success",
                        allowOutsideClick: false,
                        closeOnClickOutside: false,
                        buttonsStyling: false,

                        confirmButtonText: "<i class='la la-check-circle'></i>ok",
                        confirmButtonClass: "btn btn-default",

                        showCancelButton: false
                    }).then((result) => {
                        if (result.value) {
                            var url = "/Permission/ChangePermissionTemplate";
                            window.location.replace(url);
                        }
                    })
                }
                else {
                    showError(response.ErrorMessage);
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

function setUserPageMode(readOnly) {

    if (readOnly) {

        showError("This template is under review and cannot be changed.");
        $('#btnAdd').hide();
    }
    else {

        $('#btnAdd').show();
    }

}