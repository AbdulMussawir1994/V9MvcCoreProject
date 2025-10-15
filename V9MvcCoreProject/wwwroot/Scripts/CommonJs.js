function LogOut() {
    debugger
    var token = $('input[name="__RequestVerificationToken"]').val(); // Get the anti-forgery token value

    $.ajax({
        type: "POST",
        url: "Account/LogOut",
        success: function (response) {
            debugger
            if (response == "LogOut") {
                var url = "/Account/Login";
                window.location.replace(url);
            }

        },

    });
}
$(function () { /* DOM ready */



    $("#ScheduleDate").change(function () {

        var values = {};
        values["ScheduleDate"] = $("#ScheduleDate option:selected").text();
        values["ReportConfigId"] = $('#ReportConfigId').val();
        values["ReportScheduleId"] = $('#ScheduleDate').val();       
        $.ajax({
            type: "POST",
            url: "Report/GetToolRunDateByScheduleDate",
            data: values,

            success: function (response) {
                $('#ToolRun').html("");
                $('#ToolRun').append(`<option value="0">Please Select Tool Run Date</option>`);
                for (var i = 0; i < response.length; i++) {
                    $('#ToolRun').append(`<option>` + response[i] + `</option>`);
                }

            },

        });
    });
});
var LoaderDisable = false;
$(document).ajaxStart(function (event, jqxhr, settings) {
    if (LoaderDisable === false) {
        $.blockUI({
            message: "<img style='height: 40px' src='/Content/assest/loader.gif' />",
            css: { width: "150px", left: "45%", "position": "absolute", "z-index":"1050"  }
        });
    }

}).ajaxStop(function () {

    $.unblockUI();
});
$(document).ajaxError(function () {
    $.unblockUI();
});
$(function () {
    //setup ajax error handling
    $.ajaxSetup({
        error: function (x, status, error) {
            //if (x.status == 401) {
            //    var url = "/Account/Login";
            //    window.location.replace(url);
            //}
            if (x.status === 401) {
                UnauthorizeAlert();
            }

        }
    });
});
function UnauthorizeAlert() {
    swal.fire({
        title: 'Unauthorized',
        text: 'You do not have permission to perform this operation',
        type: "error",

        buttonsStyling: true,

        confirmButtonText: "<i class='la la-check-circle'></i>ok",
        confirmButtonClass: "btn btn-default",

        showCancelButton: false
    })
}


function DefaultAlert() {
    swal.fire({
        title: 'Success',
        text: 'Record has been inserted successfully',
        type: "success",

        buttonsStyling: false,

        confirmButtonText: "<i class='la la-check-circle'></i>ok",
        confirmButtonClass: "btn btn-default",

        showCancelButton: false
    }).then((result) => {
        if (result.value) {
            window.location.href = location.href
        }
    });
}


// Report Queries Listing method
function showDistributionQueriesByScheduleDate(scheduleId, processDate, columnValueId) {

    let DistributionQueriesResultDTO = {};
    DistributionQueriesResultDTO.ColumnValueId = columnValueId;
    DistributionQueriesResultDTO.ScheduleId = scheduleId;
    DistributionQueriesResultDTO.ProcessDate = processDate;

    $.ajax({

        type: "POST",
        url: "Report/GetDistributionQueriesResult",
        data: DistributionQueriesResultDTO,

        success: function (data) {

            // console.log(data)
            if (data !== null && data !== undefined && data.length > 0) {

                createDistributionTable(data);
            }
            else {

                console.log("No data...........");
            }

            $('#kt_modal_1').modal('show');

        },
        error: function (response) {
            var r = jQuery.parseJSON(response.responseText);
            alert("Message: " + r.Message);
            alert("StackTrace: " + r.StackTrace);
            alert("ExceptionType: " + r.ExceptionType);
        }
    });

    return false;
}

function createDistributionTable(data) {

    var parse = JSON.parse(data);
    var gridColumns = parse.GridColumns;
    var gridTable = parse.GridTable;

    //// 
    //// CREATE DYNAMIC TABLE.
    var table = document.createElement("table");
    table.setAttribute("class", "tblReportsList table table-striped- table-bordered table-hover table-checkable");

    //// CREATE HTML TABLE HEADER ROW USING THE EXTRACTED HEADERS ABOVE.

    //// TABLE ROW.
    var tr = table.insertRow(-1);
    tr.setAttribute("class", "GreyColor");

    //// TABLE HEADER.
    let columnCount = gridColumns.length - 1;
    for (var i = 0; i < gridColumns.length; i++) {

        var th = document.createElement("th");
        th.innerHTML = gridColumns[i];
        tr.appendChild(th);
    }

    //// ADD JSON DATA TO THE TABLE AS ROWS.
    $.each(gridTable, function (index, item) {

        tr = table.insertRow(-1);

        $.each(gridColumns, function (colIndex, item) {

            //alert(gridTable[index][gridColumns[colIndex]]);
            //alert(gridColumns[index]);
            var tabCell = tr.insertCell(-1);
            tabCell.innerHTML = formatCellValue(columnCount, colIndex, gridTable[index][gridColumns[colIndex]], gridColumns[colIndex]);
        });

    });

    // FINALLY ADD THE NEWLY CREATED TABLE WITH JSON DATA TO A CONTAINER.
    var dvTable = document.getElementById("dvTable");
    dvTable.innerHTML = "";
    dvTable.appendChild(table);
}

