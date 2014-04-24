
namespace MvcWebRole1.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Index2()
        {        
            return View();
        }

        public ActionResult SetVariable(string value)
        {
            Session["favorite"] = value;

            return this.Json(new { Status = "Ok" });
        }

    }
}
