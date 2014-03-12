using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using MvcWebRole1.Models;
using System;
using System.Collections.Generic;
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
                AuthenticationEntity auth = json.Deserialize(hfLogin, typeof(AuthenticationEntity)) as AuthenticationEntity;
                if (auth != null)
                {
                    SetConnectionString();

                    TableManager tblMgr = new TableManager();
                    AuthenticationEntity entity = tblMgr.GetUserByName(auth.UserName);        

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





        //public string q { get; set; }
    }
}
