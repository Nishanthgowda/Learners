$(function () {
    var loginUserButton = $("#UserLoginModal button[name='Login']").click(OnUserLoginClick);

    function OnUserLoginClick() {

        var url = "UserAuth/Login";

        var antiForgeryToken = $("#UserLoginModal input[name='__RequestVerificationToken']").val();    // input[name='__RequestVerificationToken']  automatically created by mvc

        var email = $("#UserLoginModal input[name= 'Email']").val();
        var password = $("#UserLoginModal input[name= 'Password']").val();
        var rememberMe = $("#UserLoginModal input[name= 'RememberMe']").prop('.checked');

        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            RememberMe: rememberMe
        };

        $.ajax({
            type: "POST",
            url: url,
            data: userInput,
            success: function (data){

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name='LoginInValid']").val() == "true";
                if (hasErrors) {
                    $("#UserLoginModal").html(data);
                    userLoginModal = $("#UserLoginModal button[name='Login']").click(OnUserLoginClick);


                    //to make UI more responsive we use unobtrusive client side validation
                    var form = $("#UserLoginForm");

                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                }
                else {
                    window.location.href = 'Home/Index';
                }
            },
            error: function (xhr, ajaxOptions, thrownErrors) {
                var errorText = "Status: " + xhr.status + "-" + xhr.statusTex;
                PresentClosableBoostrapAlert("#alert_placeholder_login", "danger", "Error!", errorText);
                //console.log(thrownErrors + "\r\n" + xhr.statusText + "\r\t" + xhr.responseText);
            }
        });


    }
});