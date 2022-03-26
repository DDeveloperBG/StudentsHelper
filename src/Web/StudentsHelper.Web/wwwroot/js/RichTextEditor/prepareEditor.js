window.addEventListener('load', async function () {
    await tinymce.init({ selector: '#description-textarea' });
    $('.tox-statusbar__branding').remove();
});