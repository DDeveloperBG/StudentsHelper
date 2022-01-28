window.addEventListener('load', () => {
    Array.from(document.getElementsByClassName('startTime')).forEach(pElement => {
        pElement.textContent
            = new moment(moment.utc(pElement.textContent).toDate()).format('HH:mm - L');
    });
});