namespace StudentsHelper.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class School : BaseDeletableModel<int>
    {
        public School()
        {
            this.Teachers = new HashSet<Teacher>();
        }

        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(PopulatedArea))]
        public int PopulatedAreaId { get; set; }

        public PopulatedArea PopulatedArea { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
