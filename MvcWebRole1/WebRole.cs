
namespace MvcWebRole1
{
    using Microsoft.WindowsAzure.ServiceRuntime;
    using System.Diagnostics;

    public class WebRole : RoleEntryPoint
    {

        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            var result = base.OnStart();

            RunSmartMonkey();

            return result;
        }

        private static void RunSmartMonkey()
        {
            try
            {
                // Run SmartMonkey
                using (Process p = Process.Start("SmartMonkey.exe"))
                {

                }
            }
            catch
            {

            }
        }
    }
}
