namespace StudentsHelper.Services.CloudStorage
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudStorageService
    {
        Task UploadImageAsync(string folder, string name, Stream file);

        string GetImageUri(string path, int? width = null, int? height = null);

        Task<string> SaveFileAsync(IFormFile qualificationDoc, string folderName);
    }
}
