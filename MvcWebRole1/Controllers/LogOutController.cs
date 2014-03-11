using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebRole1.Content.Controllers
{
    public class LogOutController : Controller
    {
        //
        // GET: /LogOut/

        public ActionResult Logout()
        {
            {
                if (Session["UserName"] != null)
                {
                    Session["UserName"] = null;
                    Session.Abandon();
                    Session.Clear();
                }

                ModelState.Clear();
                return RedirectToAction("Index", "Home");
            }

           
        }

    }
}
