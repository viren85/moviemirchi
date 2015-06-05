using MvcWebRole1.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWebRole1.Models.Page
{
    public class HomePage
    {
        public IEnumerable<Movie> UpcomingMovies { get; set; }
        public IEnumerable<Movie> NowPlayingMovies { get; set; }
    
    }
}