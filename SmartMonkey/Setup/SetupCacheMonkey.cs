using SmartMonkey.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonkey
{
    internal class SetupCacheMonkey
    {
        private string APIUrl { get; set; }
        private string WebUrl { get; set; }

        public SetupCacheMonkey(string apiurl, string weburl)
        {
            this.APIUrl = apiurl;
            this.WebUrl = weburl;
        }

        private IEnumerable<Url> cacheMoviePage(Test test)
        {
            var movies =
                test.Data.Split(new string[] { "MovieId" }, StringSplitOptions.None).Skip(1)
                .Select(m =>
                    m.Split(new string[] { "UniqueName\\\":\\\"" }, StringSplitOptions.None).Skip(1).First()
                    .Split(new string[] { "\\\"" }, StringSplitOptions.None).First());

            return movies.Select(id => new Url(this.WebUrl, "movie/" + id));
        }

        private IEnumerable<Url> cacheMovieInfo(Test test)
        {
            var movies =
                test.Data.Split(new string[] { "MovieId" }, StringSplitOptions.None).Skip(1)
                .Select(m =>
                    m.Split(new string[] { "UniqueName\\\":\\\"" }, StringSplitOptions.None).Skip(1).First()
                    .Split(new string[] { "\\\"" }, StringSplitOptions.None).First());

            return movies.Select(id => new Url(this.APIUrl, "api/movieinfo?q=" + id));
        }

        private IEnumerable<Url> cacheArtistPage(Test test)
        {
            if (!test.Data.Contains("Actor"))
            {
                Console.WriteLine("Note: No actors for {0}", test.Url.Part);
                return Enumerable.Empty<Url>();
            }

            var seg =
                test.Data.Split(new string[] { "Actor" }, StringSplitOptions.None).Take(5);

            var actors = seg
                .Select(s =>
                    (s.Split(new string[] { "\"name\\\\\\\":\\\\\\\"" }, StringSplitOptions.None).LastOrDefault() ?? "")
                    .Split(new string[] { "\\\\\\\"" }, StringSplitOptions.None).FirstOrDefault())
                .Where(s => !String.IsNullOrWhiteSpace(s));

            return actors.SelectMany(a => new Url[] {
                new Url(this.WebUrl, "artists/" + a.Replace(" ", "-")),
                new Url(this.APIUrl, "api/ArtistMovies?type=bio&name=" + a.Replace(" ", "%20")),
                new Url(this.APIUrl, "api/ArtistMovies?type=movie&name=" + a.Replace(" ", "%20")),
            });
        }

        public IMonkey CacheMoviePage()
        {
            return Setup(this.cacheMoviePage);
        }

        public IMonkey CacheArtistPage()
        {
            return Setup(this.cacheMovieInfo, this.cacheArtistPage);
        }

        private IMonkey Setup(Func<Test, IEnumerable<Url>> funcLevel1, Func<Test, IEnumerable<Url>> funcLevel2 = null)
        {
            CacheMonkey monkey = new CacheMonkey();
            monkey.Name = "CacheMonkey";
            monkey.APIUrl = this.APIUrl;
            monkey.WebUrl = this.WebUrl;
            monkey.AddTest(new Test()
            {
                Name = "Now playing",
                Url = new Url(this.APIUrl, "api/movies?type=current"),
                Validate = Test.DefaultValidate(null),
                ScratchLevel1 = funcLevel1,
                ScratchLevel2 = funcLevel2,
            });
            monkey.AddTest(new Test()
            {
                Name = "Upcoming",
                Url = new Url(this.APIUrl, "api/movies?type=upcoming"),
                Validate = Test.DefaultValidate(null),
                ScratchLevel1 = funcLevel1,
                ScratchLevel2 = funcLevel2,
            });
            return monkey;
        }
    }
}
