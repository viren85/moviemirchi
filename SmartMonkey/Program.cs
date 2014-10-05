using SmartMonkey.UDT;
using System;
using System.Linq;

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

            var hits = new SetupHitMonkey(apiurl, weburl);
            var cache = new SetupCacheMonkey(apiurl, weburl);

            var monkeys = new IMonkey[] { 
                hits.HitProductAPIs(),
                cache.CacheMoviePage(),
                cache.CacheArtistPage(),
            };
            foreach (IMonkey monkey in monkeys)
            {
                monkey.Jump();
            }

            ResultCollection.Stats();
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
