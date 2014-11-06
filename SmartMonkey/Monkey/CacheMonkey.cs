using SmartMonkey.UDT;
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
                var urls = test.ScratchLevel1(test);
                if (test.ScratchLevel2 != null)
                {
                    this.InitiateCacheMonkey(test, urls);
                }
                else if (test.ScratchLevel1 != null)
                {
                    this.InitiateHitMonkey(test, urls);
                }
            };
        }

        private void InitiateHitMonkey(Test test, IEnumerable<Url> urls)
        {
            IMonkey monkey = new HitMonkey();
            monkey.Name = "HitMonkey for " + test.Name;
            monkey.APIUrl = this.APIUrl;
            monkey.WebUrl = this.WebUrl;
            monkey.AddTests(
                urls.Select(url =>
                {
                    return new Test()
                    {
                        Name = test.Name + " - " + url.Part,
                        Url = url,
                        Validate = null, //test.Validate,
                    };
                }));
            monkey.Jump();
        }

        private void InitiateCacheMonkey(Test test, IEnumerable<Url> urls)
        {
            CacheMonkey monkey = new CacheMonkey();
            monkey.Name = "CacheMonkey for " + test.Name;
            monkey.APIUrl = this.APIUrl;
            monkey.WebUrl = this.WebUrl;
            monkey.AddTests(
                urls.Select(url =>
                {
                    return new Test()
                    {
                        Name = test.Name + " - " + url.Part,
                        Url = url,
                        Validate = null, //test.Validate,
                        ScratchLevel1 = test.ScratchLevel2,
                        ScratchLevel2 = null,
                    };
                }));
            monkey.Jump();
        }
    }
}
