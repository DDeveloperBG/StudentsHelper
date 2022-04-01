window.addEventListener('DOMContentLoaded', runShowNextReviews);

function runShowNextReviews() {
    try {
        showNextReviews();
    }
    catch (error) {
        console.error(error);
    }
}

function showNextReviews() {
    const pagingSize = 4;
    const allRatingsCountEl = document.getElementById('allRatingsCount');
    if (allRatingsCountEl == null) {
        return;
    }
    const allRatingsCount = Number(allRatingsCountEl.textContent);
    document.getElementById('to-start').addEventListener('click', toStart);
    document.getElementById('previous').addEventListener('click', previous);
    document.getElementById('next').addEventListener('click', next);
    document.getElementById('to-end').addEventListener('click', toEnd);

    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const teacherId = urlParams.get('teacherId');

    function toStart() {
        const currentNumber = Number(document.getElementById('currentNumber').textContent);
        if(currentNumber == 1) return;

        $('#reviews-container').load(`/Reviews/GetNextReviews?teacherId=${teacherId}&currentNumber=${0}`, null, prepareNavigation);
    }

    function previous() {
        const currentNumber = Number(document.getElementById('currentNumber').textContent);
        const leftReviews = currentNumber - pagingSize;

        if(leftReviews > 0){
            $('#reviews-container').load(`/Reviews/GetNextReviews?teacherId=${teacherId}&currentNumber=${leftReviews - pagingSize}`, null, prepareNavigation);
        }
    }

    function next() {        
        const currentNumber = Number(document.getElementById('currentNumber').textContent);

        if(allRatingsCount - currentNumber - pagingSize + 1 > 0){
            $('#reviews-container').load(`/Reviews/GetNextReviews?teacherId=${teacherId}&currentNumber=${currentNumber}`, null, prepareNavigation);
        }
    }

    function toEnd() {
        if(allRatingsCount / pagingSize < 1) return;

        const currentNumber = Number(document.getElementById('currentNumber').textContent);
        if(allRatingsCount == pagingSize + currentNumber - 1) return;

        const end = allRatingsCount - pagingSize * 2 + 1;

        $('#reviews-container').load(`/Reviews/GetNextReviews?teacherId=${teacherId}&currentNumber=${end}`, null, prepareNavigation);
    }
}