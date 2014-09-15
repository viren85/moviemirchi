using System;
using System.Collections.Generic;
using System.Linq;
using Validator = System.Func<string, string, bool>;

namespace SmartMonkey
{
    class Program
    {
        private const string BaseUrl = @"http://451a26608c494838ae8cb17189110428.cloudapp.net/";

        static void Main(string[] args)
        {
            string url = GetBaseURL(args);

            var monkeys = new IMonkey[] { 
                SetupHitMonkey(url), 
                SetupCacheMonkey(url),
            };
            foreach (IMonkey monkey in monkeys)
            {
                monkey.Jump();
            }
        }

        private static IMonkey SetupCacheMonkey(string url)
        {
            System.Func<Test, IEnumerable<string>> cacheMovieInfo = test =>
                {
                    var movies =
                        test.Data.Split(new string[] { "MovieId" }, StringSplitOptions.None).Skip(1)
                        .Select(m =>
                            m.Split(new string[] { "UniqueName\\\":\\\"" }, StringSplitOptions.None).Skip(1).First()
                            .Split(new string[] { "\\\"" }, StringSplitOptions.None).First());

                    return movies.Select(id => "movie/" + id);
                };

            CacheMonkey monkey = new CacheMonkey();
            monkey.Name = "CacheMonkey";
            monkey.BaseUrl = url;
            monkey.AddTest(new Test()
            {
                Name = "Now playing",
                Url = "api/movies?type=current",
                Validate = Test.DefaultValidate(null),
                Scratch = cacheMovieInfo,
            });
            monkey.AddTest(new Test()
            {
                Name = "Upcoming",
                Url = "api/movies?type=upcoming",
                Validate = Test.DefaultValidate(null),
                Scratch = cacheMovieInfo,
            });
            return monkey;
        }

        private static IMonkey SetupHitMonkey(string url)
        {
            var dict = new Dictionary<string, string>()
            {
                {"Now playing", "api/movies?type=current"},
                {"Upcoming", "api/movies?type=upcoming"},
                {"Twitter", "api/twitter?start=0&page=20"},
                {"Popular", "api/popular?type=all"},
                {"News", "api/news?start=0&page=20"},
                {"Movie info", "api/movieinfo?q=humshakals"},
                {"Twitter movie", "api/twitter?start=0&page=20&type=movie&name=humshakals"},
                {"Twitter Artist", "api/twitter?start=0&page=20&type=artist&name=Bipasha-Basu"},
                {"Artist bio", "api/artistmovies?type=bio&name=Bipasha%20Basu"},
                {"Artist movies", "api/artistmovies?type=movie&name=Bipasha%20Basu"},
                {"Genre movies", "api/genremovies?type=Romance"},
                {"Reviewer", "api/reviewerinfo?name=Anupama%20Chopra"},
            };

            IMonkey monkey = new HitMonkey();
            monkey.Name = "HitMonkey";
            monkey.BaseUrl = url;
            monkey.AddTests(
                dict.Select(item =>
                {
                    return new Test()
                    {
                        Name = item.Key,
                        Url = item.Value,
                        Validate = Test.DefaultValidate(null),
                    };
                }));
            monkey.AddTest(new Test()
            {
                Name = "Search",
                Url = "autocomplete/autocompletemovies?query=deepika",
                Validate = Test.DefaultValidate("\"id\":null"),
            });

            return monkey;
        }

        private static string GetBaseURL(string[] args)
        {
            string url = null;
            if (args != null && args.Length > 0)
            {
                url = args[0];
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                url = BaseUrl;
            }

            url = url.Trim();

            if (!url.EndsWith("/"))
            {
                url = url + "/";
            }

            return url;
        }
    }
}
