namespace StudentsHelper.Services.Data.Meetings
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Hangfire;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class MeetingsService : IMeetingsService
    {
        private readonly IRepository<Meeting> meetingsRepository;

        public MeetingsService(IRepository<Meeting> meetingsRepository)
        {
            this.meetingsRepository = meetingsRepository;
        }

        public Task UpdateParticipantActivityAndIncreaseDurationAsync(string role, string meetingId, DateTime utcNow, Expression<Action> scheduleChargeStudent)
        {
            var meeting = this.GetTrackedMeetingById(meetingId);
            int difference;
            if (role == GlobalConstants.StudentRoleName)
            {
                meeting.StudentLastActivity = utcNow;
            }
            else if (role == GlobalConstants.TeacherRoleName)
            {
                // In case teacher refreshes before the time becomes 1 minute
                if (meeting.TeacherLastActivity == null)
                {
                    meeting.TeacherLastActivity = utcNow;
                }

                difference = this.GetDatesDifferenceInSeconds(meeting.TeacherLastActivity.Value, utcNow);
                if (difference < 60)
                {
                    meeting.TeacherLastActivity = utcNow;
                    return this.meetingsRepository.SaveChangesAsync();
                }

                meeting.TeacherLastActivity = utcNow;

                // It doesn't matter either if it happens when called by teacher or student, but it has to happen only for one type.
                if (meeting.StudentLastActivity != null)
                {
                    difference = this.GetDatesDifferenceInSeconds(meeting.StudentLastActivity.Value, meeting.TeacherLastActivity.Value);
                    if (difference <= 60)
                    {
                        // Important !!!
                        if (meeting.DurationInMinutes == 0)
                        {
                            BackgroundJob.Schedule(
                                scheduleChargeStudent,
                                this.GetConsultationEndTimeByMeetingId(meetingId) - utcNow);
                        }

                        meeting.DurationInMinutes++;
                    }
                }
            }

            return this.meetingsRepository.SaveChangesAsync();
        }

        public T GetMeetingData<T>(string meetingId)
        {
            return this.meetingsRepository
                 .AllAsNoTracking()
                 .Where(x => x.Id == meetingId)
                 .To<T>()
                 .Single();
        }

        private Meeting GetTrackedMeetingById(string meetingId)
        {
            return this.meetingsRepository
                .All()
                .Where(x => x.Id == meetingId)
                .Single();
        }

        private int GetDatesDifferenceInSeconds(DateTime x, DateTime y)
        {
            return Math.Abs((int)(x - y).TotalSeconds);
        }

        private DateTime GetConsultationEndTimeByMeetingId(string id)
        {
            return this.meetingsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => x.Consultation.EndTime)
                .Single();
        }
    }
}
