
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

            var result = base.OnStart();

            // WEB role
            {
                // DeploymentUtilities.UpdateBaseUrl("Web", @"Content\movie.core.js");


                //Task.Run(() =>
                //{
                //    var installDirPath = DeploymentUtilities.GetVirtualDirPath("Web");
                //    DeploymentUtilities.ScheduleSmartMonkey(installDirPath);
                //});

                Task.Run(() =>
                {
                    DeploymentUtilities.RunSmartMonkey();
                });
            }

            // Editorial role
            {
                // DeploymentUtilities.UpdateBaseUrl("Editorial", @"Content\controls\mm.admin.core.js");
                // DeploymentUtilities.UpdateBaseUrl("Editorial", @"Content\movie.autocomplete.js");
            }

            // API role
            {
                // Lucene setup
                // DeploymentUtilities.HandleLucene("API");
            }

            return result;
        }
    }
}
