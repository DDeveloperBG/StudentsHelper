namespace StudentsHelper.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    // Община
    public class Township : BaseDeletableModel<int>
    {
        public Township()
        {
            this.PopulatedAreas = new HashSet<PopulatedArea>();
        }

        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(Region))]
        public int RegionId { get; set; }

        public Region Region { get; set; }

        public ICollection<PopulatedArea> PopulatedAreas { get; set; }
    }
}
