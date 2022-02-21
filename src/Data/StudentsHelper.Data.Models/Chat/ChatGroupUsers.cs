namespace StudentsHelper.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class ChatGroupUsers : BaseModel<int>
    {
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [ForeignKey(nameof(ChatGroup))]
        public string ChatGroupId { get; set; }

        public ChatGroup ChatGroup { get; set; }
    }
}
