using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace MvcWebRole2.Handler
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadFile : IHttpHandler, IRequiresSessionState
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        string cnxnString = string.Empty;
        string logPath = string.Empty;
        string configFilePath = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            string movieName = context.Request.QueryString["movie"];
            string banner = context.Request.QueryString["banner"];

            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (!string.IsNullOrEmpty(context.Request.Headers["X-File-Name"]))
            {
                try
                {
                    string path = string.Empty;
                    string xFileName = context.Request.Headers["X-File-Name"];
                    string xfileExtention = xFileName.Substring(xFileName.LastIndexOf(".") + 1);

                    int posterCount = new MovieCrawler.ImdbCrawler().GetMaxImageCounter(movieName);

                    string newPosterName = movieName + "-poster-" + posterCount + "." + xfileExtention;

                    string folderPath = Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    path = Path.Combine(folderPath, newPosterName);

                    Stream inputStream = context.Request.InputStream;
                    FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                    inputStream.CopyTo(fileStream);
                    fileStream.Close();

                    context.Response.Write(jss.Serialize(new { Status = "Ok", Message = "file uploaded successfully", FileUrl = newPosterName }));
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write(jss.Serialize(new { Status = "Error", Message = "Sorry! An error occured while uploading image on mooc server" }));
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}