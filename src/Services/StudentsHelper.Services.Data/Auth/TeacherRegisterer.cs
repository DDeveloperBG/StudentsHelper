namespace StudentsHelper.Services.Auth
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class TeacherRegisterer : ITeacherRegisterer
    {
        private readonly IDeletableEntityRepository<Teacher> teachersRepository;
        private readonly IDeletableEntityRepository<School> schoolsRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public TeacherRegisterer(
            IDeletableEntityRepository<Teacher> teachersRepository,
            IDeletableEntityRepository<School> schoolsRepository,
            IHostingEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.teachersRepository = teachersRepository;
            this.schoolsRepository = schoolsRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
            this.roleManager = roleManager;
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

            string qualificationDocumentPath = await this.SaveQualificationDocument(inputModel.QualificationDocument);

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

        private async Task<string> SaveQualificationDocument(IFormFile qualificationDoc)
        {
            string folderPath = Path.Combine(this.hostingEnvironment.ContentRootPath, "QualificationDocuments");
            Directory.CreateDirectory(folderPath);

            if (qualificationDoc.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(qualificationDoc.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await qualificationDoc.CopyToAsync(fileStream);
                }

                return filePath;
            }

            throw new Exception("File size was 0!");
        }
    }
}
