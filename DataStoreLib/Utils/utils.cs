
namespace DataStoreLib.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public static class Utils
    {
        public static IEnumerable<string> GetListFromCommaSeparatedString(string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return source.Split(',');
            }

            return Enumerable.Empty<string>();
        }

        public static string GetCommaSeparatedStringFromList(IEnumerable<string> source)
        {
            Debug.Assert(source != null);

            return string.Join(",", source);
        }

        public static DateTime GetBornDate(string bornStr, out string birthDateStr)
        {
            DateTime bornDate = DateTime.Now;
            birthDateStr = "";
            try
            {
                if (bornStr.ToLower().Contains("january"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("january");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("february"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("february");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("march"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("march");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("april"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("april");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("may"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("may");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("june"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("june");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("july"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("july");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("august"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("august");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("september"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("september");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("october"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("october");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("november"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("november");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
                else if (bornStr.ToLower().Contains("december"))
                {
                    int bIndex = bornStr.ToLower().IndexOf("december");

                    string bDate = bornStr.Substring(bIndex, 15);

                    if (bDate.Contains(","))
                    {
                        birthDateStr = bDate = bDate.Remove(bDate.LastIndexOf(","));
                    }

                    bornDate = DateTime.Parse(bDate + " " + DateTime.Now.Year);
                }
            }
            catch (Exception ex)
            {
                
            }

            return bornDate;
        }
    }
}
