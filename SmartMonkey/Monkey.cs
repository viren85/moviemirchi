using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SmartMonkey
{
    public abstract class Monkey : IMonkey
    {
        public string Name { get; set; }
        public string APIUrl { get; set; }
        public string WebUrl { get; set; }
        public Action<Test> JumpStyle { get; set; }

        protected List<Test> ValidationList { get; private set; }

        public Monkey()
        {
            this.ValidationList = new List<Test>();
        }

        public void AddTest(Test test)
        {
            this.ValidationList.Add(test);
        }

        public void AddTests(IEnumerable<Test> tests)
        {
            this.ValidationList.AddRange(tests);
        }

        public void Jump()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Starting - {0}", this.Name);

            var tasks =
                this.ValidationList.Select(test =>
                    Task.Factory.StartNew(() =>
                    {
                        string url = this.APIUrl + test.Url.TrimStart('/');
                        var client = new WebClient();
                        client.DownloadStringCompleted += (sender, e) =>
                        {
                            if (!e.Cancelled && e.Error == null)
                            {
                                string data = (string)e.Result;
                                test.Data = data;
                                this.JumpStyle(test);
                            }
                            else
                            {
                                test.ReportResult(false, e.Error.HResult + " - " + e.Error.Message);
                            }
                        };

                        var downloadTask = client.DownloadStringTaskAsync(url);
                        try
                        {
                            downloadTask.Wait();
                        }
                        catch (AggregateException)
                        {
                            // We must have handled and reported this error earlier
                        }
                    }));

            Task.WaitAll(tasks.ToArray());

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Stopping - {0}", this.Name);
        }
    }
}
