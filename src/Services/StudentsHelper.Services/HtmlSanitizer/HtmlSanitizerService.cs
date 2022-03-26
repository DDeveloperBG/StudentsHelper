namespace StudentsHelper.Services.HtmlSanitizer
{
    using Ganss.XSS;

    public class HtmlSanitizerService : IHtmlSanitizerService
    {
        public string SanitizeHtml(string html)
        {
            var sanitizer = new HtmlSanitizer();

            var sanitized = sanitizer.Sanitize(html, string.Empty);

            return sanitized;
        }
    }
}
