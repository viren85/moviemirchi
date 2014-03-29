using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebRole1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Index2()
        {        
            return View();
        }

        public ActionResult SetVariable(string value)
        {
            Session["favorite"] = value;

            return this.Json(new { Status = "Ok" });
        }

    }
}
