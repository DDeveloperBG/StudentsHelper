using JWT.Algorithms;
using JWT.Builder;

using System;
using StudentsHelper.Services.Data.VideoChat;

namespace StudentsHelper.Services.VideoChat
{
    public class VideoChat : IVideoChat
    {
        private readonly string apiKey;
        private readonly string apiSecret;

        public VideoChat(string apiKey, string apiSecret)
        {
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }

        public AccessTokenModel CreateAccessTocken(string userIdentity, string userName)
        {
            string roomName = Guid.NewGuid().ToString();

            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(apiSecret)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                      .AddClaim("apikey", apiKey)
                      .AddClaim("permissions", new string[2] { "allow_join", "allow_mod" })
                      .Encode();

            return new AccessTokenModel() {
                Identity = userIdentity,
                UserName = userName,
                RoomName = roomName,
                AccessTocken = token 
            };
        }
    }
}
