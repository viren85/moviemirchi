
namespace DataStoreLib.Utils
{
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
    }
}
