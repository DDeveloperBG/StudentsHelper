namespace StudentsHelper.Web.ViewModels.Administration.Teachers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Common;
    using StudentsHelper.Services.Data.Subjects;

    public class TeacherExternalData
    {
        public TeacherExternalDataViewData ViewData { get; set; }

        [Required]
        public string Id { get; set; }

        [Display(Name = "Предмети")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        public int[] SubjectsId { get; set; }
    }

    public class TeacherExternalDataViewData
    {
        public string ApplicationUserName { get; set; }

        public IEnumerable<SchoolSubjectPickViewModel> Subjects { get; set; }
    }
}
