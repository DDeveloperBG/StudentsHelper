namespace StudentsHelper.Services.VideoChat
{
    using System;

    using StudentsHelper.Web.ViewModels.VideoChat;

    public class VideoChat : IVideoChat
    {
        private readonly string apiKey;

        public VideoChat(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public UserConfiguration GetUserDefaultConfiguration(string userName, string meetingId)
        {
            UserConfiguration userConfigs = new UserConfiguration
            {
                Name = userName,
                ApiKey = this.apiKey,
                MeetingId = meetingId,

                //brandingEnabled: true,
                //brandLogoURL: logo url,

                RedirectOnLeave = "https://localhost:44319/",

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
                    Visible = true, // Show the join screen ?
                    MeetingUrl = $"/VideoChat/VideoChat?{nameof(meetingId)}={meetingId}", // Meeting joining url
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
