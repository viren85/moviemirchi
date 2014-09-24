using DataStoreLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudMovie.APIRole.UDT
{
    public class MovieInfo
    {
        public MovieEntity Movie { get; set; }
        public List<ReviewEntity> MovieReviews { get; set; }
        public List<MovieEntity> MoviesList { get; set; }
        public string movieId { get; set; }
        public string name { get; set; }
        public PosterInfo poster { get; set; }
        public Rating rating { get; set; }
        public Info info { get; set; }
        public List<Review> reviews { get; set; }
    }

    public class PosterInfo
    {
        public int height { get; set; }
        public int width { get; set; }
        public string url { get; set; }
    }

    public class Rating
    {
        public int system { get; set; }
        public int critic { get; set; }
        public string hot { get; set; }
    }

    public class Info
    {
        public string synopsis { get; set; }
        public List<Cast> cast { get; set; }
        public Stats stats { get; set; }

        public Multimedia multimedia { get; set; }
    }

    public class Cast
    {
        public string name { get; set; }
        public string charactername { get; set; }
        public PosterInfo image { get; set; }
        public string role { get; set; }
    }

    public class Stats
    {
        public string budget { get; set; }
        public string boxoffice { get; set; }
    }

    public class Multimedia
    {
        public List<SongTrailer> songs { get; set; }
        public List<SongTrailer> trailers { get; set; }
        public List<Picture> pics { get; set; }
    }

    public class SongTrailer
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Picture
    {
        public string caption { get; set; }
        public PosterInfo pics { get; set; }
    }

    public class Review
    {
        public string name { get; set; }
        public Rating rating { get; set; }
        public string summary { get; set; }
        public string outlink { get; set; }
    }
}