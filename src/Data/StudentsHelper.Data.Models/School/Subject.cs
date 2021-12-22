﻿namespace StudentsHelper.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Data.Common.Models;

    public class Subject : BaseDeletableModel<int>
    {
        public Subject()
        {
            this.Teachers = new HashSet<Teacher>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
