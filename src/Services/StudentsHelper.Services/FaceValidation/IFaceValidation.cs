namespace StudentsHelper.Services.FaceValidation
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IFaceValidation
    {
        Task<bool> IsFaceValidAsync(Stream imageStream);
    }
}
