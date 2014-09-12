using System.Collections.Generic;
using System.Linq;

namespace SmartMonkey
{
    class Program
    {
        static void Main(string[] args)
        {
            var dict = new Dictionary<string, string>()
            {
                {"Now playing","api/Movies?type=current"},
                {"Upcoming","api/Movies?type=upcoming"},
                {"Twitter","api/Twitter?start=0&page=20"},
                {"Popular","api/Popular?type=all"},
                {"News","api/News?start=0&page=20"},
                {"Movie info","api/MovieInfo?q=humshakals"},
                {"Twitter movie","api/Twitter?start=0&page=20&type=movie&name=humshakals"},
                {"Twitter Artist","api/Twitter?start=0&page=20&type=artist&name=Bipasha-Basu"},
                {"Artist bio","api/ArtistMovies?type=bio&name=Bipasha%20Basu"},
                {"Artist movies","api/ArtistMovies?type=movie&name=Bipasha%20Basu"},
                {"Search","AutoComplete/AutoCompleteMovies?query=deepika"},
                {"Genre movies","api/GenreMovies?type=Romance"},
                {"Reviewer","api/ReviewerInfo?name=Anupama%20Chopra"}
            };

            IMonkey monkey = new SmartMonkey();
            monkey.BaseUrl = @"http://451a26608c494838ae8cb17189110428.cloudapp.net/";
            monkey.AddTests(
                dict.Select(item =>
                {
                    return new Test()
                    {
                        Name = item.Key,
                        Url = item.Value,
                        Validate = Test.DefaultValidate
                    };
                }));

            monkey.StartJumping();
            monkey.StopJumping();
        }
    }
}
