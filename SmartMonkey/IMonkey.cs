using System.Collections.Generic;

namespace SmartMonkey
{
    interface IMonkey
    {
        string BaseUrl { get; set; }
        void AddTest(Test test);
        void AddTests(IEnumerable<Test> tests);
        void StartJumping();
        void StopJumping();
    }
}