// Not in use - will remove
function showDistributionQueries(reportId, headingId, columnValueId) {

    let requestURL = "/Report/GetReportCellQueriesResults?reportId=" + reportId + "&headingId=" + headingId + "&columnValueId=" + columnValueId;
    // alert("requestURL: " + requestURL);

    // let listItem = "<ul>";

    $.ajax({

        type: "GET",
        url: requestURL,

        success: function (data) {

            // console.log(data)
            if (data !== null && data !== undefined && data.length > 0) {

                var parse = JSON.parse(data);
                var gridColumns = parse.GridColumns;
                var gridTable = parse.GridTable;

                // 

                //// CREATE DYNAMIC TABLE.
                var table = document.createElement("table");
                table.setAttribute("class", "tblReportsList table table-striped- table-bordered table-hover table-checkable");

                //// CREATE HTML TABLE HEADER ROW USING THE EXTRACTED HEADERS ABOVE.

                //// TABLE ROW.
                var tr = table.insertRow(-1);
                tr.setAttribute("class", "GreyColor");

                //// TABLE HEADER.
                let columnCount = gridColumns.length - 1;
                for (var i = 0; i < gridColumns.length; i++) {

                    var th = document.createElement("th");
                    th.innerHTML = gridColumns[i];
                    tr.appendChild(th);
                }

                //// ADD JSON DATA TO THE TABLE AS ROWS.
                $.each(gridTable, function (index, item) {

                    tr = table.insertRow(-1);

                    $.each(gridColumns, function (colIndex, item) {

                        //alert(gridTable[index][gridColumns[colIndex]]);
                        //alert(gridColumns[index]);
                        var tabCell = tr.insertCell(-1);
                        tabCell.innerHTML = formatCellValue(columnCount, colIndex, gridTable[index][gridColumns[colIndex]]);
                    });

                });

                // FINALLY ADD THE NEWLY CREATED TABLE WITH JSON DATA TO A CONTAINER.
                var dvTable = document.getElementById("dvTable");
                dvTable.innerHTML = "";
                dvTable.appendChild(table);
            }
            else {

                console.log("No data...........");
            }

            $('#kt_modal_1').modal('show');

        },
        error: function (response) {
            var r = jQuery.parseJSON(response.responseText);
            alert("Message: " + r.Message);
            alert("StackTrace: " + r.StackTrace);
            alert("ExceptionType: " + r.ExceptionType);
        }
    });

    return false;
}

// https://www.tutorialsteacher.com/javascript/javascript-date
function formatCellValue(colCount, currentColIndex, colValue, colName) {

    let cellValue = colValue;

    // Error check
    if (colCount === 0)
        return cellValue;

    // Account Id Check
    // 
    if (colName === "AccountID" || colName === "Account_ID" || colName === "AccountId")
        return cellValue;

    //if (((colCount - currentColIndex) == 1) || (colCount == currentColIndex)) {

    //    var d = new Date(colValue);
    //    return d.toLocaleDateString();
    //}

    // Format value
    if (isNaN(cellValue))
        return cellValue;

    //var n = parseFloat(cellValue).toFixed(2); 
    //return Number(n).toLocaleString('en');
    return parseFloat(cellValue).toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
}


// Common methods
function doAlert(title, message) {

    $('#exampleModalLabel').text(title);
    $('#dvTable').html(message);
    $('#kt_modal_1').modal('show');
    return false;
}

function showDiv(dvId) {

    dvId = "#" + dvId;
    $(dvId).css('display', 'block');
}

function hideDiv(dvId) {

    dvId = "#" + dvId;
    $(dvId).css('display', 'none');
}

function setDivContent(dvId, content) {

    dvId = "#" + dvId;
    $(dvId).html(content);
}

function showError(msg) {
    showDiv("errordiv");
    setDivContent("errordivtext", msg);
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    setTimeout(function () { hideDiv("errordiv") }, 8000)
 }

// End Common methods