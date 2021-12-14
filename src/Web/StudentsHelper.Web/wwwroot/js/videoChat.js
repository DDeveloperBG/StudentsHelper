window.addEventListener("load", () => {
    const participants = document.getElementById("participants");
    const usernameSpan = document.getElementById("username");
    const roomSpan = document.getElementById("room");

    fetch("/VideoChat/GetRoomAuthTocken", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        })
        .then((res) => res.json())
        .then(({ accessTocken, roomName, identity }) => {
            usernameSpan.textContent = identity;
            roomSpan.textContent = roomName;
            startVideoChat(roomName, accessTocken);
        });

    function startVideoChat(room, token) {
        // Start video chat and listen to participant connected events
        Twilio.Video.connect(token, {
            room: room,
            audio: false,
            video: true,
        }).then((room) => {
            // Once we're connected to the room, add the local participant to the page
            participantConnected(room.localParticipant);
            // Add any existing participants to the page.
            room.participants.forEach(participantConnected);
            // Listen for other participants to join and add them to the page when they
            // do.
            room.on("participantConnected", participantConnected);
            // Listen for participants to leave the room and remove them from the page
            room.on("participantDisconnected", participantDisconnected);
            // Eject the participant from the room if they reload or leave the page
            window.addEventListener("beforeunload", tidyUp(room));
            window.addEventListener("pagehide", tidyUp(room));
        });
    }

    function participantConnected(participant) {
        // Create new <div> for participant and add it to the page
        const el = document.createElement("div");
        el.setAttribute("id", participant.identity);
        participants.appendChild(el);
        // Find all the participant's existing tracks and publish them to our page
        participant.tracks.forEach((trackPublication) => {
            trackPublished(trackPublication, participant);
        });
        // Listen for the participant publishing new tracks
        participant.on("trackPublished", trackPublished);
    }

    function trackPublished(trackPublication, participant) {
        // Get the participant's <div> we created earlier
        const el = document.getElementById(participant.identity);
        // Find out if the track has been subscribed to and add it to the page or
        // listen for the subscription, then add it to the page.

        // First create a function that adds the track to the page
        const trackSubscribed = (track) => {
            // track.attach() creates the media elements <video> and <audio> to
            // to display the track on the page.
            el.appendChild(track.attach());
        };
        // If the track is already subscribed, add it immediately to the page
        if (trackPublication.track) {
            trackSubscribed(trackPublication.track);
        }
        // Otherwise listen for the track to be subscribed to, then add it to the
        // page
        trackPublication.on("subscribed", trackSubscribed);
    }

    function participantDisconnected(participant) {
        participant.removeAllListeners();
        const el = document.getElementById(participant.identity);
        el.remove();
    }

    function trackUnpublished(trackPublication) {
        trackPublication.track.detach().forEach(function(mediaElement) {
            mediaElement.remove();
        });
    }

    function tidyUp(room) {
        return function(event) {
            if (event.persisted) {
                return;
            }
            if (room) {
                room.disconnect();
                room = null;
            }
        };
    }
});