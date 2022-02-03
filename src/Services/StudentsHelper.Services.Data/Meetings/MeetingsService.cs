namespace StudentsHelper.Services.Data.Meetings
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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

        public Task UpdateParticipantActivityAndIncreaseDurationAsync(string role, string meetingId, DateTime now)
        {
            var meeting = this.GetTrackedMeetingById(meetingId);

            switch (role)
            {
                case GlobalConstants.StudentRoleName:
                    meeting.StudentLastActivity = now;
                    break;
                case GlobalConstants.TeacherRoleName:
                    meeting.TeacherLastActivity = now;

                    // It doesn't matter either if it is for teacher or student, but it has to happen only once.
                    if (meeting.StudentLastActivity != null && meeting.TeacherLastActivity != null)
                    {
                        var difference = Math.Abs((meeting.StudentLastActivity.Value - meeting.TeacherLastActivity.Value).Seconds);
                        if (difference <= 60)
                        {
                            meeting.DurationInMinutes++;
                        }
                    }

                    break;
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
    }
}
