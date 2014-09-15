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

        private void InitiateHitMonkey(Test test, IEnumerable<string> movies)
        {
            IMonkey monkey = new HitMonkey();
            monkey.Name = "HitMonkey for " + test.Name;
            monkey.BaseUrl = this.BaseUrl;
            monkey.AddTests(
                movies.Select(movie =>
                {
                    return new Test()
                    {
                        Name = test.Name + " - " + movie,
                        Url = movie,
                        Validate = null, //test.Validate,
                    };
                }));
            monkey.Jump();
        }
    }
}
