

namespace CloudMovie.APIRole.Controllers.api
{
    using CloudMovie.APIRole.API;
    using Crawler;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using MovieCrawler;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class CrawlPostersController : BaseController
    {
        protected override string ProcessRequest()
        {
            return string.Empty;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ActionResult GetPosters(XMLMovieProperties prop)
        {
            try
            {
                if (prop == null)
                    return null;

                JavaScriptSerializer json = new JavaScriptSerializer();
                TableManager tblMgr = new TableManager();
                List<string> urls = new SantaImageCrawler().GetMoviePosterUrls(prop.SantaPosterLink);
                ImdbCrawler ic = new ImdbCrawler();

                MovieEntity me = tblMgr.GetMovieByUniqueName(prop.MovieName.ToLower());
                List<string> processedUrl = json.Deserialize<List<string>>(me.Posters);
                List<PosterInfo> posters = json.Deserialize<List<PosterInfo>>(me.Pictures);

                int imageCounter = 1;
                string newImageName = string.Empty;

                if (processedUrl != null)
                {
                    imageCounter = processedUrl.Count + 1;

                    if (posters == null)
                        posters = new List<PosterInfo>();

                    foreach (string process in processedUrl)
                    {
                        PosterInfo info = new PosterInfo();
                        info.url = process;
                        posters.Add(info);
                    }
                }
                else
                {
                    processedUrl = new List<string>();
                    posters = new List<PosterInfo>();

                }

                foreach (string url in urls)
                {
                    PosterInfo info = new PosterInfo();

                    try
                    {
                        string posterPath = ic.GetNewImageName(prop.MovieName, ic.GetFileExtension(url), imageCounter, false, ref newImageName);
                        ic.DownloadImage(url, posterPath);

                        processedUrl.Add(newImageName);

                        info.url = newImageName;
                        info.source = prop.SantaPosterLink;
                        posters.Add(info);

                        imageCounter++;
                    }
                    catch (Exception)
                    {
                        // Skip that image
                    }
                }

                me.Posters = JsonConvert.SerializeObject(processedUrl);
                me.Pictures = JsonConvert.SerializeObject(posters);
                tblMgr.UpdateMovieById(me);
            }
            catch (Exception)
            {

            }

            return null;
            //return Json(new { Status = "Ok", Message = "Selected news deleted successfully." }, JsonRequestBehavior.AllowGet);
        }
    }
}
