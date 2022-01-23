namespace StudentsHelper.Web.ViewModels.Balance
{
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Common;

    public class TeacherWageInputModel
    {
        [Display(Name = "Възнаграждение/час - лв")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [Range(
                 ValidationConstants.MinTeacherWage,
                 ValidationConstants.MaxTeacherWage,
                 ErrorMessage = "Стойноста трябва да е поне {1} и не повече от {2} лв.")]
        public decimal TeacherWage { get; set; }
    }
}
