using System;
using System.Collections.Generic;

namespace SmartMonkey
{
    public class Test
    {
        private static object lockObject = new object();

        public string Name { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }
        public Func<string, bool> Validate { get; set; }

        public Func<Test, IEnumerable<string>> ScratchLevel1 { get; set; }
        public Func<Test, IEnumerable<string>> ScratchLevel2 { get; set; }

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

        public void ReportResult(bool res, string data = "")
        {
            lock (lockObject)
            {
                Console.ForegroundColor = res ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                Console.WriteLine(
                    "{0}: {1} - {2}",
                    res ? "Passed" : "Failed",
                    this.Name,
                    this.Url);
                if (!res && !string.IsNullOrWhiteSpace(data))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\t{0}", data);
                }
            }
        }
    }
}
