namespace StudentsHelper.Web.ViewModels.Administration.Teachers
{
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Common;

    public class TeacherExternalDataInputModel
    {
        [Required]
        public string Id { get; set; }

        [Display(Name = "Предмети")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        public int[] SubjectsId { get; set; }
    }
}
