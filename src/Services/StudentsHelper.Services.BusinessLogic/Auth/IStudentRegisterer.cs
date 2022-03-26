namespace StudentsHelper.Services.BusinessLogic.Auth
{
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public interface IStudentRegisterer
    {
        public Task RegisterAsync(ApplicationUser user);
    }
}
