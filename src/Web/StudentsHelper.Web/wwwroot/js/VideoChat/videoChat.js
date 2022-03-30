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
    const urlParams = new URLSearchParams(window.location.search);
    const meetingId = urlParams.get('meetingId');

    let requestUrl = '/VideoChat/UserConfig';

    if (meetingId != null) {
        requestUrl += `?meetingId=${meetingId}`;
    }

    fetch(requestUrl)
        .then((res) => res.json())
        .then((config) => new VideoSDKMeeting().init(config));

    const oneMinuteInMiliseconds = 60000;
    setInterval(updateUserActivity, oneMinuteInMiliseconds);

    function updateUserActivity() {
        const urlParams = new URLSearchParams(window.location.search);
        const meetingId = urlParams.get('meetingId');
        fetch(`/VideoChat/UpdatePartiacipantStatus?meetingId=${meetingId}`)
            .then(response => {
                if (response.status != 200) {
                    window.location.replace("/Home/Index?message=Събранието свърши!");
                }
            })
    }
}