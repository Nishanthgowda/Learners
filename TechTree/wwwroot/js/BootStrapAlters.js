function PresentClosableBoostrapAlert(placeHolder, alertType, alertHeading, alertMessage) {
    if (alertType == "")
        alertType = "info";

    var alertHtml = '<div class="alert alert-' + alertType + ' alert-dismissible fade show" role="alert"><strong>' + alertHeading + '</strong><br>' + alertMessage +
        '<button type="button" class="btn-close" data-bs-dismiss = "alert" aria-label="Close">' + '</button><div >';

    $(placeHolder).html(alertHtml);
}

function CloseAlert(placeHolder) {
    $(placeHolder).html(alertHtml);
}