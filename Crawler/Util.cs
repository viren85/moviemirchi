using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}