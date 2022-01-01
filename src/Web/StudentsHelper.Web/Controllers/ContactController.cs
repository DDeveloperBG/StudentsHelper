namespace RPM.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Common;
    using StudentsHelper.Services.Messaging;
    using StudentsHelper.Web.Controllers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Contact;

    public class ContactController : BaseController
    {
        private readonly IEmailSender emailSender;

        public ContactController(
            IEmailSender emailSender)
        {
            this.emailSender = emailSender;
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
                await this.emailSender.SendEmailAsync(model.Email, null, GlobalConstants.ContactEmail, model.Title, model.Message, null);

                return this.RedirectToAction(
                    nameof(HomeController.Index),
                    "Home",
                    new { area = string.Empty })
                    .WithSuccess("Съобщението ви бе изпратено успешно!");
            }

            return this.View(model);
        }
    }
}
