window.addEventListener('DOMContentLoaded', runTeacherDescriptionWarnBeforeLeave);

function runTeacherDescriptionWarnBeforeLeave() {
    try {
        teacherDescriptionWarnBeforeLeave();
    }
    catch (error) {
        console.error(error);
    }
}

function teacherDescriptionWarnBeforeLeave() {
    let formSubmitting = false;
    let initValue = null;
    document.getElementById('description-form').addEventListener('click', function () { formSubmitting = true; });
    initValue = document.getElementById('description-textarea').value;

    window.addEventListener('beforeunload', warnUserOfNotSavedContent);

    function warnUserOfNotSavedContent(e) {
        if (formSubmitting) {
            return undefined;
        }

        tinyMCE.triggerSave();
        if (document.getElementById('description-textarea').value == initValue) {
            return undefined;
        }

        // The custom message isn't visualized by browsers. Not bug, that's just how they work.
        var confirmationMessage = 'Изглежда, че сте редактирали нещо. '
            + 'Ако напуснете, преди да запазите, промените ви ще бъдат загубени.';

        (e || window.event).returnValue = confirmationMessage; //Gecko + IE
        return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.
    }
}