window.addEventListener('DOMContentLoaded', runActivateAllPopovers);

function runActivateAllPopovers() {
    try {
        activateAllPopovers();
    }
    catch (error) {
        console.error(error);
    }
}

function activateAllPopovers() {
    $('[data-toggle="popover"]').popover();
}