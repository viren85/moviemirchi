
namespace MvcWebRole1.Controllers
{
    using CloudMovie.APIRole.API;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
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
