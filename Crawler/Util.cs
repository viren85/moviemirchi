
namespace Crawler
{
    using DataStoreLib.BlobStorage;
    using System;
    using System.IO;
    using System.Net;

    public static class Util
    {
        public const string DEFAULT_POPULARITY = "1";
        public const string DEFAULT_SCORE = "0";

        public static string StripHTMLTags(string html)
        {
            string stripedHtml = string.Empty;

            try
            {
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(html);
                stripedHtml = htmlDoc.DocumentNode.InnerText;
            }
            catch (Exception)
            {
                //TODO - Log an error message
            }

            return stripedHtml;
        }

        /// <summary>
        /// Download image and return it blob url path
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string DownloadImage(string url, string fileName)
        {
            string result = string.Empty;

            try
            {
                fileName = fileName + "." + url.Substring(url.LastIndexOf(".") + 1);

                using (WebClient client = new WebClient())
                {
                    byte[] data = client.DownloadData(url);

                    using (Stream stream = new MemoryStream(data))
                    {
                        result = new BlobStorageService().UploadImageFileOnBlob(BlobStorageService.Blob_NewsImages, fileName, stream);
                    }
                }
            }
            catch (Exception)
            {
                //TODO - Log an error message
            }

            return result;
        }

        public static string UploadLogFile(string fileName)
        {
            try
            {
                using (Stream stream = new StreamReader(fileName).BaseStream)
                {
                    return new BlobStorageService().UploadImageFileOnBlob(BlobStorageService.Blob_AlgoLogs, Path.GetFileName(fileName), stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}