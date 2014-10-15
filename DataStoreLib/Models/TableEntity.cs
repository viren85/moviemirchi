
using DataStoreLib.Storage;

namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.StorageClient;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Script.Serialization;
    using IEntityPropertyDictionary = System.Collections.Generic.IDictionary<string, Microsoft.WindowsAzure.Storage.Table.EntityProperty>;

    public abstract class TableStorageEntity : TableEntity, IDataStoreTableEntity
    {
        #region table elements
        // none here, implements the base classs
        #endregion
        protected TableStorageEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        #region typecasts

        internal static IEntityPropertyDictionary MergeDicts(IEntityPropertyDictionary dictionary)
        {
            return dictionary == null ?
                new Dictionary<string, EntityProperty>() :
                dictionary
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);
        }

        #endregion

        #region dictionary operations

        public override int GetHashCode()
        {
            return this.RowKey.ToLower().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherEntity = obj as TableStorageEntity;
            Debug.Assert(otherEntity != null);

            return otherEntity.GetHashCode() == this.GetHashCode();
        }
        #endregion

        #region Implementation for IDataStoreTableEntity

        public virtual string GetKey()
        {
            throw new NotImplementedException("For classes inheriting from TableEntity, GetKey needs to be overridden");
        }

        #endregion
    }
}
