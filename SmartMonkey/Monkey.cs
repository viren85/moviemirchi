using System;
using System.Collections.Generic;
using System.Net;

namespace SmartMonkey
{
    public abstract class Monkey : IMonkey
    {
        private string baseurl;
        private List<Test> validationList;

        public string BaseUrl
        {
            get
            {
                return this.baseurl;
            }
            set
            {
                this.baseurl = value.Trim().TrimEnd('/') + '/';
            }
        }

        public Monkey()
        {

        }

        public void AddTests(IEnumerable<Test> tests)
        {
            if (this.validationList == null)
            {
                this.validationList = new List<Test>();
            }

            this.validationList.AddRange(tests);
        }

        public void StartJumping()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Starting");

            foreach (var test in this.validationList)
            {
                string url = this.baseurl + test.Url.TrimStart('/');
                var client = new WebClient();
                client.DownloadStringCompleted += (sender, e) =>
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        string data = (string)e.Result;
                        bool res = test.Validate(data);
                        ReportResult(test, res, data);
                    }
                    else
                    {
                        ReportResult(test, false, e.Error.ToString());
                    }
                };

                var task = client.DownloadStringTaskAsync(url);
                task.Wait();
            }
        }

        public void StopJumping()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Stopping");
        }

        private static void ReportResult(Test test, bool res, string data = "")
        {
            Console.ForegroundColor = res ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
            Console.WriteLine(
                "{0}: {1} - {2}",
                res ? "Passed" : "Failed",
                test.Name,
                test.Url);
        }
    }
}
