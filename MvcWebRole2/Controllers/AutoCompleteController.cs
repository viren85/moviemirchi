using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebRole2.Controllers
{
    public class AutoCompleteController : Controller
    {
        //
        // GET: /AutoComplete/

        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }

        public ActionResult AutoCompleteMovie(string query)
        {
            var list = new List<object>();

            if (string.IsNullOrEmpty(query))
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            SetConnectionString();

            var tableMgr = new TableManager();
            var movies = tableMgr.SearchMovies(query);

            if (movies != null)
            {
                foreach (MovieEntity movieEntity in movies)
                {
                    list.Add(new { id = movieEntity.MovieId, name = movieEntity.Name + " (" + movieEntity.Year + ")" });
                }
            }            

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AutoCompleteReviewer(string query)
        {
            var list = new List<object>();

            if (string.IsNullOrEmpty(query))
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            SetConnectionString();

            var tableMgr = new TableManager();
            var movies = tableMgr.SearchMovies(query);

            if (movies != null)
            {
                foreach (MovieEntity movieEntity in movies)
                {
                    list.Add(new { id = movieEntity.MovieId, name = movieEntity.Name + " (" + movieEntity.Year + ")" });
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
