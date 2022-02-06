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
            int difference;
            if (role == GlobalConstants.StudentRoleName)
            {
                meeting.StudentLastActivity = now;
            }
            else if (role == GlobalConstants.TeacherRoleName)
            {
                // In case teacher refreshes before the time becomes 1 minute
                if (meeting.TeacherLastActivity == null)
                {
                    meeting.TeacherLastActivity = now;
                }

                difference = this.GetDatesDifferenceInSeconds(meeting.TeacherLastActivity.Value, now);
                if (difference < 60)
                {
                    meeting.TeacherLastActivity = now;
                    return this.meetingsRepository.SaveChangesAsync();
                }

                meeting.TeacherLastActivity = now;

                // It doesn't matter either if it is for teacher or student, but it has to happen only once.
                if (meeting.StudentLastActivity != null)
                {
                    difference = this.GetDatesDifferenceInSeconds(meeting.StudentLastActivity.Value, meeting.TeacherLastActivity.Value);
                    if (difference <= 60)
                    {
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
    }
}
