function AddSubtractMonth(date, numberOfMonth) {
    var month = date.getMonth();
    var milliSecond = new Date(date).setMonth(month + numberOfMonth);
    return new Date(milliSecond);
}