window.addEventListener('load', () => {
    document.getElementById('navbar-profile-pic').addEventListener('click', showProfilePic);

    function showProfilePic(e) {
        const link = e.currentTarget.querySelector('a');
        link.click();
    }

    const oneMinuteInMiliseconds = 60000;
    setInterval(refreshUserStatus, oneMinuteInMiliseconds);

    // In case user stays on the same page for too long
    function refreshUserStatus() {
        fetch('/?userStatusUpdate=true');
    }
});