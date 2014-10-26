using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudMovie.APIRole.Library
{
    public class TweetFormatter
    {
        public IEnumerable<string> Format(IEnumerable<string> tweets)
        {
            var tweetLines = tweets.SelectMany(t =>
            {
                if (t.Length > 70)
                {
                    var split = t.Split(' ');
                    int length = t.Length;
                    int len = 0;
                    int prevlen = -1;
                    int breakpoint = -1;
                    foreach (var word in split)
                    {
                        prevlen = len;
                        len += word.Length;
                        if (len > 70 && (len - 70) > 4)
                        {
                            breakpoint = prevlen;
                            break;
                        }
                        ++len;
                    }

                    if (breakpoint == -1)
                    {
                        return new string[] { t };
                    }
                    else
                    {
                        var p1 = t.Substring(0, breakpoint);
                        var p2 = t.Substring(breakpoint, t.Length - breakpoint);
                        return new string[] { p1 + "<break here>" + p2 };
                    }
                }
                else
                {
                    return new string[] { t };
                }
            });

            return tweetLines;
        }
    }
}