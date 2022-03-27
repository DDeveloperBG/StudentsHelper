namespace StudentsHelper.Web.Tests
{
    using System;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;

    using Xunit;

    public class AuthSeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        private const string RegisterPagePath = "/Identity/Account/Register";
        private const string LoginPagePath = "/Identity/Account/Login";

        private readonly SeleniumServerFactory<Startup> server;
        private readonly IWebDriver browser;

        public AuthSeleniumTests(SeleniumServerFactory<Startup> server)
        {
            this.server = server;
            server.CreateClient();
            var opts = new ChromeOptions();
            opts.AddArguments("--headless");
            opts.AcceptInsecureCertificates = true;
            this.browser = new ChromeDriver(opts);
            this.browser.Manage().Window.Maximize();
        }

        [Fact]
        public void GoToRegisterPage()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri);
            this.browser
                .FindElement(By.CssSelector($"a[href='{RegisterPagePath}']"))
                .Click();

            Assert.Equal(this.server.RootUri + RegisterPagePath, this.browser.Url);

            this.AssertPageIsActiveAndOtherNot(1);
        }

        [Fact]
        public void GoToLoginPage()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri);
            this.browser
                .FindElement(By.CssSelector($"a[href='{LoginPagePath}']"))
                .Click();

            Assert.Equal(this.server.RootUri + LoginPagePath, this.browser.Url);
        }

        [Fact]
        public void RegisterStudent()
        {
            var userData = new
            {
                Name = "TestStudent",
                Email = Guid.NewGuid().ToString() + "@gmail.com",
                Password = "my pass 123321",
            };

            this.browser.Navigate().GoToUrl(this.server.RootUri + RegisterPagePath);

            // Choose student role and continue to page 2
            IJavaScriptExecutor js = (IJavaScriptExecutor)this.browser;
            js.ExecuteScript("$(\".role[userrole='student']\").click()");
            this.AssertPageIsActiveAndOtherNot(2);

            // Fill user name
            this.SetValueOfInputElement("Input_Name", userData.Name);

            // Fill user email
            this.SetValueOfInputElement("Input_Email", userData.Email);

            // Fill user password
            this.SetValueOfInputElement("Input_Password", userData.Password);

            // Finish registration
            this.browser.FindElement(By.Id("register-submit")).Click();
            Assert.Equal(this.server.RootUri + '/', this.browser.Url);
        }

        [Fact]
        public void RegisterTeacher()
        {
            var userData = new
            {
                Name = "TestTeacher",
                Email = Guid.NewGuid().ToString() + "@gmail.com",
                Password = "my pass 123321",
                Region = "3",
                Township = "30",
                PopulatedArea = "161",
                School = "291",
            };

            this.browser.Navigate().GoToUrl(this.server.RootUri + RegisterPagePath);

            // Choose student role and continue to page 2
            IJavaScriptExecutor js = (IJavaScriptExecutor)this.browser;
            js.ExecuteScript("$(\".role[userrole='teacher']\").click()");
            this.AssertPageIsActiveAndOtherNot(2);

            // Fill user name
            this.SetValueOfInputElement("Input_Name", userData.Name);

            // Fill user email
            this.SetValueOfInputElement("Input_Email", userData.Email);

            // Fill user password
            this.SetValueOfInputElement("Input_Password", userData.Password);

            // Continue to page 3
            this.browser.FindElement(By.Id("continue")).Click();
            this.AssertPageIsActiveAndOtherNot(3);

            // Choose region
            this.SetValueOfSelectElement("regions", userData.Region);

            // Choose township
            this.SetValueOfSelectElement("townships", userData.Township);

            // Choose populatedArea
            this.SetValueOfSelectElement("populatedAreas", userData.PopulatedArea);

            // Choose school
            this.SetValueOfSelectElement("Input.TeacherModel.SchoolId", userData.School);

            // Continue to page 4
            this.browser.FindElement(By.Id("continue")).Click();
            this.AssertPageIsActiveAndOtherNot(4);

            // Uploud qualification document
            var qualificationDocEl = this.browser.FindElement(By.Id("Input_TeacherModel_QualificationDocument"));
            qualificationDocEl.SendKeys(@"E:\Downloads\IMG_20220317_100328_685.jpg");

            // Finish registration
            this.browser.FindElement(By.Id("register-submit")).Click();

            // Throws error because of stripe fails in local enviroment
            Assert.Equal($"{this.server.RootUri}{RegisterPagePath}", this.browser.Url);
            var errorTitleEl = this.browser.FindElement(By.CssSelector(".error-title"));
            Assert.Equal("Woops!\r\nSomething went wrong :(", errorTitleEl.Text);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.server?.Dispose();
                this.browser?.Dispose();
            }
        }

        private void AssertPageIsActiveAndOtherNot(int page)
        {
            for (int i = 1; i <= 4; i++)
            {
                Assert.Equal(i == page, this.browser.FindElement(By.Id($"part-{i}")).Displayed);
            }
        }

        private void SetValueOfInputElement(string id, string value)
        {
            var inputEl = this.browser.FindElement(By.Id(id));
            inputEl.SendKeys(value);
            Assert.Equal(value, inputEl.GetAttribute("value"));
        }

        private void SetValueOfSelectElement(string name, string value)
        {
            var el = this.browser.FindElement(By.CssSelector($"select[name='{name}'"));
            var selectEl = new SelectElement(el);
            selectEl.SelectByValue(value);

            Assert.Equal(value, el.GetAttribute("value"));
        }
    }
}
