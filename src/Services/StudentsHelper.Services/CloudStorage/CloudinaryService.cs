namespace StudentsHelper.Services.CloudStorage
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudStorageService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(string cloudName, string apiKey, string apiSecret)
        {
            Account account = new Account(cloudName, apiKey, apiSecret);

            this.cloudinary = new Cloudinary(account);
            this.cloudinary.Api.Secure = true;
        }

        public Task UploadImageAsync(string folder, string name, Stream file)
        {
            var uploadParams = new ImageUploadParams()
            {
                UseFilenameAsDisplayName = true,
                File = new FileDescription(name, file),
                PublicId = $"{folder}/{name}",
                Overwrite = true,
            };

            return this.cloudinary.UploadAsync(uploadParams);
        }

        public string GetImageUri(string path, int? width = null, int? height = null)
        {
            if (path == null)
            {
                return null;
            }

            var res = this.cloudinary.Api.UrlImgUp;

            if (width != null && height != null)
            {
                res = res.Transform(new Transformation().Width(width).Height(height).Crop("scale").Effect("improve").Quality("auto"));
            }

            return res.BuildUrl(path);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString();
                Stream fileStream = file.OpenReadStream();

                await this.UploadImageAsync(folderName, fileName, fileStream);

                return $"{folderName}/{fileName}";
            }

            throw new Exception("File size was 0!");
        }
    }
}
