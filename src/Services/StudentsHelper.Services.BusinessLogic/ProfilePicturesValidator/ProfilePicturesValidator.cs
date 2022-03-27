namespace StudentsHelper.Services.BusinessLogic.ProfilePicturesValidator
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using StudentsHelper.Data;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.FaceValidation;

    public class ProfilePicturesValidator : IProfilePicturesValidator
    {
        private readonly IFaceValidation faceValidation;
        private readonly ICloudStorageService cloudStorageService;
        private readonly ApplicationDbContext applicationDbContext;

        public ProfilePicturesValidator(
            IFaceValidation faceValidation,
            ApplicationDbContext applicationDbContext,
            ICloudStorageService cloudStorageService)
        {
            this.faceValidation = faceValidation;
            this.applicationDbContext = applicationDbContext;
            this.cloudStorageService = cloudStorageService;
        }

        public async Task ValidateProfilePicturesAsync()
        {
            var usersWithNotValidatedProfilePictures = this.applicationDbContext
                .Users
                .Where(x => !x.IsPictureValidated)
                .ToList();

            foreach (var user in usersWithNotValidatedProfilePictures)
            {
                if (user.PicturePath != null)
                {
                    var pictureUrl = this.cloudStorageService.GetImageUri(user.PicturePath);
                    HttpClient httpClient = new HttpClient();
                    var responce = await httpClient.GetAsync(pictureUrl);

                    var profilePictureStream = await responce.Content.ReadAsStreamAsync();
                    bool profilePictureIsValid = await this.faceValidation
                        .IsFaceValidAsync(profilePictureStream);

                    if (!profilePictureIsValid)
                    {
                        user.PicturePath = null;
                    }
                }

                user.IsPictureValidated = true;
            }

            await this.applicationDbContext.SaveChangesAsync();
        }
    }
}
