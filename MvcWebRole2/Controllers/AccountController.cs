using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;


namespace MvcWebRole2.Controllers
{
    public class AccountController : Controller
    {
        #region Set Connection String
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        #endregion

        #region Login
        // GET: /Login/
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["userid"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Please enter username and password";
                return View();
            }

            try
            {
                SetConnectionString();

                TableManager tblMgr = new TableManager();
                UserEntity entity = tblMgr.GetUserByName(userName);
                if (entity != null)
                {
                    if (entity.UserName == userName && entity.Password == password)
                    {
                        Session["user"] = entity.UserName;
                        Session["userid"] = entity.UserId;
                        Session["username"] = entity.FirstName + " " + entity.LastName;
                        Session["type"] = entity.UserType;

                        return RedirectToAction("AddMovie", "Movie");
                    }
                    else
                    {
                        TempData["Error"] = "Login Failed. Inalid username or password.";
                    }
                }
                else
                {
                    TempData["Error"] = "Login Failed. Inalid username or password.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Login Failed. Inalid username or password.";
            }

            return View();
        }
        #endregion

        #region Sign up
        [HttpGet]
        public ActionResult Register()
        {
            if (Session["userid"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public void Crawl()
        {
            SetConnectionString();
            Crawler.MovieCrawler movieCrawler = new Crawler.MovieCrawler();
            try
            {
                XmlDocument xdoc = new XmlDocument();
                var file = Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);
                xdoc.Load(file);

                var movies = xdoc.SelectNodes("MovieList/Movies/Month/Movie");

                if (!Directory.Exists(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters")))
                {
                    Directory.CreateDirectory(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"));
                }

                if (!Directory.Exists(Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Images")))
                {
                    Directory.CreateDirectory(Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Images"));
                }

                if (!Directory.Exists(Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Thumbnails")))
                {
                    Directory.CreateDirectory(Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Thumbnails"));
                }

                List<string> processedList = new List<string>();

                foreach (XmlNode movie in movies)
                {
                    if (movie.Attributes["link"] != null && !string.IsNullOrEmpty(movie.Attributes["link"].Value))
                    {
                        try
                        {
                            MovieEntity mov = movieCrawler.Crawl(movie.Attributes["link"].Value);
                            TableManager tblMgr = new TableManager();
                            tblMgr.UpdateMovieById(mov);
                            processedList.Add(movie.Attributes["link"].Value);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Error while crawling movie - " + movie.Attributes["link"].Value);
                        }
                    }
                }

                if (processedList.Count > 0)
                {
                    foreach (string name in processedList)
                    {
                        XmlNode node = xdoc.SelectSingleNode("MovieList/Movies/Month/Movie[@link='" + name + "']");
                        xdoc.RemoveChild(node);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult Register(string userJson)
        {
            if (string.IsNullOrEmpty(userJson))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();

                UserEntity deUser = json.Deserialize(userJson, typeof(UserEntity)) as UserEntity;

                if (deUser != null)
                {
                    try
                    {
                        System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(deUser.Email);
                    }
                    catch (Exception)
                    {
                        return Json(new { Status = "Error", Message = "Please provide valid email address." }, JsonRequestBehavior.AllowGet);
                    }

                    if (deUser.Password != deUser.Mobile)
                    {
                        return Json(new { Status = "Error", Message = "Password and confirm password does not match." }, JsonRequestBehavior.AllowGet);
                    }

                    SetConnectionString();

                    TableManager tblMgr = new TableManager();

                    UserEntity oldUser = tblMgr.GetUserByName(deUser.UserName);

                    if (oldUser == null)
                    {
                        UserEntity entity = new UserEntity();
                        entity.RowKey = entity.UserId = Guid.NewGuid().ToString();
                        entity.UserName = deUser.UserName;
                        entity.Password = deUser.Password;
                        entity.FirstName = deUser.FirstName;
                        entity.LastName = deUser.LastName;
                        entity.Email = deUser.Email;
                        entity.UserType = "Application";
                        entity.Status = 1;
                        entity.Created_At = DateTime.Now;
                        entity.Country = string.Empty;

                        tblMgr.UpdateUserById(entity);
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Username (" + deUser.UserName + ") already exist. Please choose another username." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Status = "Error", Message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["userid"] = null;
            Session["username"] = null;
            Session["type"] = null;
            Session.Abandon();
            Session.Clear();

            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}
