$(function () {

    $("button[name='SaveSelectedUsers']").prop('disabled', true);

    var errorText = "An error has occured. An administrator has been notified. Please try again sometime.";
    $('select').on('change', function () {

        var url = "/Admin/UsersToCategory/GetusersForCategory?categoryId=" + this.value;

        if (this.value != null) {

            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    $("#UsersCheckList").html(data);
                    $("button[name='SaveSelectedUsers']").prop('disabled', false);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    PresentClosableBoostrapAlert("#alert_placeholder", "danger", "Error!", errorText);
                   // console.error("An error has occured" + thrownError + "Status: " + xhr.status + "\r\n" + xhr.responseText);
                }

            });

        }
        else
        {
            $("button[name='SaveSelectedUsers']").prop('disabled', true);
            $("input[type='checkbox']").prop('checked', false);
            $("input[type='checkbox']").prop('disabled', true);

        }


    });

   
    $("#SaveSelectedUsers").click(function () {

        var url = "/Admin/UsersToCategory/SaveSelectedUsers";

        var categoryId = $("#CategoryId").val();
        var anitiforgeryKey = $("input[name='__RequestVerificationToken']").val();
        var selectedUsers = [];

        DisableControls(true);

        $(".progress").show("fade");
        $("input[type='checkbox']:checked").each(function () {

            var userModel = {
                Id: $(this).attr("value")
            };
            selectedUsers.push(userModel);
        });



        var selectUserForCategory = {
            __RequestVerificationToken : anitiforgeryKey,
            CategoryId: categoryId,
            UsersSelected: selectedUsers
        }

        $.ajax({
            type: "POST",
            url:url,
            data: selectUserForCategory,
            success: function (data) {
                $("#UsersCheckList").html(data);

                $(".progress").hide("fade", function () {

                    $(".alert-success").fadeTo(2000, 500).slideUp(500, function () {

                        DisableControls(false);

                    });

                });

            },
            error: function (xhr, ajaxOptions, thrownError) {

                $(".progress").hide("fade", function () {

                    PresentClosableBoostrapAlert("#alert_placeholder", "danger", "Error!", errorText);
                    console.error("An error has occured" + thrownError + "Status: " + xhr.status + "\r\n" + xhr.responseText)

                    DisableControls(false);
                });
            }

        });

    });

    function DisableControls(disable) {
        $('input[type=checkbox]').prop("disabled", disable);
        $("#SaveSelectedUsers").prop('disabled', disable);
        $('select').prop('disabled', disable);
    }



});