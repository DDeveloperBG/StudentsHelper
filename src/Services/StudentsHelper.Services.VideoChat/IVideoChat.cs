using StudentsHelper.Services.Data.VideoChat;

namespace StudentsHelper.Services.VideoChat
{
    public interface IVideoChat
    {
        public AccessTokenModel CreateAccessTocken(string userIdentity, string userName);
    }
}
