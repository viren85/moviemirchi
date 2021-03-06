﻿using DataStoreLib.Models;
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
    public class Rajasen
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
                Debug.WriteLine(string.Format("Exception occored while getting reviews (rajasen.com), message= {0}", ex.Message));
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
                    var headerNode = helper.GetElementWithAttribute(bodyNode, "h1", "class", "entry-title");
                    HtmlNode header = headerNode.Element("a");
                    var head = header == null ? headerNode.InnerHtml : header.InnerText;
                    head = head.Replace("&#8217;", "'").Replace("&nbsp;", " ").Trim();
                    //var reviewerNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "blbox");
                    // var reviewerMiddleNode = helper.GetElementWithAttribute(reviewerNode, "div", "class", "blbox");
                    /*
                    var reviewerName = helper.GetElementWithAttribute(bodyNode, "div", "class", "story-date");
                    HtmlNode node = reviewerName.Element("a");
                    var reviewName = node == null ? reviewerName.InnerHtml : node.InnerText;
                    */
                    var reviewContentNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "entry entry-content");
                    HtmlNodeCollection nodes = reviewContentNode.SelectNodes("p");
                    var review = string.Empty;
                    var reviewerRating = string.Empty;
                    foreach (var ratingNode in nodes)
                    {
                        review += ratingNode.InnerText;

                        if (ratingNode.InnerText.ToLower().Contains("rating"))
                        {
                            try
                            { 
                                rating = ratingNode.InnerText.Replace("Rating: ", "").Replace("stars", "").Trim();
                                rating = (Decimal.Parse(rating) * 2).ToString();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Affiliation = affiliation.Trim();
                    re.Review = review.Trim();
                    re.ReviewerName = "Raja Sen";
                    re.ReviewerRating = rating;
                    re.MyScore = string.Empty;
                    re.JsonString = string.Empty;
                    return re;
                }
            }

            return null;
        }
    }
}
