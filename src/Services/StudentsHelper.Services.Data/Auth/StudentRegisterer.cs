namespace StudentsHelper.Services.Auth
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class StudentRegisterer : IStudentRegisterer
    {
        private readonly IDeletableEntityRepository<Student> studentsRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public StudentRegisterer(
            IDeletableEntityRepository<Student> studentsRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.studentsRepository = studentsRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task RegisterAsync(ApplicationUser user)
        {
            Student student = new Student
            {
                ApplicationUserId = user.Id,
            };

            var role = await this.roleManager.FindByNameAsync(GlobalConstants.StudentRoleName);
            await this.userManager.AddToRoleAsync(user, role.Name);

            await this.studentsRepository.AddAsync(student);
            await this.studentsRepository.SaveChangesAsync();
        }
    }
}
