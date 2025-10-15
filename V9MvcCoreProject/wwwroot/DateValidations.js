$('#SelectEndDate').change(function () {
    var StartDate = $('#SelectStartDate').val();
    if (StartDate == "") {
        $('#SelectEndDate').val("");
        swal.fire({
            title: 'Warning',
            text: 'Please select start date first..!',
            type: "error",
            buttonsStyling: false,
            confirmButtonText: "<i class='la la-check-circle'></i>Ok",
            confirmButtonClass: "btn btn-default",
            showCancelButton: false
        })
    }

});

$('#SelectStartDate').change(function () {
    $('#SelectEndDate').val("");
    var StartDate = /(\d+)\/(\d+)\/(\d+)/.exec($('#SelectStartDate').val())
    StartDate = StartDate[2] + '/' + StartDate[1] + '/' + StartDate[3]
    var startOfEndDate = new Date(StartDate);
    var date = new Date(StartDate);
    var newdate = date.setDate(date.getDate() + 1);
    var newdate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    $('#SelectEndDate').datepicker('setEndDate', newdate);
    $('#SelectEndDate').datepicker('setStartDate', startOfEndDate);
});