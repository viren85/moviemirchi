using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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
