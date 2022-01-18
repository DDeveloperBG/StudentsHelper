namespace StudentsHelper.Services.Auth
{
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public interface ITeacherRegisterer
    {
        public Task RegisterAsync(TeacherInputModel inputModel, ApplicationUser user);
    }
}
