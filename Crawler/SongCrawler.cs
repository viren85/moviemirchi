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
    public class SongCrawler
    {
        private CrawlerHelper helper = new CrawlerHelper();

        string songPageContent = string.Empty;

        /// <summary>
        /// Entry point for the crawler. Pass the URL from source file (XML)
        /// </summary>
        /// <param name="url"></param>
        public List<Songs> Crawl(string url)
        {
            MovieEntity movie = new MovieEntity();
            TableManager tableMgr = new TableManager();

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

                    songPageContent = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                    #endregion
                }

                return PopulateSongDetails(songPageContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while populating the movie details. Url = " + url + ". Error=" + ex.Message);
            }

            return null;
        }

        private List<Songs> PopulateSongDetails(string html)
        {
            List<Songs> crawledSongList = new List<Songs>();
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
                    var songNodeList = GetSongNodeList(bodyNode);

                    var songs = songNodeList.ChildNodes;

                    foreach (HtmlAgilityPack.HtmlNode song in songs)
                    {
                        var songItem = song.ChildNodes["strong"];//.ch.ch.GetElementWithAttribute(song, "li", "class", "song-wrap");
                        if (songItem != null && !string.IsNullOrEmpty(songItem.InnerText.Trim()))
                        {
                            Songs songObj = new Songs();
                            songObj.Composed = string.Empty;
                            songObj.Courtsey = string.Empty;
                            songObj.Lyrics = string.Empty;
                            songObj.Performer = string.Empty;
                            songObj.Recite = string.Empty;
                            songObj.SongTitle = songItem.InnerText;
                            crawledSongList.Add(songObj);
                        }
                    }
                }
            }

            return crawledSongList;
        }

        private HtmlAgilityPack.HtmlNode GetSongNodeList(HtmlAgilityPack.HtmlNode body)
        {
            var contentNode = helper.GetElementWithAttribute(body, "div", "class", "section");
            //var songList = contentNode.ChildNodes;// helper.GetElementWithAttribute(contentNode, "p", "class", "row");
            return contentNode;
            //var song = helper.GetElementWithAttribute(songList, "li", "class", "song-wrap");
            //return helper.GetElementWithAttribute(song, "span", "class", "main");
        }

        private string GetArtistName(HtmlAgilityPack.HtmlNode node)
        {
            string artistNames = string.Empty;

            var artists = helper.GetElementWithAttribute(node, "em", "class", "artist");

            HtmlAgilityPack.HtmlNodeCollection artistsList = artists.ChildNodes;// helper.GetElementWithAttribute(artists, "a", "class", "");

            foreach (HtmlAgilityPack.HtmlNode artist in artistsList)
            {
                artistNames += artist.InnerText + ",";
            }

            return artistNames;
        }

        private string GetSongTitle(HtmlAgilityPack.HtmlNode node)
        {
            var songTitleNode = helper.GetElementWithAttribute(node, "a", "class", "title");
            return (songTitleNode == null) ? string.Empty : songTitleNode.InnerText;
        }

    }
}