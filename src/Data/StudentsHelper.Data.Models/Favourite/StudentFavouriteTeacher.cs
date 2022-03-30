namespace StudentsHelper.Data.Models.Favourite
{
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class StudentFavouriteTeacher : BaseModel<int>
    {
        public StudentFavouriteTeacher()
        {
            this.StillIsFavourite = true;
        }

        public bool StillIsFavourite { get; set; }

        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        public Student Student { get; set; }

        [ForeignKey(nameof(Teacher))]
        public string TeacherId { get; set; }

        public Teacher Teacher { get; set; }
    }
}
