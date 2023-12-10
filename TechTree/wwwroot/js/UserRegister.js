$(function () {

    $("#UserRegistrationModal input[name='AcceptUserAgreement']").click(OnAcceptUserAgreement);
    $("#UserRegistrationModal button[name='register']").prop("disabled", true);

    $("#UserRegistrationModal input[name= 'Email']").blur(function () {

        var url = "UserAuth/UserNameExists?userName=" + $("#UserRegistrationModal input[name= 'Email']").val();
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {

                if (data == true) {

                    PresentClosableBoostrapAlert("#alert_placeholder_register", "warning", "Invalid Email","This email address has been already been registered");
                    //var alertHtml = '<div class="alert alert-warning alert-dismissible fade show" role="alert"><strong>Invalid Email</strong><br>This email address has been already been registered ' +
                    //    '<button type="button" class="btn-close" data-bs-dismiss = "alert" aria-label="Close">' + '</button><div >';

                    //$("#alert_placeholder_register").html(alertHtml);
                }
                else {
                   // $("#alert_placeholder_register").html("");
                    CloseAlert("#alert_placeholder_register");
                }
            },
            error: function (xhr, ajaxOptions, thrownErrors) {
                var errorText = "Status: " + xhr.status + "-" + xhr.statusTex;
                PresentClosableBoostrapAlert("#alert_placeholder_register", "danger", "Error!", errorText)
                console.error(thrownErrors + "\r\n" + xhr.statusText + "\r\t" + xhr.responseText);
            }
        });


    });

    function OnAcceptUserAgreement() {

        
        if ($(this).is(":checked")) {
            $("#UserRegistrationModal button[name='register']").prop("disabled", false);
        }
        else {
            $("#UserRegistrationModal button[name='register']").prop("disabled", true);
        }

    }

    var registrationUserButton = $("#UserRegistrationModal button[name='register']").click(OnUserRegisterClick);



    function OnUserRegisterClick() {

        var url = "UserAuth/RegisterUser";

        var antiForgeryToken = $("#UserRegistrationModal input[name='__RequestVerificationToken']").val();    // input[name='__RequestVerificationToken']  automatically created by mvc

        var email = $("#UserRegistrationModal input[name= 'Email']").val();
        var password = $("#UserRegistrationModal input[name= 'Password']").val();
        var confirmPassword = $("#UserRegistrationModal input[name= 'ConfirmPassword']").val();
        var firstName = $("#UserRegistrationModal input[name= 'FirstName']").val();
        var lastName = $("#UserRegistrationModal input[name= 'LastName']").val();
        var postCode = $("#UserRegistrationModal input[name= 'PostCode']").val();
        var address1 = $("#UserRegistrationModal input[name= 'Address1']").val();
        var address2 = $("#UserRegistrationModal input[name= 'Address2']").val();
        var phoneNumber = $("#UserRegistrationModal input[name= 'PhoneNumber']").val();


        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            FirstName: firstName,
            LastName: lastName,
            PostCode: postCode,
            Address1: address1,
            Address2: address2,
            PhoneNumber: phoneNumber,
            AcceptUserAgreement:true
        };

        $.ajax({
            type: "POST",
            url: url,
            data: userInput,
            success: function (data) {

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name='RegistrationInValid']").val() == "true";
                if (hasErrors) {
                    $("#UserRegistrationModal").html(data);
                    var registerUserButton = $("#UserRegistrationModal button[name='register']").click(OnUserRegisterClick);


                    //to make UI more responsive we use unobtrusive client side validation
                    var form = $("#UserRegisterForm");

                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                }
                else {
                    window.location.href = 'Home/Index';
                }
            },
            error: function (xhr, ajaxOptions, thrownErrors) {
                console.log(thrownErrors + "\r\n" + xhr.statusText + "\r\t" + xhr.responseText);
            }
        });


    }
});