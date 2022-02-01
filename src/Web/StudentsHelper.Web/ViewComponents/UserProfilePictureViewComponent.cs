namespace StudentsHelper.Web.ViewComponents
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Web.ViewModels.User;

    public class UserProfilePictureViewComponent : ViewComponent
    {
        private readonly ICloudStorageService cloudStorageService;

        public UserProfilePictureViewComponent(
            ICloudStorageService cloudStorageService)
        {
            this.cloudStorageService = cloudStorageService;
        }

        public IViewComponentResult Invoke(string picturePath, string email = null, bool? isActive = null)
        {
            if (email == null && isActive == null)
            {
                throw new Exception("Either Email or IsActive should have value!");
            }

            if (string.IsNullOrWhiteSpace(picturePath))
            {
                picturePath = GlobalConstants.NoProfilePicturePath;
            }
            else
            {
                picturePath = this.cloudStorageService.GetImageUri(picturePath, 50, 50);
            }

            if (isActive == null)
            {
                GlobalVariables.UsersActivityDictionary.TryGetValue(email, out DateTime lastTimeActive);
                isActive = (DateTime.UtcNow - lastTimeActive).Minutes < 2;
            }

            var viewModel = new UserProfilePictureViewModel
            {
                PicturePath = picturePath,
                IsActive = isActive.Value,
            };

            return this.View(viewModel);
        }
    }
}
