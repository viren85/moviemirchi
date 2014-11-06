using SmartMonkey.UDT;
using System;
using System.Collections.Generic;

namespace SmartMonkey
{
    public class Test
    {
        private static object lockObject = new object();

        public string Name { get; set; }
        public Url Url { get; set; }
        public string Data { get; set; }
        public bool Result { get; set; }

        public Func<string, bool> Validate { get; set; }
        public Func<Test, IEnumerable<Url>> ScratchLevel1 { get; set; }
        public Func<Test, IEnumerable<Url>> ScratchLevel2 { get; set; }

        internal static Func<string, Func<string, bool>> DefaultValidate =
            (errStr) =>
            {
                return new Func<string, bool>((data) =>
                {
                    if (string.IsNullOrWhiteSpace(data))
                    {
                        return false;
                    }

                    data = data.ToLower();
                    errStr = string.IsNullOrWhiteSpace(errStr) ? "\\\"status\\\":\\\"error" : errStr;

                    if (data.Contains(errStr))
                    {
                        return false;
                    }

                    return true;
                });
            };

        public void ReportResult()
        {
            string result = this.Result ? "Passed" : "Failed";
            ResultCollection.Add(new TestResult
            {
                Name = this.Name,
                Url = this.Url.Part,
                Result = this.Result,
            });

            lock (lockObject)
            {
                Console.ForegroundColor = this.Result ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                Console.WriteLine(
                    "{0}: {1} - {2}",
                    result,
                    this.Name,
                    this.Url.Part);
                if (!this.Result && !string.IsNullOrWhiteSpace(this.Data))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\t{0}", this.Data);
                }
            }
        }
    }
}
