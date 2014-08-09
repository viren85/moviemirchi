
namespace MvcWebRole2
{
    using Microsoft.Web.Administration;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    public class WebRole : RoleEntryPoint
    {
        // Update this if you are running locally
        public static string VirtualDirPath = @"C:\Users\bash\Documents\GitHub\moviemirchi\qiouwpeiqwokelwqej";

        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            var path = GetVirtualDirPath();
            ConfigurationManager.AppSettings["ImagePath"] = WebRole.VirtualDirPath = path;

            SetupLuceneIndexes(path);

            return base.OnStart();
        }

        private string GetVirtualDirPath()
        {
            try
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    var paths =
                        serverManager.Sites
                             .SelectMany(site => site.Applications
                                 .Where(a => a.Path.Contains(RoleEnvironment.CurrentRoleInstance.Role.Name))
                                 .SelectMany(ar => ar.VirtualDirectories
                                     .Select(vr => vr.PhysicalPath)))
                             .FirstOrDefault();
                    return paths ?? WebRole.VirtualDirPath;
                }
            }
            catch { ;}

            return WebRole.VirtualDirPath;
        }

        private void SetupLuceneIndexes(string destDir)
        {
            try
            {
                var separator = new char[] { '_' };
                var luceneDir = Path.Combine(destDir, "lucene_index");
                if (Directory.Exists(luceneDir))
                {
                    foreach (var file in Directory.EnumerateFiles(luceneDir))
                    {
                        try
                        {
                            var name = Path.GetFileName(file);
                            var lname = name.ToLowerInvariant();
                            if (lname.Contains("segment"))
                            {
                                name = name.TrimStart(separator);
                            }
                            else if (lname.Contains(".cfs"))
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
    }
}
