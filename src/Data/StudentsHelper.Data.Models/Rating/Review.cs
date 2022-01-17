namespace StudentsHelper.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class Review : BaseDeletableModel<int>
    {
        public byte Rating { get; set; }

        public string Comment { get; set; }

        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        public Student Student { get; set; }

        [ForeignKey(nameof(Teacher))]
        public string TeacherId { get; set; }

        public Teacher Teacher { get; set; }
    }
}
