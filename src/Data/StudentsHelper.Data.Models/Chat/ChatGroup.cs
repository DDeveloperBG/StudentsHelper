namespace StudentsHelper.Data.Models
{
    using System;
    using System.Collections.Generic;

    using StudentsHelper.Data.Common.Models;

    public class ChatGroup : BaseDeletableModel<string>
    {
        public ChatGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Messages = new HashSet<Message>();
            this.Users = new HashSet<ChatGroupUsers>();
        }

        public DateTime LastMessageTime { get; set; }

        public ICollection<ChatGroupUsers> Users { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
