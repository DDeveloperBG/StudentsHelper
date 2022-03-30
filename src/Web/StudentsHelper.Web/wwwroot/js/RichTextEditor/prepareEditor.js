document.addEventListener('load', runEngine);

async function runEngine() {
    try {
        await engine();
    }
    catch (error) {
        console.error(error);
    }
}

async function engine() {
    await tinymce.init({ selector: '#description-textarea' });
    $('.tox-statusbar__branding').remove();
}