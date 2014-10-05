using System;
using System.Collections.Generic;

namespace SmartMonkey
{
    interface IMonkey
    {
        string Name { get; set; }
        string APIUrl { get; set; }
        string WebUrl { get; set; }
        Action<Test> JumpStyle { get; set; }
        void AddTest(Test test);
        void AddTests(IEnumerable<Test> tests);
        void Jump();
    }
}
