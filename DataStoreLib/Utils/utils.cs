using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Utils
{
    public class utils
    {
        public static List<string> GetListFromCommaSeparatedString(string str)
        {
            var retList = new List<string>();

            if (!string.IsNullOrWhiteSpace(str))
            {
                string[] splitStr = str.Split(',');
                foreach (var token in splitStr)
                {
                    retList.Add(token);
                }
            }

            return retList;
        }

        public static string GetCommaSeparatedStringFromList(List<string> actors)
        {
            Debug.Assert(actors != null);

            StringBuilder sb = new StringBuilder("");

            for (int iter = 0; iter < actors.Count; iter++)
            {
                sb.Append(actors[iter]);

                if (iter != actors.Count - 1)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }
    }
}
