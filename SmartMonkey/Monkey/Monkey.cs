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
        private static object lockObject = new object();
        private static HttpClient client = new HttpClient();

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
                this.ValidationList
                    .Select(test =>
                        client.GetAsync(test.Url.Base.TrimEnd('/') + '/' + test.Url.Part.TrimStart('/'))
                        .IgnoreExceptions(e =>
                        {
                            lock (lockObject)
                            {
                                var messages = e.InnerExceptions
                                    .Select(a => a.InnerException)
                                    .Where(b => b.InnerException == null)
                                    .Select(c => c.Message);

                                var color = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Exception: {1}{0}{2}",
                                    Environment.NewLine,
                                    test.Url.Part,
                                    string.Join(" | ", messages));
                                Console.ForegroundColor = color;
                            }
                        })
                        .ContinueWith(taskResponse =>
                        {
                            if (taskResponse.Status == TaskStatus.Faulted || taskResponse.Status == TaskStatus.Canceled)
                            {
                            }
                            else
                            {
                                HttpResponseMessage response = taskResponse.Result;
                                string data = response.Content.ReadAsStringAsync().Result;
                                var statusCode = response.StatusCode;

                                if (statusCode == HttpStatusCode.OK)
                                {
                                    test.Data = data;
                                    this.JumpStyle(test);
                                }
                                else
                                {
                                    test.Data = statusCode + " - " + response.ReasonPhrase;
                                    test.Result = false;
                                    test.ReportResult();
                                }
                            }
                        }));

            Task.WaitAll(tasks.ToArray());

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Stopping - {0}", this.Name);
        }
    }
}
