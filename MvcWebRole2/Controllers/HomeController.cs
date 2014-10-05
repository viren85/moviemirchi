using System.Web.Mvc;

namespace MvcWebRole2.Controllers
{
    public class HomeController : Controller
    {
        #region Various Views
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Artists()
        {
            return View();
        }

        public ActionResult Critics()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Crawler()
        {
            return View();
        }

        [HttpGet]
        public ActionResult News()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Twitter()
        {
            return View();
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
    }
}
