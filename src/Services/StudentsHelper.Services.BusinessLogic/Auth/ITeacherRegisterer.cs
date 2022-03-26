namespace StudentsHelper.Services.BusinessLogic.Auth
{
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Web.ViewModels.Teachers;

    public interface ITeacherRegisterer
    {
        public Task RegisterAsync(TeacherInputModel inputModel, ApplicationUser user, bool isInProduction);
    }
}
