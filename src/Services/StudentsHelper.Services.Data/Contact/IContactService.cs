namespace StudentsHelper.Services.Data.Contact
{
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Contact;

    public interface IContactService
    {
        Task SaveContactFormDataAsync(ContactInputModel model, string ip);
    }
}
