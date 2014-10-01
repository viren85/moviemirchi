using System;
using System.Collections.Generic;
using System.Linq;
using Validator = System.Func<string, string, bool>;

namespace SmartMonkey
{
    class Program
    {
        private const string APIUrl = @"http://96d1f263dff744ddab4f493b9ac935e5.cloudapp.net:8081/";
        private const string WebUrl = @"http://96d1f263dff744ddab4f493b9ac935e5.cloudapp.net:80/";

        static void Main(string[] args)
        {
            string apiurl = GetURL(args, 0, APIUrl);
            string weburl = GetURL(args, 1, WebUrl);
            Console.WriteLine("APIUrl is {1}{0}WebUrl is {2}{0}", Environment.NewLine, apiurl, weburl);

            var monkeys = new IMonkey[] { 
                SetupHitMonkey(apiurl, weburl), 
                SetupCacheMonkey(apiurl, weburl),
            };
            foreach (IMonkey monkey in monkeys)
            {
                monkey.Jump();
            }
        }

        private static IMonkey SetupCacheMonkey(string apiurl, string weburl)
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
            monkey.APIUrl = apiurl;
            monkey.WebUrl = weburl;
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

        private static IMonkey SetupHitMonkey(string apiurl, string weburl)
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
            monkey.APIUrl = apiurl;
            monkey.WebUrl = weburl;
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
            monkey.AddTests(
                new string[] {
                    "api/AutoComplete?query=deepika",
                    "api/AutoComplete?query=chennai",
                    "api/AutoComplete?query=leela",
                    "api/AutoComplete?query=ali",
                    "api/AutoComplete?query=masand",
                    "api/AutoComplete?query=sonam%20kapoor",
                    "api/AutoComplete?query=drama",
                }
                .Select(u => new Test()
                {
                    Name = "Search",
                    Url = u,
                    Validate = Test.DefaultValidate("\"id\":null"),
                }));

            return monkey;
        }

        private static string GetURL(string[] args, int index, string defaultUrl)
        {
            string url = null;
            if (args != null && args.Length > index)
            {
                url = args[index];
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                url = defaultUrl;
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
