using DataStoreLib.Models;
using DataStoreLib.Storage;
using MovieCrawler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;


namespace Crawler
{
    public class MovieCrawler
    {
        private CrawlerHelper helper = new CrawlerHelper();
        string thumbnailPath = string.Empty;
        string moviePageContent = string.Empty;

        /// <summary>
        /// Entry point for the crawler. Pass the URL from source file (XML)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MovieEntity Crawl(string url)
        {
            MovieEntity movie = new MovieEntity();
            TableManager tableMgr = new TableManager();
            thumbnailPath = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    #region Get Movie Page Content
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    moviePageContent = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                    #endregion

                    movie = PopulateMovieDetails(moviePageContent);
                    bool crawlPosters = true;

                    TableManager tblMgr = new TableManager();

                    MovieEntity me = tblMgr.GetMovieByUniqueName(movie.UniqueName);
                    if (me != null && !string.IsNullOrEmpty(me.RowKey))
                    {
                        movie.RowKey = me.RowKey;
                        movie.MovieId = me.MovieId;
                        crawlPosters = false;
                    }

                    //PopulateMovieDetails(ref movie, url, crawlPosters);
                    return movie;
                    ////tableMgr.UpdateMovieById(movie);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while populating the movie details. Url = " + url + ". Error=" + ex.Message);
            }

            return movie;
        }

        private bool PopulateMovieDetails(ref MovieEntity movie, string url, bool isCrawlPosters)
        {
            try
            {
                List<string> poster = new List<string>();
                List<Cast> cast = CrawlCast(url + "fullcredits");
                List<Songs> songs = CrawlSongs(url + "soundtrack");

                if (isCrawlPosters)
                {
                    //poster = CrawlPosters(url + "mediaindex", movie.Name, ref thumbnailPath);
                }

                movie.Casts = JsonConvert.SerializeObject(cast);
                movie.Songs = JsonConvert.SerializeObject(songs);

                if (poster != null && poster.Count > 0)
                {
                    movie.Posters = JsonConvert.SerializeObject(poster);
                }
                else
                {
                    movie.Posters = "[]";
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("An error occurred while populating other bulk movie details. Url = " + url + ". Error=" + e.Message);
                return false;
            }
        }

        private List<Cast> CrawlCast(string url)
        {
            List<Cast> casts = new List<Cast>();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    string data = readStream.ReadToEnd();

                    casts = GetCast(data);

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Url:" + url);
                Debug.WriteLine("Message:" + ex.Message);
            }

            return casts;
        }

        private List<string> CrawlPosters(string url, string movieName, ref string thumbnailPath)
        {
            List<string> posters = new List<string>();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    string data = readStream.ReadToEnd();

                    posters = GetPosters(data, movieName, ref posters, ref thumbnailPath);

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Url:" + url);
                Debug.WriteLine("Message:" + ex.Message);
            }

            return posters;
        }

        private List<Songs> CrawlSongs(string url)
        {
            List<Songs> songs = new List<Songs>();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    string data = readStream.ReadToEnd();

                    songs = GetSongs(data);

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Url:" + url);
                Debug.WriteLine("Message:" + ex.Message);
            }

            return songs;
        }

        private MovieEntity PopulateMovieDetails(string html)
        {
            ImdbCrawler imdb = new ImdbCrawler();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                if (bodyNode == null)
                {
                    Console.WriteLine("body node is null");
                }
                else
                {
                    return imdb.GetMovieDetails(bodyNode);
                }
            }

            return null;
        }

        private List<Cast> GetCast(string html)
        {
            ImdbCrawler imdb = new ImdbCrawler();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                if (bodyNode == null)
                {
                    Console.WriteLine("body node is null");
                }
                else
                {
                    return imdb.GetMovieCastDetails(bodyNode);
                    //return imdb.GetMovieDetails(bodyNode);
                }
            }

            return null;
        }

        private List<string> GetPosters(string html, string movieName, ref List<string> posterPath, ref string thumbnailPath)
        {
            ImdbCrawler imdb = new ImdbCrawler();
            posterPath = new List<string>();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                if (bodyNode == null)
                {
                    Console.WriteLine("body node is null");
                }
                else
                {
                    return imdb.GetMoviePosterDetails(bodyNode, movieName, ref posterPath, ref thumbnailPath);
                }
            }

            return null;
        }

        private List<Songs> GetSongs(string html)
        {
            ImdbCrawler imdb = new ImdbCrawler();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                if (bodyNode == null)
                {
                    Console.WriteLine("body node is null");
                }
                else
                {
                    return imdb.GetSongDetails(bodyNode);
                }
            }

            return null;

        }
    }
}