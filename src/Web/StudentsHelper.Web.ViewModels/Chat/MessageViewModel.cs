using System;

namespace StudentsHelper.Web.ViewModels.Chat
{
    public class MessageViewModel
    {
        public string SenderPicturePath { get; set; }

        public string SenderEmail { get; set; }

        public string SenderName { get; set; }

        public DateTime SendTime { get; set; }

        public string Text { get; set; }
    }
}
