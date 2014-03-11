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

        public JsonResult AutoComplete(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                //return null;
                return Json(new MovieSearchData(), JsonRequestBehavior.AllowGet);
            }

            SetConnectionString();

            //var tableMgr = new TableManager();
            //var movie = tableMgr.SearchMovies(query);

            var movie = new List<MovieSearchData>() 
            { 
                new MovieSearchData { Id = "1", Title = "Awarapan", TitleImageURL = "Images/awarapan.jpg", Type="Movie", Link="Movie?name=awarapan", Description="Imran Hashmi, Ashotosh Rana" } ,
                new MovieSearchData{ Id = "1", Title = "Don", TitleImageURL = "Images/awarapan-movie.jpg", Type="Movie", Link="Movie?name=don", Description="Amitabh Bachchan, Jinat Aman" } ,
                new MovieSearchData{ Id = "1", Title = "Gunde", TitleImageURL = "Images/awarapan.jpg", Type="Movie", Link="Movie?name=gunde", Description="Ranbir Sing, Arjun Kapor" }, 
                new MovieSearchData{ Id = "1", Title = "Shahrukh Khan", TitleImageURL = "Images/awarapan.jpg", Type="Actor", Link="Movie/Name?name=shahrukh", Description="Actor, My Name Is Khan (2009)" } ,
                new MovieSearchData{ Id = "1", Title = "Amitabh Bachchan", TitleImageURL = "Images/awarapan.jpg", Type="Actor", Link="Movie/Name?name=amitabh", Description="Actor, Sholay (1980)" } ,
                new MovieSearchData{ Id = "1", Title = "Gabbar Sing", TitleImageURL = "Images/awarapan.jpg", Type="Charactor", Link="Charactor?name=gabbar", Description="Character, Sholay (1980)" } 
            };

            var users = (from u in movie
                         where u.Title.ToLower().Contains(query.ToLower())
                         select u).Distinct().ToArray();

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}
