namespace StudentsHelper.Services.VideoChat
{
    using System;

    using StudentsHelper.Services.Data.VideoChat;

    public class VideoChat : IVideoChat
    {
        private readonly string apiKey;

        public VideoChat(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public UserConfiguration GetUserDefaultConfiguration(string userName, string meetingId, string host)
        {
            UserConfiguration userConfigs = new UserConfiguration
            {
                Name = userName,
                ApiKey = this.apiKey,
                MeetingId = meetingId,

                //brandingEnabled: true,
                //brandLogoURL: logo url,

                RedirectOnLeave = $"https://{host}/",

                MicEnabled = true,
                WebcamEnabled = true,
                ParticipantCanToggleSelfWebcam = true,
                ParticipantCanToggleSelfMic = true,

                ChatEnabled = true,
                ScreenShareEnabled = true,
                PollEnabled = false,
                WhiteBoardEnabled = true,
                RaiseHandEnabled = true,

                RecordingEnabled = false,
                ParticipantCanToggleRecording = false,

                BrandingEnabled = false,
                PoweredBy = true,

                ParticipantCanLeave = true,

                Permissions = new UserPermissions
                {
                    AskToJoin = false, // Ask joined participants for entry in meeting
                    ToggleParticipantMic = false, // Can toggle other participant's mic
                    ToggleParticipantWebcam = false, // Can toggle other participant's webcam
                    DrawOnWhiteboard = true,
                    ToggleWhiteboard = true,
                },

                JoinScreen = new JoinScreenConf
                {
                    Visible = true, // Show the join screen
                    MeetingUrl = $"/VideoChat/VideoChat?{nameof(meetingId)}={meetingId}", // Meeting joining url
                },

                Pin = new PinConf
                {
                    Allowed = true, // participant can pin any participant in meeting
                    Layout = "SPOTLIGHT", // meeting layout - GRID | SPOTLIGHT | SIDEBAR
                },
            };

            return userConfigs;
        }

        public UserConfiguration GetUserConfigurations(string userName, string meetingId, string host)
        {
            return this.GetUserDefaultConfiguration(userName, meetingId, host);
        }
    }
}
