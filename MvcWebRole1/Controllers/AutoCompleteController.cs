using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.WindowsAzure;
using System.Diagnostics;
using DataStoreLib.Constants;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using LuceneSearchLibrarby;

namespace MvcWebRole1.Controllers
{
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
            var movies = tableMgr.GetAllMovies();

           // List<Object> allCast = new List<Object>();
            List<Cast> tempCast = new List<Cast>();
            //int counter = 0;
            foreach (var movie in movies)
            {
                List<Cast> castList = json.Deserialize(movie.Value.Casts, typeof(List<Cast>)) as List<Cast>;
                if (castList != null)
                {

                    foreach (var cast in castList)
                    {
                        if (!tempCast.Exists(c => c.name == cast.name))
                        {
                            tempCast.Add(cast);
                            list.Add(new {name = cast.name });
                        }
                    }

                }
            }
            /*
            if (movies != null)
            {
                foreach (MovieEntity movieEntity in movies)
                {
                    list.Add(new { id = movieEntity.MovieId, name = movieEntity.Name + " (" + movieEntity.Year + ")" });
                }
            }
            */
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteMovies(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                //return null;
                return Json(new MovieSearchData(), JsonRequestBehavior.AllowGet);
            }

            var users = LuceneSearch.Search(query);

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}
