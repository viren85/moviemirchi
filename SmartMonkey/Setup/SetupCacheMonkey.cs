using System;
using System.Collections.Generic;
using System.Linq;

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

        private static readonly Func<Test, IEnumerable<string>> cacheMoviePage = test =>
        {
            var movies =
                test.Data.Split(new string[] { "MovieId" }, StringSplitOptions.None).Skip(1)
                .Select(m =>
                    m.Split(new string[] { "UniqueName\\\":\\\"" }, StringSplitOptions.None).Skip(1).First()
                    .Split(new string[] { "\\\"" }, StringSplitOptions.None).First());

            return movies.Select(id => "movie/" + id);
        };

        private static readonly Func<Test, IEnumerable<string>> cacheMovieInfo = test =>
        {
            var movies =
                test.Data.Split(new string[] { "MovieId" }, StringSplitOptions.None).Skip(1)
                .Select(m =>
                    m.Split(new string[] { "UniqueName\\\":\\\"" }, StringSplitOptions.None).Skip(1).First()
                    .Split(new string[] { "\\\"" }, StringSplitOptions.None).First());

            return movies.Select(id => "api/movieinfo?q=" + id);
        };

        private static readonly Func<Test, IEnumerable<string>> cacheArtistPage = test =>
        {
            if (!test.Data.Contains("Actor"))
            {
                Console.WriteLine("Note: No actors for {0}", test.Url);
                return Enumerable.Empty<string>();
            }

            var seg =
                test.Data.Split(new string[] { "Actor" }, StringSplitOptions.None).Take(5);

            var actors = seg
                .Select(s =>
                    (s.Split(new string[] { "\"name\\\\\\\":\\\\\\\"" }, StringSplitOptions.None).LastOrDefault() ?? "")
                    .Split(new string[] { "\\\\\\\"" }, StringSplitOptions.None).FirstOrDefault())
                .Where(s => !String.IsNullOrWhiteSpace(s));

            return actors.Select(a => "artists/" + a.Replace(" ", "-"));
        };

        public IMonkey CacheMoviePage()
        {
            return Setup(cacheMoviePage);
        }

        public IMonkey CacheArtistPage()
        {
            return Setup(cacheMovieInfo, cacheArtistPage);
        }

        private IMonkey Setup(Func<Test, IEnumerable<string>> funcLevel1, Func<Test, IEnumerable<string>> funcLevel2 = null)
        {
            CacheMonkey monkey = new CacheMonkey();
            monkey.Name = "CacheMonkey";
            monkey.APIUrl = this.APIUrl;
            monkey.WebUrl = this.WebUrl;
            monkey.AddTest(new Test()
            {
                Name = "Now playing",
                BaseUrl = this.APIUrl,
                Url = "api/movies?type=current",
                Validate = Test.DefaultValidate(null),
                ScratchLevel1 = funcLevel1,
                ScratchLevel2 = funcLevel2,
            });
            monkey.AddTest(new Test()
            {
                Name = "Upcoming",
                Url = "api/movies?type=upcoming",
                BaseUrl = this.APIUrl,
                Validate = Test.DefaultValidate(null),
                ScratchLevel1 = funcLevel1,
                ScratchLevel2 = funcLevel2,
            });
            return monkey;
        }
    }
}
