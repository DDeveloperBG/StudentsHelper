namespace StudentsHelper.Services.Auth
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.CloudStorage;

    public class TeacherRegisterer : ITeacherRegisterer
    {
        private const string QualificationDocumentFolder = "QualificationDocuments";
        private readonly IDeletableEntityRepository<Teacher> teachersRepository;
        private readonly IDeletableEntityRepository<School> schoolsRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ICloudStorageService cloudStorageService;

        public TeacherRegisterer(
            IDeletableEntityRepository<Teacher> teachersRepository,
            IDeletableEntityRepository<School> schoolsRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ICloudStorageService cloudStorageService)
        {
            this.teachersRepository = teachersRepository;
            this.schoolsRepository = schoolsRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.cloudStorageService = cloudStorageService;
        }

        public async Task RegisterAsync(TeacherInputModel inputModel, ApplicationUser user)
        {
            if (inputModel == null)
            {
                throw new ArgumentException("Настъпи грешка!");
            }

            if (inputModel.SchoolId == 0)
            {
                throw new ArgumentException("Задайте училището в което преподавате");
            }

            if (!this.schoolsRepository.AllAsNoTracking().Any(x => x.Id == inputModel.SchoolId))
            {
                throw new ArgumentException("Невалидно училище!");
            }

            string qualificationDocumentPath = await this.cloudStorageService.SaveFileAsync(inputModel.QualificationDocument, QualificationDocumentFolder);

            Teacher teacher = new Teacher
            {
                ApplicationUserId = user.Id,
                SchoolId = inputModel.SchoolId,
                QualificationDocumentPath = qualificationDocumentPath,
            };

            var role = await this.roleManager.FindByNameAsync(GlobalConstants.TeacherRoleName);
            await this.userManager.AddToRoleAsync(user, role.Name);

            await this.teachersRepository.AddAsync(teacher);
            await this.teachersRepository.SaveChangesAsync();
        }
    }
}
