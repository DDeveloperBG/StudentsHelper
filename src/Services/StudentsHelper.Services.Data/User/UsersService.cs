namespace StudentsHelper.Services.Data.User
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public ApplicationUser GetUserWithUsername(string username)
        {
            return this.usersRepository.AllWithDeleted().Where(x => x.UserName == username).SingleOrDefault();
        }

        public async Task RestoreUserAsync(ApplicationUser user)
        {
            user.IsDeleted = false;
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
