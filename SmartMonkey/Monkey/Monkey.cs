using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                        string url = test.BaseUrl.TrimEnd('/') + '/' + test.Url.TrimStart('/');

                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = client.GetAsync(url).Result;
                            string data = response.Content.ReadAsStringAsync().Result;
                            var statusCode = response.StatusCode;
                            
                            if (statusCode == HttpStatusCode.OK)
                            {
                                test.Data = data;
                                this.JumpStyle(test);
                            }
                            else
                            {
                                test.ReportResult(false, statusCode + " - " + response.ReasonPhrase);
                            }
                        }
                    }));

            Task.WaitAll(tasks.ToArray());

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Stopping - {0}", this.Name);
        }
    }
}
