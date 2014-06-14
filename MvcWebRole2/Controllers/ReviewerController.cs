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
using System.Web.Script.Serialization;

namespace MvcWebRole2.Controllers
{
    public class ReviewerController : Controller
    {
        #region Set Connection string
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        #endregion

        #region Add Affiliation
        [HttpGet]
        public ActionResult AddAffiliation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAffiliation(string affiliationJson)
        {
            if (string.IsNullOrEmpty(affiliationJson))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                AffilationEntity affil = json.Deserialize(affiliationJson, typeof(AffilationEntity)) as AffilationEntity;
                if (affil != null)
                {
                    SetConnectionString();

                    AffilationEntity entity = new AffilationEntity();

                    entity.RowKey = entity.AffilationId = Guid.NewGuid().ToString();
                    entity.AffilationName = affil.AffilationName;
                    entity.WebsiteName = affil.WebsiteName;
                    entity.WebsiteLink = affil.WebsiteLink;
                    entity.LogoLink = affil.LogoLink;
                    entity.Country = affil.Country;

                    TableManager tblMgr = new TableManager();
                    tblMgr.UpdateAffilationById(entity);
                }
                else
                {
                    return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update Affiliation
        [HttpGet]
        public ActionResult UpdateAffiliation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateAffiliation(string affiliationJson)
        {
            if (string.IsNullOrEmpty(affiliationJson))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                AffilationEntity affil = json.Deserialize(affiliationJson, typeof(AffilationEntity)) as AffilationEntity;

                if (affil != null)
                {
                    SetConnectionString();

                    AffilationEntity entity = new AffilationEntity();

                    entity.RowKey = entity.AffilationId = affil.AffilationId;
                    entity.AffilationName = affil.AffilationName;
                    entity.WebsiteName = affil.WebsiteName;
                    entity.WebsiteLink = affil.WebsiteLink;
                    entity.LogoLink = affil.LogoLink;
                    entity.Country = affil.Country;

                    TableManager tblMgr = new TableManager();
                    tblMgr.UpdateAffilationById(entity);
                }
                else
                {
                    return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add Reviewer
        [HttpGet]
        public ActionResult AddReviewer()
        {
            SetConnectionString();

            ViewBag.affilation = GetAffilationName();
            return View();
        }

        [HttpPost]
        public ActionResult AddReviewer(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                ReviewerEntity reviewer = json.Deserialize(data, typeof(ReviewerEntity)) as ReviewerEntity;

                if (reviewer != null)
                {
                    SetConnectionString();
                    TableManager tblMgr = new TableManager();

                    ReviewerEntity entity = new ReviewerEntity();
                    entity.RowKey = entity.ReviewerId = reviewer.ReviewerId;
                    entity.ReviewerName = reviewer.ReviewerName;
                    entity.ReviewerImage = reviewer.ReviewerImage;
                    entity.Affilation = reviewer.Affilation;

                    tblMgr.UpdateReviewerById(entity);                    
                }
                else
                {
                    return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet); ;
                }
            }
            catch (Exception)
            {

                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Reviewer
        [HttpGet]
        public ActionResult UpdateReviewer()
        {
            SetConnectionString();

            ViewBag.affilation = GetAffilationName();
            return View();
        }

        [HttpPost]
        public ActionResult UpdateReviewer(string reviewerJson)
        {
            if (string.IsNullOrEmpty(reviewerJson))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                ReviewerEntity reviewer = json.Deserialize(reviewerJson, typeof(ReviewerEntity)) as ReviewerEntity;

                if (reviewer != null)
                {
                    SetConnectionString();
                    ReviewerEntity entity = new ReviewerEntity();

                    entity.RowKey = entity.ReviewerId = reviewer.ReviewerId;
                    entity.ReviewerName = reviewer.ReviewerName;
                    entity.ReviewerImage = reviewer.ReviewerImage;

                    TableManager tblMgr = new TableManager();

                    var affiliation = tblMgr.GetAffilationById(reviewer.Affilation); // use as a id

                    entity.Affilation = json.Serialize(affiliation);

                    tblMgr.UpdateReviewerById(entity);
                }
                else
                {
                    return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet); ;
                }
            }
            catch (Exception)
            {

                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Pirvate Function
        private IEnumerable<SelectListItem> GetAffilationName()
        {
            var tableMgr = new TableManager();

            var affiliations = tableMgr.GetSortedAffilationByName().Select(
                x => new SelectListItem
                {
                    Value = x.AffilationId.ToString(),
                    Text = x.AffilationName
                }
            );


            return new SelectList(affiliations, "Value", "Text");
        }
        #endregion

        public ActionResult GetAffiliationById(string query)
        {
            var movie = new TableManager().GetAffilationById(query);

            return Json(movie, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReviewerById(string query)
        {
            var movie = new TableManager().GetReviewerById(query);

            return Json(movie, JsonRequestBehavior.AllowGet);
        }
    }
}
