namespace RPM.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Services.Data.Contact;
    using StudentsHelper.Services.Messaging;
    using StudentsHelper.Web.Controllers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Contact;

    public class ContactController : BaseController
    {
        private readonly IEmailSender emailSender;
        private readonly IContactService contactService;

        public ContactController(
            IEmailSender emailSender,
            IContactService contactService)
        {
            this.emailSender = emailSender;
            this.contactService = contactService;
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
                var result = this.RedirectToAction("Index", "Home", new { area = string.Empty });

                try
                {
                    var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();

                    await this.contactService.SaveContactFormData(model, ip);

                    await this.emailSender.SendEmailAsync(
                        GlobalConstants.ContactEmail,
                        $"{model.Name} - {model.Email}",
                        GlobalConstants.ContactEmail,
                        model.Title,
                        model.Content);
                }
                catch (System.Exception)
                {
                    return result.WithDanger(GlobalConstants.ContactFormMessages.MessageNotSentMessage);
                }

                return result.WithSuccess(GlobalConstants.ContactFormMessages.MessageSentSuccessfullyMessage);
            }

            return this.View(model);
        }
    }
}
