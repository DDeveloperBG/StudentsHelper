namespace StudentsHelper.Services.Data.VideoChat
{
    public class UserConfiguration
    {
        public string Name { get; set; }

        public string ApiKey { get; set; }

        public string MeetingId { get; set; }

        public string RedirectOnLeave { get; set; }

        public bool MicEnabled { get; set; }

        public bool WebcamEnabled { get; set; }

        public bool ParticipantCanToggleSelfWebcam { get; set; }

        public bool ParticipantCanToggleSelfMic { get; set; }

        public bool ChatEnabled { get; set; }

        public bool ScreenShareEnabled { get; set; }

        public bool PollEnabled { get; set; }

        public bool WhiteBoardEnabled { get; set; }

        public bool RaiseHandEnabled { get; set; }

        public bool RecordingEnabled { get; set; }

        public bool ParticipantCanToggleRecording { get; set; }

        public bool BrandingEnabled { get; set; }

        public bool PoweredBy { get; set; }

        public bool ParticipantCanLeave { get; set; }

        public UserPermissions Permissions { get; set; }

        public JoinScreenConf JoinScreen { get; set; }

        public PinConf Pin { get; set; }
    }
}
