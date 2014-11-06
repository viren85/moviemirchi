using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SmartMonkey.UDT
{
    internal abstract class ResultCollection
    {
        private static ConcurrentBag<TestResult> list = new ConcurrentBag<TestResult>();

        public static ConcurrentBag<TestResult> Results
        {
            get
            {
                return list;
            }
        }

        public static void Add(TestResult item)
        {
            list.Add(item);
        }

        public static int Count
        {
            get
            {
                return list.Count;
            }
        }

        public static void Stats()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(
                "{0}Stats: Total={1} Passed={2} Failed={3}{0}",
                Environment.NewLine,
                list.Count,
                list.Count(r => r.Result),
                list.Count(r => !r.Result)
            );
        }

        public static void GenerateSitemap(string root, string filePath, IEnumerable<Url> seedUrl)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("filePath is not valid");
            }

            // Add the urls here
            var seed = seedUrl ?? Enumerable.Empty<Url>();
            var allUrls = seed.Concat(list.Where(u => u.Result).Select(u => new Url("", u.Url)));
            var properUrls = allUrls
                .Where(u => !u.Part.StartsWith("api/"))
                .Select(u => root + u.Part);
            var urls = properUrls.Distinct();

            XNamespace goog = "http://www.google.com/schemas/sitemap/0.9";
            var xdoc = new XDocument(
                new XElement(goog + "urlset",
                    urls.Select(url =>
                        new XElement(goog + "url",
                            new XElement(goog + "loc", url)
                    ))
                )
            );

            try
            {
                xdoc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not save Sitemap at {1}{0}Exception: {2}{0}"
                    , Environment.NewLine
                    , filePath
                    , ex);
            }
        }

        public static void SendMail()
        {
            if (list.Any(r => !r.Result))
            {
                // We have atleast one test that has failed
                // TODO: Send mail :)
            }
        }
    }
}
