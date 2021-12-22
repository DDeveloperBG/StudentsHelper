namespace StudentsHelper.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Data.Common.Models;

    // Област
    public class Region : BaseDeletableModel<int>
    {
        public Region()
        {
            this.Townships = new HashSet<Township>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<Township> Townships { get; set; }
    }
}
