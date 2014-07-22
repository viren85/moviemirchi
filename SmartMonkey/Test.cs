using System;

namespace SmartMonkey
{
    public class Test
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Func<string, bool> Validate { get; set; }

        internal static Func<string, bool> DefaultValidate =
            (data) =>
            {
                if (string.IsNullOrWhiteSpace(data))
                {
                    return false;
                }

                data = data.ToLower();
                if (data.Contains("\"Error\":"))
                {
                    return false;
                }

                return true;
            };
    }
}
