window.addEventListener('DOMContentLoaded', runBookConsultationDatetimeManagment);

function runBookConsultationDatetimeManagment() {
    try {
        bookConsultationDatetimeManagment();
    }
    catch (error) {
        console.error(error);
    }
}

function bookConsultationDatetimeManagment() {
    const format = 'yyyy-MM-DDTHH:mm';
    const startTimeInput = document.getElementById('startTimeInput');
    const timeNowFormatted = moment().format(format);
    startTimeInput.value = timeNowFormatted;
    startTimeInput.setAttribute('min', timeNowFormatted);

    const formEl = document.querySelector('form');
    formEl.addEventListener('submit', (e) => {
        if (!isValidCurrentForm()) { e.preventDefault(); return; }
        startTimeInput.removeAttribute('min');
        startTimeInput.value = new moment(startTimeInput.value, format).utc().format(format);
    });

    function isValidCurrentForm() {
        const inputFields = Array.from(formEl.querySelectorAll('[name]'));

        return inputFields.every(x => $(x).valid());
    }
}