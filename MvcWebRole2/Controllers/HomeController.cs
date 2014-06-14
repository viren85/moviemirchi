using Crawler;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcWebRole2.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        #region Set connection string
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        #endregion

        public ActionResult Index()
        {
           /* XMLMovieProperties testMovie = new XMLMovieProperties();
            testMovie.MovieName = "Lagaan 3";
            testMovie.MovieLink = "http://www.c-sharpcorner.com/UploadFile/";
            testMovie.Year = 2001;
            testMovie.Month = "April";

            List<XMLReivewProperties> testReivew = new List<XMLReivewProperties> { 
                new XMLReivewProperties() { Name = "Hidustan Times", Link = "http://www.c-sharpcorner.com/UploadFile/" } ,
                new XMLReivewProperties() { Name = "Film fare", Link = "http://www.c-sharpcorner.com/UploadFile/" },
                new XMLReivewProperties() { Name = "Bollywood Hungama", Link = "http://www.c-sharpcorner.com/UploadFile/" }
            };

            testMovie.Reviews = testReivew;

            var filePath = @"D:\GitHub-SVN\moviemirchi\MvcWebRole2\Filters";

            new GenerateXMLFile().CreatingFile(filePath, testMovie);*/

            return View();
        }

        #region Artist view
        public ActionResult Artists()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateArtists(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                ArtistEntity movie = json.Deserialize(data, typeof(ArtistEntity)) as ArtistEntity;

                if (movie != null)
                {
                    string uniqueName = movie.ArtistName.Replace(" ", "-").Replace("&", "-and-").Replace(".", "").Replace("'", "").ToLower();

                    var tableMgr = new TableManager();

                    ArtistEntity entity = new ArtistEntity();

                    entity.RowKey = entity.ArtistId = movie.ArtistId;
                    entity.ArtistName = movie.ArtistName;
                    entity.Bio = movie.Bio;
                    entity.Posters = movie.Posters;
                    entity.Born = movie.Born;
                    entity.MovieList = movie.MovieList;                    
                    entity.UniqueName = movie.UniqueName;                    
                    entity.MyScore = movie.MyScore;
                    entity.JsonString = movie.JsonString;
                    entity.Popularity = movie.Popularity;

                    tableMgr.UpdateArtistById(entity);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", actors = "Actors" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Critics()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Crawler()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateXMLFile(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                XMLMovieProperties movieProps = json.Deserialize(data, typeof(XMLMovieProperties)) as XMLMovieProperties;

                if (movieProps != null)
                {
                    XMLMovieProperties entity = new XMLMovieProperties();

                    entity.MovieName = movieProps.MovieName;
                    entity.MovieLink = movieProps.MovieLink;
                    entity.Year = Convert.ToInt32(movieProps.Month.Split(new char[] { ' ' })[1]);
                    entity.Month = movieProps.Month;
                    entity.Reviews = movieProps.Reviews;

                    string xmlFilePath = Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);

                    string fullSavedFileName = new GenerateXMLFile().CreatingFile(xmlFilePath, entity);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", actors = "Actors" }, JsonRequestBehavior.AllowGet);
        }
    }
}
