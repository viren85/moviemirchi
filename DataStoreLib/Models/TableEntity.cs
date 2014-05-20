
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.StorageClient;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using IEntityPropertyDictionary = System.Collections.Generic.IDictionary<string, Microsoft.WindowsAzure.Storage.Table.EntityProperty>;

    public abstract class TableEntity : TableServiceEntity, IDataStoreTableEntity
    {
        #region table elements
        // none here, implements the base classs
        #endregion
        protected TableEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public string ETag { get; set; }
        public new DateTimeOffset Timestamp { get; set; }

        public virtual void ReadEntity(IEntityPropertyDictionary properties, OperationContext operationContext)
        {
            ETag = ReadString(properties, "ETag");
            Timestamp = ReadTimestamp(properties, "Timestamp");
        }

        public virtual IEntityPropertyDictionary WriteEntity(OperationContext operationContext)
        {
            var dict = MergeDicts(null);

            Timestamp = DateTimeOffset.Now;
            WriteTimestamp(dict, "Timestamp", Timestamp);

            return dict;
        }

        #region typecasts

        internal static string ReadString(IEntityPropertyDictionary properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                return properties[key].StringValue;
            }
            else
            {
                return null;
            }
        }

        internal static Guid ReadGuid(IEntityPropertyDictionary properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                var val = properties[key].GuidValue;
                if (!val.HasValue)
                {
                    return Guid.Empty;
                }
                else
                {
                    return val.Value;
                }
            }
            else
            {
                return Guid.Empty;
            }
        }

        internal static bool ReadBool(IEntityPropertyDictionary properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                var val = properties[key].BooleanValue;
                if (!val.HasValue)
                {
                    return false;
                }
                else
                {
                    return val.Value;
                }
            }
            else
            {
                return false;
            }
        }

        internal static int ReadInt(IEntityPropertyDictionary properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                var val = properties[key].Int32Value;
                if (!val.HasValue)
                {
                    return 0;
                }
                else
                {
                    return val.Value;
                }
            }
            else
            {
                return 0;
            }
        }

        internal static DateTimeOffset ReadTimestamp(IEntityPropertyDictionary properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                var val = properties[key].DateTimeOffsetValue;
                if (!val.HasValue)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return val.Value;
                }
            }
            else
            {
                //return DateTime.MinValue;
                return DateTime.Now;
            }
        }

        internal static void WriteString(IEntityPropertyDictionary prop, string key, string val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static void WriteGuid(IEntityPropertyDictionary prop, string key, Guid val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static void WriteBool(IEntityPropertyDictionary prop, string key, bool val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static void WriteInt(IEntityPropertyDictionary prop, string key, int val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static void WriteTimestamp(IEntityPropertyDictionary prop, string key, DateTimeOffset val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

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
            var otherEntity = obj as TableEntity;
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
