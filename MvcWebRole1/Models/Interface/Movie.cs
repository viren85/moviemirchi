using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWebRole1.Models.Interface
{
    public class Movie
    {
        public string Name { get; set; }
        public string UniqueName { get; set; }
        public string Synopsis { get; set; }
        public string MyScore { get; set; }
        public string Month { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string Poster { get; set; }
        public bool IsTrailer { get; set; }
        public bool IsSong { get; set; }
        public bool IsPoster { get; set; }
    }
}