namespace RPM.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models.Contact;
    using StudentsHelper.Services.Messaging;
    using StudentsHelper.Web.Controllers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Contact;

    public class ContactController : BaseController
    {
        private readonly IEmailSender emailSender;
        private readonly IRepository<ContactFormEntry> contactsRepository;

        public ContactController(
            IEmailSender emailSender,
            IRepository<ContactFormEntry> contactsRepository)
        {
            this.emailSender = emailSender;
            this.contactsRepository = contactsRepository;
        }

        public IActionResult Index()
        {
            var viewModel = new ContactInputModel();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = this.RedirectToAction(nameof(HomeController.IndexAsync), "Home", new { area = string.Empty });

                try
                {
                    var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
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

                    await this.emailSender.SendEmailAsync(
                        GlobalConstants.ContactEmail,
                        $"{model.Name} - {model.Email}",
                        GlobalConstants.ContactEmail,
                        model.Title,
                        model.Content);
                }
                catch (System.Exception)
                {
                    return result.WithDanger("Не успяхме да изпратим съобщението ви");
                }

                return result.WithSuccess("Съобщението ви бе изпратено успешно!");
            }

            return this.View(model);
        }
    }
}
