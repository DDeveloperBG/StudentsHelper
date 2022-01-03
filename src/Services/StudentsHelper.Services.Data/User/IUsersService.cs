namespace StudentsHelper.Services.Data.User
{
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public interface IUsersService
    {
        ApplicationUser GetUserWithUsername(string username);

        Task RestoreUserAsync(ApplicationUser user);
    }
}
