
namespace MvcWebRole1.Controllers
{
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using LuceneSearchLibrary;
    using Microsoft.WindowsAzure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class AutoCompleteController : Controller
    {
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }


        public ActionResult AutoCompleteActors(string query)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var list = new List<object>();

            if (string.IsNullOrEmpty(query))
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            SetConnectionString();

            var tableMgr = new TableManager();
            var movies = tableMgr.SearchMoviesByActor(query);

            // List<Object> allCast = new List<Object>();
            List<Cast> tempCast = new List<Cast>();
            //int counter = 0;
            foreach (var movie in movies)
            {
                List<Cast> castList = json.Deserialize(movie.Casts, typeof(List<Cast>)) as List<Cast>;
                if (castList != null)
                {

                    foreach (var cast in castList)
                    {
                        if (!tempCast.Exists(c => c.name == cast.name))
                        {
                            tempCast.Add(cast);
                        }
                    }

                }
            }

            var actors = (from u in tempCast
                          where u.name.ToLower().Contains(query.ToLower()) && u.role.ToLower() == "actor"
                          select u).Distinct().ToArray().ToList();

            foreach (var actor in actors)
            {
                list.Add(new { name = actor.name });
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoCompleteDirectors(string query)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var list = new List<object>();

            if (string.IsNullOrEmpty(query))
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            SetConnectionString();

            var tableMgr = new TableManager();
            var movies = tableMgr.SearchMoviesByActor(query);

            // List<Object> allCast = new List<Object>();
            List<Cast> tempCast = new List<Cast>();
            //int counter = 0;
            foreach (var movie in movies)
            {
                List<Cast> castList = json.Deserialize(movie.Casts, typeof(List<Cast>)) as List<Cast>;
                if (castList != null)
                {

                    foreach (var cast in castList)
                    {
                        if (!tempCast.Exists(c => c.name == cast.name))
                        {
                            tempCast.Add(cast);
                        }
                    }

                }
            }

            var directors = (from u in tempCast
                             where u.name.ToLower().Contains(query.ToLower()) && u.role.ToLower() == "director"

                             select u).Distinct().ToArray().ToList();

            foreach (var director in directors)
            {
                if (director.role.ToLower() == "director")
                {
                    list.Add(new { name = director.name });
                }

            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AutoCompleteMusicDirectors(string query)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var list = new List<object>();

            if (string.IsNullOrEmpty(query))
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            SetConnectionString();

            var tableMgr = new TableManager();
            var movies = tableMgr.SearchMoviesByActor(query);

            // List<Object> allCast = new List<Object>();
            List<Cast> tempCast = new List<Cast>();
            //int counter = 0;
            foreach (var movie in movies)
            {
                List<Cast> castList = json.Deserialize(movie.Casts, typeof(List<Cast>)) as List<Cast>;
                if (castList != null)
                {

                    foreach (var cast in castList)
                    {
                        if (!tempCast.Exists(c => c.name == cast.name))
                        {
                            tempCast.Add(cast);
                        }
                    }

                }
            }

            var directors = (from u in tempCast
                             where u.name.ToLower().Contains(query.ToLower()) && u.role.ToLower() == "music director"
                            ||
                            u.name.ToLower().Contains(query.ToLower()) && u.role.ToLower() == "musicdirector"
                             select u).Distinct().ToArray().ToList();

            foreach (var director in directors)
            {
                list.Add(new { name = director.name });
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AutoCompleteMovies(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    //return null;
                    return Json(new MovieSearchData(), JsonRequestBehavior.AllowGet);
                }

                var users = LuceneSearch.Search(query);

                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new MovieSearchData(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}
