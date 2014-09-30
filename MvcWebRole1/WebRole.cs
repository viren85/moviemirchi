
namespace MvcWebRole1
{
    using TS = Microsoft.Win32.TaskScheduler;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    public class WebRole : RoleEntryPoint
    {

        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            var result = base.OnStart();

            //// TODO: Figure out install path - refactor the helpers from different webrole.cs files
            //var installDirPath = "";

            //// TODO: Schedule smart monkey
            //Task.Run(() =>
            //{
            //    ScheduleSmartMonkey(installDirPath);
            //});

            Task.Run(() =>
            {
                RunSmartMonkey();
            });

            return result;
        }

        private static void ScheduleSmartMonkey(string dirPath)
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
