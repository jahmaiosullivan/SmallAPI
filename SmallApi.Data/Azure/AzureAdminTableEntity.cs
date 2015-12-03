using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SmallApi.Data.Azure
{
    public class AzureAdminTableEntity : TableEntity
    {
        public AzureAdminTableEntity()
        {
            TableName = GetType().Name;
        }
        [ExcludeFromAzureTable]
        public string TableName { get; set; }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var entityProperties = base.WriteEntity(operationContext);
            var objectProperties = GetType().GetProperties();

            foreach (var property in from property in objectProperties
                                     let nonSerializedAttributes = property.GetCustomAttributes(typeof(ExcludeFromAzureTableAttribute), false)
                                     where nonSerializedAttributes.Length > 0
                                     select property)
            {
                entityProperties.Remove(property.Name);
            }

            return entityProperties;
        }
    }
}
