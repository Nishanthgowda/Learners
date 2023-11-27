$(function () {

    function WiredUpDatepicker() {
        const currentDate = new Date();
        const numberOfMonths = 2;
        $('#DateTimeItemReleased').datepicker(
            {
                dateFormat: "yy-MM-dd",
                minDate: currentDate,
                maxDate: AddSubtractMonth(currentDate, numberOfMonths)
            }

        );
    }
    WiredUpDatepicker();
});
