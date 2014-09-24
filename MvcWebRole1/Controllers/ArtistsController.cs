
namespace MvcWebRole1.Controllers
{
    using CloudMovie.APIRole.API;
    using DataStoreLib.Utils;
    using Microsoft.WindowsAzure;
    using System.Diagnostics;
    using System.Web.Mvc;

    public class ArtistsController : Controller
    {
        [HttpGet]
        public ActionResult Index(string name)
        {
            return View();
        }
    }
}
