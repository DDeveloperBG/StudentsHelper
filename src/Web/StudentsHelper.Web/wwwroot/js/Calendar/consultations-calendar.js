window.addEventListener('DOMContentLoaded', runDisplayConsultationsCalendar);

function runDisplayConsultationsCalendar() {
    try {
        displayConsultationsCalendar();
    }
    catch (error) {
        console.error(error);
    }
}

function displayConsultationsCalendar() {
    $.ajax({
        url: '/Consultations/GetCalendarConsultations',
        type: "GET",
        dataType: "JSON",

        success: function (result) {
            let events = [];

            $.each(result, function (i, data) {
                events.push(
                    {
                        id: data.id,
                        title: `Консултация`,
                        description: data.description,
                        start: new moment(moment.utc(data.startTime).toDate()).format('YYYY-MM-DDTHH:mm'),
                        end: new moment(moment.utc(data.endTime).toDate()).format('YYYY-MM-DDTHH:mm'),
                        backgroundColor: 'rgb(51, 182, 121)',
                        borderColor: 'rgb(51, 182, 121)',
                    });
            });

            displayCalendar(events);
        }
    });

    function displayCalendar(events) {
        let calendarEl = document.getElementById('calendar');

        let calendar = new FullCalendar.Calendar(calendarEl, {
            locale: 'bg',
            initialView: 'dayGridMonth',
            editable: true,
            eventDurationEditable: false,
            contentHeight: "auto",
            dayMaxEvents: true,
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'timeGridDay,timeGridWeek,dayGridMonth',
            },
            eventMouseEnter: function (info) {
                let jqueryEl = $(info.el);

                if (!jqueryEl.hasClass('hasTooltip')) {
                    jqueryEl.tooltip({ title: info.event.extendedProps.description, placement: "top" });
                    jqueryEl.addClass('hasTooltip');
                }

                jqueryEl.triggerHandler('mouseover');
            },
            eventDrop: function (info) {
                if (!confirm("Сигурни ли сте относно тази промяна?")) {
                    info.revert();
                    return;
                }

                let start = new moment(info.event.start).utc().format('yyyy-MM-DDTHH:mm');

                $.ajax({
                    type: "POST",
                    url: '/Consultations/ChangeConsultationStartTime',
                    data: JSON.stringify({ consultationId: info.event.id, startTime: start }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: () => {
                        info.revert();
                        alert('Невалидни данни!');
                    },
                });
            },
            events,
        });

        calendar.render();
    }
}