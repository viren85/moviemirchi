
namespace MvcWebRole1.Controllers
{
    using CloudMovie.APIRole.API;
    using DataStoreLib.Utils;
    using Microsoft.WindowsAzure;
    using System.Diagnostics;
    using System.Web.Mvc;

    public class MovieController : Controller
    {

        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        //
        // GET: /Movie/
        [HttpGet]
        public ActionResult Index(string movieid)
        {
            SetConnectionString();
            return View();
        }

        public ActionResult Reviewer(string name)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Name(string name)
        {
            ViewBag.ActorName = name.ToUpper();
            return View();
        }

      
    }
}
