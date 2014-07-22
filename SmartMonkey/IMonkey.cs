using System.Collections.Generic;

namespace SmartMonkey
{
    interface IMonkey
    {
        string BaseUrl { get; set; }
        void AddTests(IEnumerable<Test> tests);
        void StartJumping();
        void StopJumping();
    }
}
