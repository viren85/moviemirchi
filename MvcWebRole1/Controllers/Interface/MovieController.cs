using MvcWebRole1.Models.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcWebRole1.Controllers.Interface
{
    public class MovieController : ApiController
    {
        public IEnumerable<Movie> GetUpcoming()
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(@"http://127.0.0.1:8081/api/RazorMovies?type=upcoming");
                var movies = JsonConvert.DeserializeObject<List<Movie>>(json);
                return movies;
            }
        }

        public IEnumerable<Movie> GetNowPlaying()
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(@"http://127.0.0.1:8081/api/RazorMovies?type=current");
                var movies = JsonConvert.DeserializeObject<List<Movie>>(json);
                return movies;
            }
        }
    }
}
