using DataStoreLib.BlobStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public static class Util
    {
        public static string DEFAULT_POPULARITY = "1";
        public static string DEFAULT_SCORE = "0";
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
            try
            {
                fileName = fileName + "." + url.Substring(url.LastIndexOf(".") + 1);

                using (WebClient client = new WebClient())
                {
                    byte[] data = client.DownloadData(url);

                    Stream stream = new MemoryStream(data);

                    return new BlobStorageService().UploadImageFileOnBlob(BlobStorageService.Blob_NewsImages, fileName, stream);
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}