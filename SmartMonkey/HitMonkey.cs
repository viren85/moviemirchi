
namespace SmartMonkey
{
    public class HitMonkey : Monkey
    {
        public HitMonkey()
            : base()
        {
            this.JumpStyle = (test) =>
            {
                bool res = test.Validate(test.Data);
                test.ReportResult(res, test.Data);
            };
        }
    }
}
