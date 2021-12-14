using StudentsHelper.Services.Data.VideoChat;

namespace StudentsHelper.Services.VideoChat
{
    public interface ITwilioVideoChat
    {
        public AccessTokenModel CreateAccessTocken(string userId);
    }
}
