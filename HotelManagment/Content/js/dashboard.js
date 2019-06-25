(function ($) {
    'use strict';
    $(function () {
        $('#example').DataTable();
        $('.dataTable').DataTable();
        $('.select2').select2({
            closeOnSelect: false,
            placeholder: "Select..."
        });
        if ($(window).width() > 768) {
            $('#keyboard').keyboard();
        }

    });
    var table = $('.dataTable').DataTable();

    $('#tblcategory tbody').on('click', 'tr td.catedit', function () {
        var d = $(this).closest("tr")[0].children;
        $("#model_CategoryID").val(d[0].textContent);
        $("#model_CategoryName").val(d[1].textContent);
        $("#model_Description").val(d[2].textContent);
        if (d[3].textContent == true || d[3].textContent == "True") {
            $("#model_Status").prop("checked", true);
        }
        else {
            $("#model_Status").prop("checked", false);
        }

    });

    //edit is working on paging as well
    $('#tblSubCategory tbody').on('click', 'tr td button.subcatedit', function () {

        var d = $(this).closest("tr")[0].children;

        $("#model_SubCategoryId").val(d[0].textContent);
        $("#model_SubCategoryName").val(d[1].textContent);
        $("#model_Description").val(d[3].textContent);
        $("#model_Priority").val(d[4].textContent).trigger("change");
        var category = d[6].textContent.split(',');
        if (d[5].textContent == true || d[5].textContent == "True") {
            $('#model_Status').prop('checked', true);
        }
        else {
            $('#model_Status').prop('checked', false);
        }
        $("#categoryIds").val(category).trigger("change");
    });
    $('button[type=reset]').click(function () {
        $('.select2').val(null).trigger('change');
    });
    //Update save to update on button click
    //$('button.subcatedit').click(function () {
    //    $('button[type=submit]').text("Update").removeClass('btn-primary').addClass('btn-warning');
    //});

    $('#tbllocations tbody').on('click', 'tr td button.locationedit', function () {
        var d = $(this).closest("tr")[0].children;
        $("#model_LocationId").val(d[0].textContent);
        $("#model_LocationName").val(d[1].textContent);

        if (d[2].textContent == true || d[2].textContent == "True") {
            $("#model_Status").prop("checked", true);
        }
        else {
            $("#model_Status").prop("checked", false);
        }

    });
    $('#tblusers tbody').on('click', 'tr td button.useredit', function () {

        var d = $(this).closest("tr")[0].children;
        $("#model_UserId").val(d[0].textContent);
        $("#model_UserName").val(d[1].textContent);
        $("#model_EmailId").val(d[2].textContent);
        $("#model_Password").val(d[6].textContent);
        $("#model_LocationId").val(d[4].textContent);
        if (d[3].textContent == true || d[3].textContent == "True") {
            $("#model_Status").prop("checked", true);
        }
        else {
            $("#model_Status").prop("checked", false);
        }
        $('#AddEditSection').slideDown();

    });
    $('#tblroles tbody').on('click', 'tr td button.roleedit', function () {
        var d = $(this).closest("tr")[0].children;
        $("#model_RoleId").val(d[0].textContent);
        $("#model_RoleName").val(d[1].textContent);
        $("#model_RoleDescription").val(d[2].textContent);
        if (d[3].textContent == true || d[3].textContent == "True") {
            $("#model_Status").prop("checked", true);
        }
        else {
            $("#model_Status").prop("checked", false);
        }
    });
    $('#tblassignroles tbody').on('click', 'tr td button.assignRolesedit', function () {
        var d = $(this).closest("tr")[0].children;

        $("#model_UserRoleLinkId").val(d[0].textContent);
        $("#model_UserId").val(d[2].textContent).trigger("change");
        $("#model_RoleId").val(d[1].textContent).trigger("change");
        $('#AddEditRole').slideDown();
        var dt = d[5].textContent.split('/');
        var fromdate = d[5].textContent.toString().split(' ')[0];
        $("#model_FromDate").val(fromdate);
        dt = d[6].textContent.split('/');
        var todate = d[6].textContent.toString().split(' ')[0];
        $("#model_ToDate").val(todate);
    });
    $('#culture').on('change', function (event) {
          
        var form = $(event.target).parents('form');
        form.submit();
    });
})(jQuery);


function BindScreenPermission(roleid) {
    fetch('/ManageRoles?handler=LoadScreenPermissions&roleid=' + roleid)
        .then((response) => {

            return response.text();
        })
        .then((result) => {

            $("#screenper").html(result);
            $('#myModal').modal('toggle');
            //document.getElementById('grid').innerHTML = result;
        });
}
function SavePermissions() {
    var data;
    var table = $('#dtpermissions');
    var list = [];
    $('#dtpermissions > tbody  > tr').each(function () {
        list.push({
            RoleId: 1,
            ScreenPermLinkId: $($(this)[0].children[2].children[0]).val()
        });
    });

    $.ajax({
        type: "POST",
        url: "/ManageRoles?handler=SaveScreenPermission",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: JSON.stringify(list),
        success: function (response) {
            if (response.success == true) {
                launch_toast("true", "Permission Saved Successfully");

                $('button[data-dismiss=modal').trigger("click");
            }
            else {
                launch_toast("false", "Error in Saving  Permissions");

            }

        },
        failure: function (response) {
            launch_toast("false", response.text);
        }
    });
}


function SaveTestDetails() {

    if ($("#nav-tabContent input:checkbox:checked").length == 0) {
        launch_toast("false", "Please select any category for saving test");
        return;
    }
    var list = [];
    $('.testcategory').each(function (i, obj) {
        var categoryid = obj.attributes["data-cateid"].value;
        var subcategorylist = [];
        var elemts = $("#" + obj.id + " input[name='checkbox']:checked");

        $.each(elemts, function (k, v) {
            subcategorylist.push({
                SubCategoryId: v.attributes["data-subcatid"].value
            })
        });

        list.push({
            HarvestId: $("#keyboard").val(),
            CategoryId: categoryid,
            subcategorylist: subcategorylist
        });

    });


    $.ajax({
        type: "POST",
        url: "/StartTest?handler=SaveTestDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: JSON.stringify(list),
        success: function (response) {
            if (response.success == true) {

                launch_toast("true", "Test Saved Successfully");
                $("#nav-tabContent input:checkbox").prop('checked', false);

            }
            else {
                launch_toast("false", "Error in Saving  Test");

            }

        },
        failure: function (response) {
            launch_toast("false", response);

        }
    });
}


function ValidateChecks() {
    if ($("#btnNext").is(":disabled")) {
        launch_toast("false", "Please click on starttest to before saving");
        return;
    }
    if ($("#btnFinish").is(":disabled")) {
        launch_toast("false", "Please click on starttest to before saving");
        return;
    }
}


function FinishTest() {

    $.ajax({
        type: "POST",
        url: "/StartTest?handler=FinishTestDetails&harvestId=" + $("#keyboard").val(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: {},
        success: function (response) {
            if (response.success == true) {
                launch_toast("true", "Test Finished Successfully");

                window.location.href = "/Index";

            }
            else {
                launch_toast("false", "Error in Saving  Test");

            }

        },
        failure: function (response) {
            launch_toast("false", response);
        }
    });
}

