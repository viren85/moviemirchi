using System.Collections.Generic;
using System.Linq;

namespace SmartMonkey
{
    internal class SetupHitMonkey
    {
        private string APIUrl { get; set; }
        private string WebUrl { get; set; }

        public SetupHitMonkey(string apiurl, string weburl)
        {
            this.APIUrl = apiurl;
            this.WebUrl = weburl;
        }

        public IMonkey HitProductAPIs()
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
            monkey.APIUrl = this.APIUrl;
            monkey.WebUrl = this.WebUrl;
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
    }
}
