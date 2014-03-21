using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using MvcWebRole1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers
{
    public class LoginController : Controller
    {
        //string userName, password;
        //
        // GET: /Login/

        [HttpGet]
        public ActionResult UserLogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(string hfLogin)
        {
            if (string.IsNullOrEmpty(hfLogin))
            {
                return View();
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                UserEntity auth = json.Deserialize(hfLogin, typeof(UserEntity)) as UserEntity;

                if (auth != null)
                {
                    SetConnectionString();

                    TableManager tblMgr = new TableManager();
                    UserEntity user = tblMgr.GetUserByName(auth.UserName);

                    if (user.UserName == auth.UserName && user.Password == auth.Password)
                    {
                        Session["userid"] = user.UserId;
                        Session["type"] = user.UserType;
                        Session["firstname"] = user.FirstName;
                        Session["lastname"] = user.LastName;
                        Session["email"] = user.Email;
                        Session["mobile"] = user.Mobile;
                        Session["dob"] = user.DateOfBirth;
                        Session["gender"] = user.Gender;
                        Session["city"] = user.City;
                        Session["username"] = user.FirstName + " " + user.LastName;
                        Session["favorite"] = user.Favorite;

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["Error"] = "Username or Password Require.";
                }
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Login Failed. Please try again";
            }

            return View();
        }

        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }

        public ActionResult ConnectUser(UserEntity user)
        {
            SetConnectionString();

            TableManager tblMgr = new TableManager();

            try
            {
                Session["userid"] = user.UserId;
                Session["type"] = user.UserType;
                Session["firstname"] = user.FirstName;
                Session["lastname"] = user.LastName;
                Session["email"] = user.Email;
                Session["mobile"] = user.Mobile;
                Session["dob"] = user.DateOfBirth;
                Session["gender"] = user.Gender;
                Session["city"] = user.City;
                Session["username"] = user.FirstName + " " + user.LastName;
                Session["profile_pic"] = user.Profile_Pic_Http;
                Session["profile_pic_https"] = user.Profile_Pic_Https;
                Session["facebook_access_token"] = user.Country;
                Session["favorite"] = user.Favorite = null;

                if (user.UserType == "facebook")
                {
                    var fb = new Facebook.FacebookClient();
                    dynamic result = fb.Get("oauth/access_token",
                                            new
                                            {
                                                client_id = ConfigurationManager.AppSettings["FacebookAppId"],
                                                client_secret = ConfigurationManager.AppSettings["FacebookAppSecret"],
                                                grant_type = "fb_exchange_token",
                                                fb_exchange_token = user.Country
                                            });
                }

                var userResponse = tblMgr.GetUserById(user.UserId);

                if (userResponse == null)
                {
                    user.Status = 1;
                    user.Created_At = DateTime.Now;
                    user.Country = string.Empty;
                    user.RowKey = user.UserId;

                    tblMgr.UpdateUserById(user);

                    return Json(new { success = true, createdUser = true });
                }

                return Json(new { success = true, createdUser = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        public ActionResult Logout()
        {
            Session["userid"] = null;
            Session["type"] = null;
            Session["firstname"] = null;
            Session["lastname"] = null;
            Session["email"] = null;
            Session["mobile"] = null;
            Session["dob"] = null;
            Session["gender"] = null;
            Session["city"] = null;
            Session["username"] = null;
            Session["profile_pic"] = null;
            Session["profile_pic_https"] = null;
            Session["facebook_access_token"] = null;
            Session["favorite"] = null;

            Session.Abandon();
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
