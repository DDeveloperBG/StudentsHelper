namespace StudentsHelper.Services.Data.Contact
{
    using System.Threading.Tasks;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models.Contact;
    using StudentsHelper.Web.ViewModels.Contact;

    public class ContactService : IContactService
    {
        private readonly IRepository<ContactFormEntry> contactsRepository;

        public ContactService(IRepository<ContactFormEntry> contactsRepository)
        {
            this.contactsRepository = contactsRepository;
        }

        public async Task SaveContactFormDataAsync(ContactInputModel model, string ip)
        {
            var contactFormEntry = new ContactFormEntry
            {
                Name = model.Name,
                Email = model.Email,
                Title = model.Title,
                Content = model.Content,
                Ip = ip,
            };

            await this.contactsRepository.AddAsync(contactFormEntry);
            await this.contactsRepository.SaveChangesAsync();
        }
    }
}
