using DataStoreLib.BlobStorage;
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
            string name = context.Request.QueryString["name"];
            string type = context.Request.QueryString["type"];

            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (!string.IsNullOrEmpty(context.Request.Headers["X-File-Name"]))
            {
                try
                {
                    BlobStorageService _blobStorageService = new BlobStorageService();

                    string xFileName = context.Request.Headers["X-File-Name"];
                    string xfileExtention = xFileName.Substring(xFileName.LastIndexOf(".") + 1);

                    if (type == "poster")
                    {
                        int posterCount = _blobStorageService.GetImageFileCount(BlobStorageService.Blob_ImageContainer, name.Replace(" ", "-").ToLower() + "-poster-");

                        string newPosterName = name.ToLower() + "-poster-" + posterCount + "." + xfileExtention;
                        // upload file on blob
                        string uploadedFile = _blobStorageService.UploadImageFileOnBlob(BlobStorageService.Blob_ImageContainer, newPosterName, context.Request.InputStream);

                        context.Response.Write(jss.Serialize(new { Status = "Ok", Message = "file uploaded successfully", FileUrl = newPosterName }));
                    }
                    else
                    {
                        string fileName = name.Replace(" ", "-").ToLower() + "." + xfileExtention;

                        _blobStorageService.DeleteFileFromBlob(BlobStorageService.Blob_ImageContainer, fileName);

                        string uploadedFile = _blobStorageService.UploadImageFileOnBlob(BlobStorageService.Blob_ImageContainer, fileName, context.Request.InputStream);

                        context.Response.Write(jss.Serialize(new { Status = "Ok", Message = "file uploaded successfully", FileUrl = fileName }));
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write(jss.Serialize(new { Status = "Error", Error = ex.Message, Message = "Sorry! An error occured while uploading image on mooc server" }));
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