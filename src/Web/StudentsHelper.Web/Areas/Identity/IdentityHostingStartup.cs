using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(StudentsHelper.Web.Areas.Identity.IdentityHostingStartup))]

namespace StudentsHelper.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
