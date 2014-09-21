
namespace MvcWebRole1.Controllers
{
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

        public ActionResult Home()
        {
            var tableMgr = new TableManager();

            var current = tableMgr.GetCurrentMovies();
            var upcoming = tableMgr.GetUpcomingMovies();
            var news = tableMgr.GetRecentNews(0, 20);
            var tweets = tableMgr.GetRecentTweets(0, 20);

            ViewData["CurrentMovies"] = jsonSerializer.Value.Serialize(current);
            ViewData["UpcomingMovies"] = jsonSerializer.Value.Serialize(upcoming);
            ViewData["News"] = jsonSerializer.Value.Serialize(news);
            ViewData["Tweets"] = jsonSerializer.Value.Serialize(tweets);
            return View();
        }

    }
}
