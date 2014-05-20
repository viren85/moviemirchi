

namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;

    interface IDataStoreTableEntity : ITableEntity
    {
        string GetKey();
    }
}
