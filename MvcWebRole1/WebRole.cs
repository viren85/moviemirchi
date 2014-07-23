using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Diagnostics;

namespace MvcWebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            SetupLuceneIndexes();

            var result = base.OnStart();

            RunSmartMonkey();

            return result;
        }

        private void SetupLuceneIndexes()
        {
            //// TODO: Implement this method
            ;
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
