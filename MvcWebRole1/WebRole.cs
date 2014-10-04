
namespace MvcWebRole1
{
    using CloudMovie.Library;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class WebRole : RoleEntryPoint
    {

        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            // Replace lucene path in web.config
            var path = DeploymentUtilities.GetVirtualDirPath("API");
            DeploymentUtilities.WriteToConfig(path, path);

            // Replace base url
            DeploymentUtilities.UpdateBaseUrl("Web", @"Content\movie.core.js");
            DeploymentUtilities.UpdateBaseUrl("Editorial", @"Content\controls\mm.admin.core.js");
            DeploymentUtilities.UpdateBaseUrl("Editorial", @"Content\movie.autocomplete.js");

            // Lucene setup
            DeploymentUtilities.HandleLucene("API");

            // Schedule monkey
            Task.Run(() =>
            {
                var installDirPath = DeploymentUtilities.GetVirtualDirPath("Web");
                DeploymentUtilities.ScheduleSmartMonkey(installDirPath);
            });

            // Run monkey
            Task.Run(() =>
            {
                DeploymentUtilities.RunSmartMonkey();
            });

            // Mark as start
            var result = base.OnStart();
            return result;
        }
    }
}
