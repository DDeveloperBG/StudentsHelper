window.addEventListener('DOMContentLoaded', runShowAddReviePage);

function runShowAddReviePage() {
    try {
        showAddReviePage();
    }
    catch (error) {
        console.error(error);
    }
}

function showAddReviePage() {
    let stars = Array.from(document.getElementsByClassName('small-star'));
    stars.forEach(x => {
        x.addEventListener('click', () => {
            document.getElementById("reviewForm").submit();
        })
    });
}