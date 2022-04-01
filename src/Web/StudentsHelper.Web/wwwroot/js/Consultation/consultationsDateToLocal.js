window.addEventListener('DOMContentLoaded', consultationsDateToLocal);

function runConsultationsDateToLocal() {
    try {
        consultationsDateToLocal();
    }
    catch (error) {
        console.error(error);
    }
}

function consultationsDateToLocal() {
    Array.from(document.getElementsByClassName('startTime')).forEach(pElement => {
        pElement.textContent
            = new moment(moment.utc(pElement.textContent).toDate()).format('HH:mm - L');
    });
}