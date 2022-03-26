namespace StudentsHelper.Web.Controllers
{
    using System.Text;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [AllowAnonymous]
    public class SitemapsController : Controller
    {
        private readonly string domainUrl;

        public SitemapsController(IConfiguration configuration)
        {
            this.domainUrl = configuration["DOMAIN"];
        }

        public IActionResult Sitemap()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            var pages = new string[]
            {
                string.Empty,
                "Home/Index",
                "Identity/Account/Register",
                "Identity/Account/Login",
                "Contact",
            };

            foreach (var pagePath in pages)
            {
                var url = $"{this.domainUrl}{pagePath}";
                sb.AppendLine($"<url><loc>{url}</loc></url>");
            }

            sb.AppendLine("</urlset>");
            return this.Content(sb.ToString(), "application/xml");
        }
    }
}
