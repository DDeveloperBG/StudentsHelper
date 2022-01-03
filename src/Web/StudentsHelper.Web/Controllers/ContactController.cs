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
            var result = this.RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.emailSender.SendEmailAsync(GlobalConstants.ContactEmail, model.Email, GlobalConstants.ContactEmail, model.Title, model.Message, null);
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
