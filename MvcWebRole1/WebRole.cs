using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Diagnostics;
using System.IO;

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
            try
            {
                var separator = new char[] { '_' };
                var currentDir = System.Configuration.ConfigurationManager.AppSettings["ImagePath"];
                var luceneDir = Path.Combine(currentDir, "lucene_index");
                if (Directory.Exists(luceneDir))
                {
                    foreach (var file in Directory.EnumerateFiles(luceneDir))
                    {
                        try
                        {
                            var name = Path.GetFileName(file);
                            if (name.ToLowerInvariant().Contains("segment"))
                            {
                                name = name.TrimStart(separator);
                            }
                            else if (name.ToLowerInvariant().Contains(".cfs"))
                            {
                                name = name.TrimEnd(separator);
                            }

                            var newFile = Path.Combine(
                                Path.GetDirectoryName(file),
                                name);
                            File.Move(file, newFile);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {

            }
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
