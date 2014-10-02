﻿
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
        // Update this for local runs
        private static bool isLocal = false;
        private static Dictionary<string, string> localDeploymentsPaths = new Dictionary<string, string>()
        {
            {"Web", @"C:\Users\bash\Documents\GitHub\moviemirchi\MvcWebRole1"},
            {"Editorial", @"C:\Users\bash\Documents\GitHub\moviemirchi\MvcWebRole2"},
            {"API", @"C:\Users\bash\Documents\GitHub\moviemirchi\APIRole"},
        };

        // Leave this as is - 
        public static string VirtualDirPath = @"C:\Users\bash\Documents\GitHub\moviemirchi\qiouwpeiqwokelwqej";
        //private static logger logger = LogManager.GetCurrentClassLogger();

        public static void RunSmartMonkey()
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
                             .SelectMany(site => site.Applications
                                 .Select(a =>
                                 {
                                     // logger.Info("GetVirtualDirPath: Applications Path {0}", a.Path);
                                     return a;
                                 })
                                 //// On the Azure machine we have only one Application which is the root
                                 //// This the Where condition fails
                                 .Where(a => a.Path.Contains(name))
                                 .SelectMany(ar => ar.VirtualDirectories
                                     .Select(vr =>
                                     {
                                         // logger.Info("GetVirtualDirPath: Virtual Directory physical path {0}", vr.PhysicalPath);
                                         return vr;
                                     })
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

                ConfigurationManager.AppSettings["ImagePath"] = VirtualDirPath = path;
                // logger.Info("OnStart: Path is now set to {0}", path);
                // logger.Info("OnStart: Reading path from config {0}", ConfigurationManager.AppSettings["ImagePath"]);

                // logger.Info("OnStart: Calling WriteToConfig");
                WriteToConfig(path, path);
                // logger.Info("OnStart: Return from WriteToConfig");

                // logger.Info("OnStart: Calling SetupLuceneIndexes");
                SetupLuceneIndexes(path);
                // logger.Info("OnStart: Return from SetupLuceneIndexes");
            }
        }

        public static void WriteToConfig(string path, string dir)
        {
            try
            {
                GetFullAccess(Path.Combine(dir, "web.config"));

                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // logger.Info("WriteToConfig: Setting ImagePath in the config to {0}", path);
                config.AppSettings.Settings["ImagePath"].Value = path;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
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
    }
}