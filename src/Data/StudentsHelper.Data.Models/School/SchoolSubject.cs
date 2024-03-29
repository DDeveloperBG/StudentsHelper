﻿namespace StudentsHelper.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Data.Common.Models;

    public class SchoolSubject : BaseModel<int>
    {
        public SchoolSubject()
        {
            this.Teachers = new HashSet<Teacher>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string IconPath { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
