window.addEventListener('load', engine);

function engine() {
    var schoolSubjectElements = Array.from(document.getElementsByClassName('schoolSubjectItem'));

    schoolSubjectElements.forEach(x => x.addEventListener('click', searchSchoolSubject));
}

function searchSchoolSubject(e) {
    let subjectId = e.currentTarget.id;
    window.location.href = `/Teachers/All?subjectId=${subjectId}`;
}