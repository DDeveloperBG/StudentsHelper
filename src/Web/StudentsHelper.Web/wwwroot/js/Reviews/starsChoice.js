window.addEventListener('DOMContentLoaded', runStarsChoice);

function runStarsChoice() {
    try {
        starsChoice();
    }
    catch (error) {
        console.error(error);
    }
}

function starsChoice() {
    let notSelectedStar = document.getElementById('star-not-selected');
    notSelectedStar.classList.remove('hidden');
    let notSelectedStarSrc = remove(notSelectedStar).src;
    let selectedStarSrc = remove(document.getElementById('star-selected')).src;

    const container = new DocumentFragment();

    for (let i = 1; i <= 5; i++) {
        let newStar = notSelectedStar.cloneNode();
        newStar.setAttribute('value', i);
        newStar.addEventListener('mouseenter', changeStars)
        container.appendChild(newStar);
    }

    document.getElementById('stars').appendChild(container);

    let ratingInputEl = document.getElementById('rating');

    if (ratingInputEl.value != "") {
        let chosen = document.querySelector(`.small-star[value="${ratingInputEl.value}"]`)
        chosen.src = selectedStarSrc;

        let current = chosen.previousElementSibling;
        while (current != null && current.src == notSelectedStarSrc) {
            current.src = selectedStarSrc;
            current = current.previousElementSibling;
        }

        current = chosen.nextElementSibling;
        while (current != null && current.src == selectedStarSrc) {
            current.src = notSelectedStarSrc;
            current = current.nextElementSibling;
        }
    }

    function remove(star) {
        star.remove();

        return star;
    }

    function changeStars(e) {
        e.target.src = selectedStarSrc;

        ratingInputEl.value = e.target.getAttribute("value");

        let current = e.target.previousElementSibling;
        while (current != null && current.src == notSelectedStarSrc) {
            current.src = selectedStarSrc;
            current = current.previousElementSibling;
        }

        current = e.target.nextElementSibling;
        while (current != null && current.src == selectedStarSrc) {
            current.src = notSelectedStarSrc;
            current = current.nextElementSibling;
        }
    }
}