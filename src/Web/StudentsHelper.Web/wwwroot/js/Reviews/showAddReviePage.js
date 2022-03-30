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
    let stars = Array.from(document.getElementsByClassName('small-star'));
    stars.forEach(x => {
        x.addEventListener('click', () => {
            document.getElementById("reviewForm").submit();
        })
    });
}