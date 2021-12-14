using Twilio.Jwt.AccessToken;
using System.Collections.Generic;
using System;
using StudentsHelper.Services.Data.VideoChat;

namespace StudentsHelper.Services.VideoChat
{
    public class TwilioVideoChat : ITwilioVideoChat
    {
        private readonly string accountSid;
        private readonly string apiKey;
        private readonly string apiSecret;

        public TwilioVideoChat(string accountSid, string apiKey, string apiSecret)
        {
            this.accountSid = accountSid;
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }

        public AccessTokenModel CreateAccessTocken(string userId)
        {
            string roomName = Guid.NewGuid().ToString();

            // Create an Chat grant for this token
            var grant = new VideoGrant
            {
                Room = roomName
            };

            var grants = new HashSet<IGrant>
            {
                { grant }
            };

            // Create an Access Token generator
            var token = new Token(
                accountSid,
                apiKey,
                apiSecret,
                identity: userId,
                grants: grants);

            return new AccessTokenModel
            {
                Identity = userId,
                RoomName = roomName,
                AccessTocken = token.ToJwt()
            };
        }
    }
}
