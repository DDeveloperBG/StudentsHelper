window.addEventListener('DOMContentLoaded', runHomePageCode);

function runHomePageCode() {
    try {
        homePageCode();
    }
    catch (error) {
        console.error(error);
    }
}

function homePageCode() {
    var schoolSubjectElements = Array.from(document.getElementsByClassName('schoolSubjectItem'));

    schoolSubjectElements.forEach(x => x.addEventListener('click', searchSchoolSubject));

    function searchSchoolSubject(e) {
        let subjectId = e.currentTarget.id;
        window.location.href = `/Teachers/All?subjectId=${subjectId}`;
    }
}