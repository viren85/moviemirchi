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

            IMonkey monkey = SetupSmartMonkey(url);
            monkey.StartJumping();
            monkey.StopJumping();
        }

        private static IMonkey SetupSmartMonkey(string url)
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

            IMonkey monkey = new SmartMonkey();
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
