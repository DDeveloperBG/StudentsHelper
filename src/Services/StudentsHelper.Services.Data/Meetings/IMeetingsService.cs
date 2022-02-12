namespace StudentsHelper.Services.Data.Meetings
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IMeetingsService
    {
        Task UpdateParticipantActivityAndIncreaseDurationAsync(string role, string meetingId, DateTime utcNow, Expression<Action> scheduleChargeStudent);

        T GetMeetingData<T>(string meetingId);
    }
}
