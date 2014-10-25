
namespace CloudMovie.Library
{
    using Microsoft.Web.Administration;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using TS = Microsoft.Win32.TaskScheduler;

    public static class DeploymentUtilities
    {
        // Update this for non-local runs
        private static bool isLocal = true;
        private static Dictionary<string, string> localDeploymentsPaths = new Dictionary<string, string>()
        {
            {"Web", @"C:\Users\bash\Documents\GitHub\moviemirchi\MvcWebRole1"},
            {"Editorial", @"C:\Users\bash\Documents\GitHub\moviemirchi\MvcWebRole2"},
            {"API", @"C:\Users\bash\Documents\GitHub\moviemirchi\APIRole"},
        };

        // Leave this as is - 
        public static string VirtualDirPath = @"C:\Users\bash\Documents\GitHub\moviemirchi\qiouwpeiqwokelwqej";
        //private static logger logger = LogManager.GetCurrentClassLogger();

        public static void RunSmartMonkey(string dirPath)
        {
            try
            {
                // Run SmartMonkey
                using (Process p = Process.Start(Path.Combine(dirPath, "SmartMonkey.exe")))
                {
                    p.WaitForExit(300000);
                }
            }
            catch
            {

            }
        }

        public static void ScheduleSmartMonkey(string dirPath)
        {
            using (var ts = new TS.TaskService())
            {
                string taskName = @"Run SmartMonkey";
                string exePath = Path.Combine(dirPath, "SmartMonkey.exe");
                string arguments = string.Empty;

                try
                {
                    ts.RootFolder.DeleteTask(taskName);
                }
                catch
                { }

                var td = ts.NewTask();
                td.RegistrationInfo.Description = taskName;

                var trigger = new TS.TimeTrigger();
                trigger.SetRepetition(new TimeSpan(2, 0, 0), TimeSpan.Zero);
                td.Triggers.Add(trigger);

                td.Actions.Add(new TS.ExecAction("cmd.exe", "/c \"start " + exePath + "\" " + arguments, null));

                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }

        public static string GetVirtualDirPath(string name)
        {
            if (isLocal)
            {
                return localDeploymentsPaths[name];
            }

            try
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    var paths =
                        serverManager.Sites
                            .Where(s => s.Name.EndsWith("_" + name))
                             .SelectMany(site => site.Applications
                                 .SelectMany(ar => ar.VirtualDirectories
                                     .Select(vr => vr.PhysicalPath)))
                             .FirstOrDefault();

                    var result = paths ?? VirtualDirPath;
                    // logger.Info("GetVirtualDirPath: Path before return {0}", result);
                    return result;
                }
            }
            catch { ;}

            return VirtualDirPath;
        }

        public static void SetupLuceneIndexes(string destDir)
        {
            try
            {
                var separator = new char[] { '_' };
                var luceneDir = Path.Combine(destDir, "lucene_index");

                // logger.Info("SetupLuceneIndexes: Path for lucene dir is {0}", luceneDir);
                // logger.Info("SetupLuceneIndexes: Lucene dir exists - {0}", Directory.Exists(luceneDir));

                if (Directory.Exists(luceneDir))
                {
                    foreach (var file in Directory.EnumerateFiles(luceneDir))
                    {
                        try
                        {
                            var name = Path.GetFileName(file);
                            // logger.Info("SetupLuceneIndexes: Found file {0}", name);

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
                            // logger.Info("SetupLuceneIndexes: New file {0}", newFile);
                        }
                        catch (Exception) // ex)
                        {
                            // logger.Info("SetupLuceneIndexes: Exception FileMoveLoop: {0} {1} {2}", ex.Message, ex.HResult, ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception) // ex)
            {
                // logger.Info("SetupLuceneIndexes: Exception: {0}", ex);
            }
        }

        public static void HandleLucene(string name)
        {
            if (!isLocal)
            {
                string path = VirtualDirPath;
                // logger.Info("OnStart: Calling GetVirtualDirPath");
                path = GetVirtualDirPath(name);
                // logger.Info("OnStart: Return from GetVirtualDirPath");
                // logger.Info("OnStart: Path is {0}", path);

                var luceneDir = Path.Combine(path, "lucene_index");
                foreach (var file in Directory.EnumerateFiles(luceneDir))
                {
                    GetFullAccess(file);
                }

                // logger.Info("OnStart: Calling SetupLuceneIndexes");
                SetupLuceneIndexes(path);
                // logger.Info("OnStart: Return from SetupLuceneIndexes");
            }
        }

        public static void WriteToConfig(string path, string value)
        {
            try
            {
                string filePath = Path.Combine(path, "web.config");
                GetFullAccess(filePath);

                if (!isLocal)
                {
                    var repl = "<add key=\"ImagePath\" value=\"" + value + "\" />";
                    var hit = false;

                    var lines = File.ReadAllLines(filePath);
                    var acc = lines.Aggregate<string>((a, b) =>
                    {
                        if (!hit && (b.Contains("<add key=\"ImagePath\" value=\"") && !b.Contains("<!--")))
                        {
                            hit = true;
                            return a + repl;
                        }
                        return a + b;
                    });

                    File.WriteAllText(filePath, acc);
                }
            }
            catch (Exception) // ex)
            {
                // logger.Info("WriteToConfig: Exception: {0}", ex);
            }
            // logger.Info("WriteToConfig: Reading path from config again {0}", ConfigurationManager.AppSettings["ImagePath"]);
        }

        public static void GetFullAccess(string path)
        {
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            IdentityReference act = sid.Translate(typeof(NTAccount));
            FileSecurity sec = File.GetAccessControl(path);
            sec.AddAccessRule(new FileSystemAccessRule(act, FileSystemRights.FullControl, AccessControlType.Allow));
            File.SetAccessControl(path, sec);
        }

        public static void UpdateBaseUrl(string name, string fileRelativePath)
        {
            if (!isLocal)
            {
                var path = DeploymentUtilities.GetVirtualDirPath(name);
                var filePath = Path.Combine(path, fileRelativePath);

                GetFullAccess(filePath);

                var repl = @"var BASE_URL = ""http://23.99.69.77:8081/"";";
                var start = "var BASE_URL = ";
                var hit = false;

                var lines = File.ReadAllLines(filePath);
                var acc = lines.Aggregate<string, string>("", (a, b) =>
                {
                    if (!hit && b.StartsWith(start))
                    {
                        hit = true;
                        return a + Environment.NewLine + repl;
                    }
                    return a + Environment.NewLine + b;
                });

                File.WriteAllText(filePath, acc);
            }
        }

        public static void SetupIISSettings()
        {
            if (!isLocal)
            {
                var script =
                @"cd %windir%\System32\inetsrv
%systemdrive%
cd %windir%\System32\inetsrv
appcmd.exe unlock config -section:system.webServer/httpCompression
appcmd.exe set config -section:system.webServer/httpCompression /+""dynamicTypes.[mimeType='application/json',enabled='True']"" /commit:apphost
appcmd.exe set config -section:system.webServer/httpCompression /+""dynamicTypes.[mimeType='application/json; charset=utf-8',enabled='True']"" /commit:apphost
cd ..
iisreset.exe
";
                try
                {
                    File.WriteAllText("iis.cmd", script);
                    using (Process p = Process.Start("cmd.exe", " /s iis.cmd"))
                    {
                        p.WaitForExit(300000);
                    }
                }
                catch
                {

                }
            }
        }
    }
}