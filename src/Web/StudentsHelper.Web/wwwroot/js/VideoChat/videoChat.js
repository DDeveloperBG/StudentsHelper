window.addEventListener('load', () => {
    const urlParams = new URLSearchParams(window.location.search);
    const meetingId = urlParams.get('meetingId');

    let requestUrl = '/VideoChat/UserConfig';

    if (meetingId != null) {
        requestUrl += `?meetingId=${meetingId}`;
    }

    fetch(requestUrl)
        .then((res) => res.json())
        .then((config) => new VideoSDKMeeting().init(config));
});