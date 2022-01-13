window.addEventListener('load', () => {
    let stars = Array.from(document.getElementsByClassName('small-star'));
    stars.forEach(x => {
        x.addEventListener('click', () => {
            document.getElementById("reviewForm").submit();
        })
    });
});