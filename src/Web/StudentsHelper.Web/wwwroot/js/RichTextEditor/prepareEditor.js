window.addEventListener('DOMContentLoaded', runPrepareEditor);

async function runPrepareEditor() {
    try {
        await prepareEditor();
    }
    catch (error) {
        console.error(error);
    }
}

async function prepareEditor() {
    await tinymce.init({ selector: '#description-textarea' });
    $('.tox-statusbar__branding').remove();
}