using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Models
{
    public class TableEntity : TableServiceEntity, ITableEntity
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

        public virtual void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            ETag = ReadString(properties, "ETag");
            Timestamp = ReadTimestamp(properties, "Timestamp");
        }

        public virtual IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(null);

            Timestamp = DateTimeOffset.Now;
            WriteTimestamp(dict, "Timestamp", Timestamp);

            return dict;
        }

        #region typecasts

        internal static string ReadString(IDictionary<string, EntityProperty> properties, string key)
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

        internal static Guid ReadGuid(IDictionary<string, EntityProperty> properties, string key)
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

        internal static bool ReadBool(IDictionary<string, EntityProperty> properties, string key)
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

        internal static int ReadInt(IDictionary<string, EntityProperty> properties, string key)
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

        internal static DateTimeOffset ReadTimestamp(IDictionary<string, EntityProperty> properties, string key)
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

        internal static void WriteString(IDictionary<string, EntityProperty> prop, string key, string val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static void WriteGuid(IDictionary<string, EntityProperty> prop, string key, Guid val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static void WriteBool(IDictionary<string, EntityProperty> prop, string key, bool val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static void WriteInt(IDictionary<string, EntityProperty> prop, string key, int val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static void WriteTimestamp(IDictionary<string, EntityProperty> prop, string key, DateTimeOffset val)
        {
            Debug.Assert(!prop.ContainsKey(key));
            prop[key] = new EntityProperty(val);
        }

        internal static IDictionary<string, EntityProperty> MergeDicts(IDictionary<string, EntityProperty> dict1)
        {
            var dict = new Dictionary<string, EntityProperty>();

            if (dict1 != null)
            {
                foreach (var key in dict1.Keys)
                {
                    dict.Add(key, dict1[key]);
                }
            }

            return dict;
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
    }
}
