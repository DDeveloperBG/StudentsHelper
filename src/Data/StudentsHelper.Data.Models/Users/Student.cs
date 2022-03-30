namespace StudentsHelper.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;
    using StudentsHelper.Data.Models.Favourite;

    public class Student : BaseDeletableModel<string>
    {
        public Student()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Reviews = new HashSet<Review>();
            this.Transactions = new HashSet<StudentTransaction>();
            this.StudentFavouriteTeachers = new HashSet<StudentFavouriteTeacher>();
        }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<StudentTransaction> Transactions { get; set; }

        public ICollection<StudentFavouriteTeacher> StudentFavouriteTeachers { get; set; }
    }
}
