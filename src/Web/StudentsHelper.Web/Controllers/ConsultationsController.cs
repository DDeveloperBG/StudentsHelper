using Microsoft.AspNetCore.Mvc;

namespace StudentsHelper.Web.Controllers
{
    public class ConsultationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
