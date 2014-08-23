
namespace MvcWebRole2
{
    using Microsoft.Web.Administration;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    public class WebRole : RoleEntryPoint
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Update this if you are running locally
        private static bool isLocal = false;
        public static string VirtualDirPath = @"C:\Users\bash\Documents\GitHub\moviemirchi\qiouwpeiqwokelwqej";

        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            string path = WebRole.VirtualDirPath;
            if (!isLocal)
            {
                logger.Info("OnStart: Calling GetVirtualDirPath");
                path = GetVirtualDirPath();
                logger.Info("OnStart: Return from GetVirtualDirPath");
                logger.Info("OnStart: Path is {0}", path);
            }

            ConfigurationManager.AppSettings["ImagePath"] = WebRole.VirtualDirPath = path;
            logger.Info("OnStart: Path is now set to {0}", path);
            logger.Info("OnStart: Reading path from config {0}", ConfigurationManager.AppSettings["ImagePath"]);

            logger.Info("OnStart: Calling WriteToConfig");
            WriteToConfig(path);
            logger.Info("OnStart: Return from WriteToConfig");

            logger.Info("OnStart: Calling SetupLuceneIndexes");
            SetupLuceneIndexes(path);
            logger.Info("OnStart: Return from SetupLuceneIndexes");

            return base.OnStart();
        }

        private static void WriteToConfig(string path)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                logger.Info("WriteToConfig: Setting ImagePath in the config to {0}", path);
                config.AppSettings.Settings["ImagePath"].Value = path;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {

                logger.Info("WriteToConfig: Exception: {0}", ex);
            }
            logger.Info("WriteToConfig: Reading path from config again {0}", ConfigurationManager.AppSettings["ImagePath"]);
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
                                 .Select(a =>
                                 {
                                     logger.Info("GetVirtualDirPath: Applications Path {0}", a.Path);
                                     return a;
                                 })
                                 //// On the Azure machine we have only one Application which is the root
                                 //// This the Where condition fails
                                 //.Where(a => a.Path.Contains(RoleEnvironment.CurrentRoleInstance.Role.Name))
                                 .SelectMany(ar => ar.VirtualDirectories
                                     .Select(vr =>
                                     {
                                         logger.Info("GetVirtualDirPath: Virtual Directory physical path {0}", vr.PhysicalPath);
                                         return vr;
                                     })
                                     .Select(vr => vr.PhysicalPath)))
                             .FirstOrDefault();

                    var result = paths ?? WebRole.VirtualDirPath;
                    logger.Info("GetVirtualDirPath: Path before return {0}", result);
                    return result;
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

                logger.Info("SetupLuceneIndexes: Path for lucene dir is {0}", luceneDir);
                logger.Info("SetupLuceneIndexes: Lucene dir exists - {0}", Directory.Exists(luceneDir));

                if (Directory.Exists(luceneDir))
                {
                    foreach (var file in Directory.EnumerateFiles(luceneDir))
                    {
                        try
                        {
                            var name = Path.GetFileName(file);
                            logger.Info("SetupLuceneIndexes: Found file {0}", name);

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
                            logger.Info("SetupLuceneIndexes: New file {0}", newFile);
                        }
                        catch (Exception ex)
                        {
                            logger.Info("SetupLuceneIndexes: Exception FileMoveLoop: {0} {1} {2}", ex.Message, ex.HResult, ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("SetupLuceneIndexes: Exception: {0}", ex);
            }
        }
    }
}
