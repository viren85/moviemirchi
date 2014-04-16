
namespace DataStoreLib.Utils
{
    public class ConnectionSettingsSingleton
    {
        private static ConnectionSettingsSingleton _instance;
        private static object lockbj = new object();

        private ConnectionSettingsSingleton()
        {
        }

        public static ConnectionSettingsSingleton Instance
        {
            get
            {
                lock (lockbj)
                {
                    if (_instance == null)
                    {
                        _instance = new ConnectionSettingsSingleton();
                    }

                    return _instance;
                }
            }
        }

        public string StorageConnectionString
        {
            get;
            set;
        }
    }
}
