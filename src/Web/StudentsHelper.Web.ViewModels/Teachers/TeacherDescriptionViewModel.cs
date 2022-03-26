namespace StudentsHelper.Web.ViewModels.Teachers
{
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeacherDescriptionViewModel : IMapFrom<Teacher>
    {
        [MaxLength(
            1000,
            ErrorMessage = "Дължината на описанието трябва да е под 1000 символа.")]
        public string Description { get; set; }
    }
}
