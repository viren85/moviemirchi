
namespace SmartMonkey.UDT
{
    using System;

    public class Url
    {
        public string Base { get; set; }

        public string Part { get; set; }

        public Url()
        {

        }

        public Url(string p_b, string p_u)
        {
            this.Base = p_b;
            this.Part = p_u;
        }
    }
}
