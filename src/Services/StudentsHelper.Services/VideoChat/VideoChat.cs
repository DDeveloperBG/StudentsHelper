namespace StudentsHelper.Services.VideoChat
{
    using StudentsHelper.Services.Data.VideoChat;

    public class VideoChat : IVideoChat
    {
        private readonly string apiKey;
        private readonly string host;

        public VideoChat(string apiKey, string host)
        {
            this.apiKey = apiKey;
            this.host = host;
        }

        public UserConfiguration GetUserDefaultConfiguration(string userName, string meetingId)
        {
            UserConfiguration userConfigs = new UserConfiguration
            {
                Name = userName,
                ApiKey = this.apiKey,
                MeetingId = meetingId,

                RedirectOnLeave = this.host,

                MicEnabled = true,
                WebcamEnabled = true,
                ParticipantCanToggleSelfWebcam = true,
                ParticipantCanToggleSelfMic = true,

                ChatEnabled = true,
                ScreenShareEnabled = true,
                PollEnabled = false,
                WhiteboardEnabled = true,
                RaiseHandEnabled = true,

                RecordingEnabled = false,
                ParticipantCanToggleRecording = false,

                BrandingEnabled = false,
                PoweredBy = false,

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
                    Visible = false, // Show the join screen
                    MeetingUrl = $"/VideoChat/VideoChat?{nameof(meetingId)}={meetingId}", // Meeting joining url
                },

                Pin = new PinConf
                {
                    Allowed = true, // participant can pin any participant in meeting
                    Layout = "SIDEBAR", // meeting layout - GRID | SPOTLIGHT | SIDEBAR
                },
            };

            return userConfigs;
        }

        public UserConfiguration GetUserConfigurations(string userName, string meetingId)
        {
            return this.GetUserDefaultConfiguration(userName, meetingId);
        }
    }
}
