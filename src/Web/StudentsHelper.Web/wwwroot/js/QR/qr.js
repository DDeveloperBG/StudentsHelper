window.addEventListener('DOMContentLoaded', runDisplayQrCode);

function runDisplayQrCode() {
    try {
        displayQrCode();
    }
    catch (error) {
        console.error(error);
    }
}

function displayQrCode() {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 150,
            height: 150
        });
}