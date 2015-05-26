
namespace MvcWebRole2.Handler
{
    using DataStoreLib.BlobStorage;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Web.SessionState;

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

            try
            {
                if (context.Request.Files.Count > 0)
                {
                    List<string> uploadedFiles = new List<string>();

                    BlobStorageService _blobStorageService = new BlobStorageService();

                    HttpFileCollection SelectedFiles = context.Request.Files;

                    int posterCount = _blobStorageService.GetImageFileCount(BlobStorageService.Blob_ImageContainer, name.Replace(" ", "-").ToLower() + "-poster-");

                    for (int i = 0; i < SelectedFiles.Count; i++)
                    {
                        HttpPostedFile postedFile = SelectedFiles[i];
                        
                        string xfileExtention = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(".") + 1);

                        if (type == "poster")
                        {
                            string newPosterName = name.ToLower() + "-poster-" + posterCount + "." + xfileExtention;
                            // upload file on blob
                            string uploadedFile = _blobStorageService.UploadImageFileOnBlob(BlobStorageService.Blob_ImageContainer, newPosterName, postedFile.InputStream);
                            
                            uploadedFiles.Add(newPosterName);
                            posterCount++;
                        }
                        else
                        {
                            string fileName = name.Replace(" ", "-").ToLower() + "." + xfileExtention;

                            _blobStorageService.DeleteFileFromBlob(BlobStorageService.Blob_ImageContainer, fileName);

                            string uploadedFile = _blobStorageService.UploadImageFileOnBlob(BlobStorageService.Blob_ImageContainer, fileName, postedFile.InputStream);

                            uploadedFiles.Add(fileName);
                            break;                           
                        }
                    }

                    context.Response.Write(jss.Serialize(new { Status = "Ok", Message = "file uploaded successfully", FileUrl = uploadedFiles }));
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Please Select Files");
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(jss.Serialize(new { Status = "Error", Error = ex.Message, Message = "Sorry! An error occured while uploading image" }));
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