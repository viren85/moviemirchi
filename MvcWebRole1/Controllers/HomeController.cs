
namespace MvcWebRole1.Controllers
{
    using CloudMovie.APIRole.API;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using MvcWebRole1.Models.Page;
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

        private static Lazy<HomePage> page = new Lazy<HomePage>(() =>
        {
            var page = new HomePage();

            var controller = new MvcWebRole1.Controllers.Interface.MovieController();

            page.UpcomingMovies = controller.GetUpcoming();
            page.NowPlayingMovies = controller.GetNowPlaying();

            return page;
        });

        public ActionResult Index()
        {
            return View(page.Value);
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
