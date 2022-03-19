namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;

    using StudentsHelper.Data.Models.Contact;
    using StudentsHelper.Services.Data.Contact;
    using StudentsHelper.Web.ViewModels.Contact;
    using Xunit;

    public class ContactServiceTests : BaseTest
    {
        private List<ContactFormEntry> contactForms;
        private ContactService service;

        [Fact]
        public void SaveContactFormDataAsync_Succeede()
        {
            const string ip = "127.0.0.1";
            var input = new ContactInputModel
            {
                Content = "Hello World",
                Email = "abv@edo.non",
                Name = "Zoro Ivanov",
                Title = "Hello",
            };

            this.service.SaveContactFormDataAsync(input, ip).GetAwaiter().GetResult();

            Assert.Single(this.contactForms);
            var addedContactForm = this.contactForms[0];

            Assert.Equal(input.Content, addedContactForm.Content);
            Assert.Equal(input.Email, addedContactForm.Email);
            Assert.Equal(input.Name, addedContactForm.Name);
            Assert.Equal(input.Title, addedContactForm.Title);
            Assert.Equal(ip, addedContactForm.Ip);
        }

        public override void CleanWorkbench()
        {
            this.contactForms = new List<ContactFormEntry>();

            var contactFormsRepository = GetMockedClasses.MockIRepository(this.contactForms);

            this.service = new ContactService(contactFormsRepository.Object);
        }
    }
}
