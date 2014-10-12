using SmartMonkey.UDT;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartMonkey
{
    class Program
    {
        public const string APIUrl = @"http://96d1f263dff744ddab4f493b9ac935e5.cloudapp.net:8081/";
        public const string WebUrl = @"http://moviemirchi.co.in/";
        public static readonly IEnumerable<string> SeedUrl =
            // Add urls here to make them appear in sitemap
                new string[] {
                    "",
                    "home/about",
                }.AsEnumerable();

        static void Main(string[] args)
        {
            string apiurl = GetURL(args, 0, APIUrl);
            string weburl = GetURL(args, 1, WebUrl);
            Console.WriteLine("APIUrl is {1}{0}WebUrl is {2}{0}", Environment.NewLine, apiurl, weburl);

            var hits = new SetupHitMonkey(apiurl, weburl);
            var cache = new SetupCacheMonkey(apiurl, weburl);

            var monkeys = new IMonkey[] {
                hits.HitProductAPIs(),
                cache.CacheMoviePage(),
                cache.CacheArtistPage(),
                hits.HitReviewerPage(),
                hits.HitGenrePage(),
            };
            foreach (IMonkey monkey in monkeys)
            {
                monkey.Jump();
            }

            ResultCollection.Stats();
            ResultCollection.GenerateSitemap(
                WebUrl,
                filePath: "sitemap.xml",
                seedUrl: SeedUrl
            );
            ResultCollection.SendMail();
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
