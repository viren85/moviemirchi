
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Utils;
    using Microsoft.WindowsAzure;
    using System.Diagnostics;
    using System.Web.Http;

    public abstract class BaseController : ApiController
    {
        public string Get()
        {
            SetConnectionString();
            return ProcessRequest();
        }

        protected abstract string ProcessRequest();
        
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
    }
}
