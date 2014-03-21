using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuceneSearchLibrarby;

namespace MvcWebRole1.Controllers
{
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
