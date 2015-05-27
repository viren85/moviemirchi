using DataStoreLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Crawler.Reviews
{
    public class TimesOfIndia
    {
        private CrawlerHelper helper = new CrawlerHelper();
        string reviewPageContent = string.Empty;
        public ReviewEntity Crawl(string url, string affiliation)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);

                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    }

                    reviewPageContent = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();

                    return PopulateReviewDetail(reviewPageContent, affiliation);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Exception occored while getting reviews (Times of India), message= {0}", ex.Message));
            }

            return null;
        }

        public ReviewEntity PopulateReviewDetail(string html, string affiliation)
        {
            ReviewEntity re = new ReviewEntity();
            string rating = string.Empty;
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                if (bodyNode == null)
                {
                    Console.WriteLine("Body is empty");
                }
                else
                {
                    
            
                    //var headerNode = helper.GetElementWithAttribute(bodyNode, "span", "class", "arttle");
                    var headerNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "movieReviewLeft");

                    var reviewerNode = helper.GetElementWithAttribute(headerNode, "div", "class", "flmcasting");

                    //reviewer
                    var ratingNode = helper.GetElementWithAttribute(reviewerNode, "span", "class", "ratingMovie");

                    //change code for getting critics rating, existing code not working.
                    double rate = 0;

                    try
                    {
                        if (ratingNode != null)
                        {
                            string ratStr = ratingNode.InnerText.Substring(0, 3);

                            rate = Convert.ToDouble(ratStr) * 2;
                        }
                    }
                    catch (Exception)
                    {
                    }                    

                    reviewerNode = helper.GetElementWithAttribute(reviewerNode, "span", "class", "movietime");

                    HtmlNode head = headerNode.SelectSingleNode("h1");
                    var header = head == null ? string.Empty : head.InnerText;

                    var reviewerName = reviewerNode.SelectSingleNode("a");
                    var reviewName = reviewerName == null ? string.Empty : reviewerName.InnerText;

                    var reviewContentNode = helper.GetElementWithAttribute(headerNode, "div", "class", "Normal");

                    var review = reviewContentNode == null ?  string.Empty : reviewContentNode.InnerText;

                    //this code is not working for getting critics rating, hence comentted              
                    /*var reviewRatingNode = helper.GetElementWithAttribute(bodyNode, "div", "id", "sshow");
                    var ratingNode = helper.GetElementWithAttribute(reviewRatingNode, "td", "class", "flmcast");
                    var rates = helper.GetElementWithAttribute(ratingNode, "span", "id", "stp");
                    var rate = rates == null ? string.Empty : rates.InnerText;
                    int found = rate.IndexOf(",");
                    rate = rate.Substring(0, found);
                    rate = rate.Substring(rate.Length - 3);
                    if(!rate.Contains(".")){
                        rate = rate.Substring(rate.Length - 1);
                    }

                    rate = (Double.Parse(rate) * 2).ToString();*/

                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Affiliation = affiliation.Trim();
                    re.Review = review.Trim();
                    //re.ReviewerName = "Gaurav Malani";
                    re.ReviewerName = reviewName;
                    re.ReviewerRating = rate.ToString();
                    re.MyScore = string.Empty;
                    re.JsonString = string.Empty;
                    return re;
                }
            }

            return null;
        }

      
    }
}
