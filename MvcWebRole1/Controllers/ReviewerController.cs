
namespace MvcWebRole1.Controllers
{
    using System.Web.Mvc;

    public class ReviewerController : Controller
    {
        [HttpGet]
        public ActionResult Index(string name)
        {
            return View();
        }
    }
}
