$(document).ready(function () {

    GetListData();

});


function GetListData() {
    $.ajax({
        type: "GET",
        url: "/Home/GetSelectList",
        dataType: "json",
        success: function (response) {
            console.log("List data loaded:", response);

            const $dropDown = $("#listId");
            $dropDown.empty().append('<option value ="">Select List</option>');

            if (response && response.length > 0) {
                $.each(response, function (i, item) {
                    $dropDown.append($("<option></option>").val(item.Id).text(item.Name)
                    );
                });
            }
            else {
                $dropDown.append('<option value="">No Lists Available</option>');
            }
        },
        error: function (xhr) {
            console.error("Error loading list data:", xhr.responseText);
            alert(xhr);
        }
    });
}