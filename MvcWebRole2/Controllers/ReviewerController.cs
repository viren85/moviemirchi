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

    }
}
