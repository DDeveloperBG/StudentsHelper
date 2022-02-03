namespace StudentsHelper.Services.Data.Meetings
{
    using System;
    using System.Threading.Tasks;

    public interface IMeetingsService
    {
        Task UpdateParticipantActivityAndIncreaseDurationAsync(string role, string meetingId, DateTime now);

        T GetMeetingData<T>(string meetingId);
    }
}
