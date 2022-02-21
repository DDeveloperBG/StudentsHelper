namespace StudentsHelper.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class Message : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        [Required]
        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }
    }
}
