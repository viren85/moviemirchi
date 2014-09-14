using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartMonkey
{
    class CacheMonkey : Monkey
    {
        public CacheMonkey()
            : base()
        {
            this.JumpStyle = (test) =>
            {
                var movies = test.Scratch(test);
                this.InitiateHitMonkey(test, movies);
            };
        }

        //public override void StartJumping()
        //{
        //    Console.ForegroundColor = ConsoleColor.White;
        //    Console.WriteLine("Starting - {0}", this.Name);

        //    var tasks =
        //        this.ValidationList.Select(test =>
        //            Task.Factory.StartNew(() =>
        //            {
        //                string url = this.BaseUrl + test.Url.TrimStart('/');
        //                var client = new WebClient();
        //                client.DownloadStringCompleted += (sender, e) =>
        //                {
        //                    if (!e.Cancelled && e.Error == null)
        //                    {
        //                        string data = (string)e.Result;
        //                        test.Data = data;
        //                        this.JumpStyle(test);
        //                    }
        //                    else
        //                    {
        //                        test.ReportResult(false, e.Error.HResult + " - " + e.Error.Message);
        //                    }
        //                };

        //                var downloadTask = client.DownloadStringTaskAsync(url);
        //                try
        //                {
        //                    downloadTask.Wait();
        //                }
        //                catch (AggregateException)
        //                {
        //                    // We must have handled and reported this error earlier
        //                }
        //            }));

        //    Task.WaitAll(tasks.ToArray());
        //}

        private void InitiateHitMonkey(Test test, IEnumerable<string> movies)
        {
            IMonkey monkey = new HitMonkey();
            monkey.Name = "HitMonkey";
            monkey.BaseUrl = this.BaseUrl;
            monkey.AddTests(
                movies.Select(movie =>
                {
                    return new Test()
                    {
                        Name = test.Name + " - " + movie,
                        Url = movie,
                        Validate = test.Validate,
                    };
                }));
            monkey.Jump();
        }
    }
}
