namespace StudentsHelper.Services.VideoChat
{
    using StudentsHelper.Services.Data.VideoChat;

    public interface IVideoChat
    {
        public UserConfiguration GetUserDefaultConfiguration(string userName, string meetingId, string host);

        public UserConfiguration GetUserConfigurations(string userName, string meetingId, string host);
    }
}
