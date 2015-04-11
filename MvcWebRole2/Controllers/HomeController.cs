using System.Web.Mvc;
using System.Configuration;

namespace MvcWebRole2.Controllers
{
    public class HomeController : Controller
    {
        #region Various Views

        public ActionResult Index()
        {
            if (Session["AdminUserName"] == null || Session["AdminPassword"] == null)
            {
                return new RedirectResult("/Home/Login");
            }
            else
            {
                return View();
            }
        }


        public ActionResult Artists()
        {
            if (Session["AdminUserName"] == null || Session["AdminPassword"] == null)
            {
                return new RedirectResult("/Home/Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Critics()
        {
            if (Session["AdminUserName"] == null || Session["AdminPassword"] == null)
            {
                return new RedirectResult("/Home/Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(LoginDetails data)
        {
            if (data == null || string.IsNullOrEmpty(data.UserName) || string.IsNullOrEmpty(data.Password))
            {
                return null;
            }

            if (ConfigurationManager.AppSettings["AdminUserName"] == data.UserName && ConfigurationManager.AppSettings["AdminPassword"] == data.Password)
            {
                if (Session["AdminUserName"] == null || Session["AdminPassword"] == null)
                {
                    Session.Add("AdminUserName", data.UserName);
                    Session.Add("AdminPassword", data.Password);
                }

                return Json(new { result = "Redirect", url = Url.Action("Index", "Home") });
            }

            return View();
        }
        [HttpGet]
        public ActionResult Crawler()
        {
            if (Session["AdminUserName"] == null || Session["AdminPassword"] == null)
            {
                return new RedirectResult("/Home/Login");
            }
            else
            {
                return View();
            }
        }

        
        public ActionResult Logout()
        {
            Session["AdminUserName"] = null;
            Session["AdminPassword"] = null;
            Session.Abandon();

            return new RedirectResult("/Home/Login");
        }

        [HttpGet]
        public ActionResult News()
        {
            if (Session["AdminUserName"] == null || Session["AdminPassword"] == null)
            {
                return new RedirectResult("/Home/Login");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Twitter()
        {
            if (Session["AdminUserName"] == null || Session["AdminPassword"] == null)
            {
                return new RedirectResult("/Home/Login");
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region Commented Twitter Code - Need to add it in API project
        /*
        [HttpPost]
        public ActionResult DeleteTwitt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                List<string> twittIds = json.Deserialize(data, typeof(List<string>)) as List<string>;

                if (twittIds != null)
                {
                    TableManager tabmgr = new TableManager();
                    tabmgr.DeleteTwitterItemById(twittIds);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", Message = "Selected news deleted successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetActiveTwitt(string data)
        {
            if (string.IsNullOrEmpty(data) || data == null)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                List<string> twittIds = json.Deserialize(data, typeof(List<string>)) as List<string>;

                if (twittIds != null)
                {
                    TableManager tabmgr = new TableManager();

                    List<TwitterEntity> updatedList = new List<TwitterEntity>();
                    foreach (string twittId in twittIds)
                    {
                        var newsEntity = tabmgr.GetTweetById(twittId);

                        if (newsEntity != null)
                        {
                            foreach (TwitterEntity news in newsEntity.Values)
                            {
                                news.IsActive = true;
                                updatedList.Add(news);
                            }
                        }
                    }

                    tabmgr.UpdateTweetById(updatedList);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", Message = "Selected news updated successfully." }, JsonRequestBehavior.AllowGet);
        }
         * */
        #endregion


        public ActionResult BingImages()
        {
            if (Session["AdminUserName"] == null || Session["AdminPassword"] == null)
            {
                return new RedirectResult("/Home/Login");
            }
            else
            {
                return View();
            }
        }
    }


    public class LoginDetails
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
