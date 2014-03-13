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
                    UserEntity entity = tblMgr.GetUserByName(auth.UserName);        

                    if (entity.UserName == auth.UserName && entity.Password == auth.Password){

                        Session["UserName"] = auth.UserName;
                        return RedirectToAction("AddMovie", "Admin");
                    }
                }
                else
                {
                    TempData["Error"] = "Username or Password Require.";
                }
            }
            catch (Exception)
            {
                TempData["Failed"] = "Login Failed.";
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

            TableManager tblMgr = new TableManager ();

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

                    //SocialMediaToken token = new SocialMediaToken();
                    //token.ManageSocialToken(user.UserId.ToString(), "facebook", result.access_token, string.Empty, user.FirstName);
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
    }
}
