document.addEventListener('DOMContentLoaded', start);

function start() {
    changeInitialTimesToMessages();

    const messagesHolder = document.querySelector('.chat-messages');
    scrollToEnd(messagesHolder);

    let connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/chatsocket")
            .build();

    connection.on("ReceiveMessage", displayMessage.bind(null, messagesHolder));

    const groupId = document.getElementById('group-name').textContent;

    $("#sendButton").click(function () {
        sendMessage();
    });

    document.getElementById('messageInput').addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            sendMessage();
        }
    });

    function sendMessage() {
        let message = $("#messageInput").val();
        connection.invoke("SendMessage", { Text: message, GroupId: groupId })
            .catch(function (err) {
                return console.error(err.toString());
            });
        $("#messageInput").val("");
    }

    connection.start().then(res => {
        connection.invoke("JoinGroup", groupId)
            .catch(err => {
                return console.error(err.toString());
            });
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

function scrollToEnd(messagesHolder) {
    messagesHolder.scrollTo(0, messagesHolder.scrollHeight);
}

function displayMessage(messagesHolder, messageObj) {
    const templateClone = document.getElementById('template').cloneNode(true);
    const userEmail = document.getElementById('user-email').textContent;
    const isSenderCurrentUser = messageObj.senderEmail == userEmail;

    messagesHolder.appendChild(templateClone);

    // Set sender image
    if (!isSenderCurrentUser) {
        const pathStartInd = window.location.href.indexOf(window.location.pathname);
        const serverUrl = window.location.href.substring(0, pathStartInd);
        const pictureUrl
            = `${serverUrl}/Chat/GetSenderProfilePicture?picturePath=${messageObj.senderPicturePath ?? ''}&email=${messageObj.senderEmail}`;

        fetch(pictureUrl)
            .then((response) => response.text())
            .then((html) => {
                templateClone.querySelector('#sender-picture').innerHTML = html;
            })
            .catch((error) => {
                console.warn(error);
            });
    }

    //Set send text
    templateClone.querySelector('#message').textContent = escapeHtml(messageObj.text);

    //Set position
    let position = isSenderCurrentUser ? 'right' : 'left';
    templateClone.classList.add(`chat-message-${position}`);

    //Set current time
    const timeDomObj = templateClone.querySelector('#sending-time');
    timeDomObj.textContent = messageObj.sendTime;
    changeDomObjTimeToLocal(timeDomObj);

    //Set sender name
    templateClone.querySelector('#sender-name').textContent = isSenderCurrentUser ? 'Вие' : messageObj.senderName;

    //Remove hidden class
    templateClone.classList.remove('hidden');
    scrollToEnd(messagesHolder);
}

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function changeInitialTimesToMessages() {
    Array.from(document.getElementsByClassName('send-time'))
        .forEach(changeDomObjTimeToLocal);
}

function changeDomObjTimeToLocal(timeObj) {

    if (timeObj.textContent == '') return;

    const localTimeObj = new moment(moment.utc(timeObj.textContent).toDate());
    let format = 'DD.MM.YYYY г., HH:mm ч.';
    if (moment().isSame(localTimeObj, 'date')) {
        format = 'HH:mm ч.';
    }

    timeObj.textContent = localTimeObj.format(format);
}