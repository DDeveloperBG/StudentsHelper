window.addEventListener('load', () => {
    document.getElementById('navbar-profile-pic').addEventListener('click', showProfilePic);

    function showProfilePic(e) {
        const link = e.currentTarget.querySelector('a');
        link.click();
    }
});