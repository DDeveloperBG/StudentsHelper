document.addEventListener('load', runEngine);

function runEngine() {
    try {
        engine();
    }
    catch (error) {
        console.error(error);
    }
}

function engine() {
    Array.from(document.getElementsByClassName('startTime')).forEach(pElement => {
        pElement.textContent
            = new moment(moment.utc(pElement.textContent).toDate()).format('HH:mm - L');
    });
}