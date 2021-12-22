namespace StudentsHelper.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    // Населено място
    public class PopulatedArea : BaseDeletableModel<int>
    {
        public PopulatedArea()
        {
            this.Schools = new HashSet<School>();
        }

        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(Township))]
        public int TownshipId { get; set; }

        public Township Township { get; set; }

        public ICollection<School> Schools { get; set; }
    }
}
