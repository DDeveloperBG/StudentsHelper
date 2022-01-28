window.addEventListener('load', () => {
    const format = 'yyyy-MM-DDTHH:mm';
    const startTimeInput = document.getElementById('startTimeInput');
    const timeNowFormatted = moment().format(format);
    startTimeInput.value = timeNowFormatted;
    startTimeInput.setAttribute('min', timeNowFormatted);

    document.querySelector('form').addEventListener('submit', () => {
        startTimeInput.value = new moment(startTimeInput.value, format).utc().format(format);
    });
});