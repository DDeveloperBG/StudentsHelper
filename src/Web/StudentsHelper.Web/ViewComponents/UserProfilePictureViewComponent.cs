namespace StudentsHelper.Web.ViewComponents
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Web.ViewModels.User;

    public class UserProfilePictureViewComponent : ViewComponent
    {
        private readonly ICloudStorageService cloudStorageService;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITeachersService teachersService;

        public UserProfilePictureViewComponent(
            ICloudStorageService cloudStorageService,
            IDateTimeProvider dateTimeProvider,
            UserManager<ApplicationUser> userManager,
            ITeachersService teachersService)
        {
            this.cloudStorageService = cloudStorageService;
            this.dateTimeProvider = dateTimeProvider;
            this.userManager = userManager;
            this.teachersService = teachersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string picturePath,
            string email,
            bool? isActive = null)
        {
            if (string.IsNullOrWhiteSpace(picturePath))
            {
                picturePath = GlobalConstants.NoProfilePicturePath;
            }
            else
            {
                picturePath = this.cloudStorageService.GetImageUri(picturePath, 50, 50);
            }

            if (isActive == null && !string.IsNullOrWhiteSpace(email))
            {
                GlobalVariables.UsersActivityDictionary.TryGetValue(email, out DateTime lastTimeActive);
                isActive = (this.dateTimeProvider.GetUtcNow() - lastTimeActive).Minutes < 2;
            }

            var user = await this.userManager.FindByEmailAsync(email);
            bool isInTeacherRole = await this.userManager.IsInRoleAsync(user, GlobalConstants.TeacherRoleName);
            string teacherId = null;
            if (isInTeacherRole)
            {
                teacherId = this.teachersService.GetId(user.Id);
            }

            var viewModel = new UserProfilePictureViewModel
            {
                PicturePath = picturePath,
                IsActive = isActive,
                IsInTeacherRole = isInTeacherRole,
                TeacherId = teacherId,
            };

            return this.View(viewModel);
        }
    }
}
