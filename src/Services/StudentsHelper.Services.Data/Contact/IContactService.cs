namespace StudentsHelper.Services.Data.Contact
{
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Contact;

    public interface IContactService
    {
        Task SaveContactFormData(ContactInputModel model, string ip);
    }
}
