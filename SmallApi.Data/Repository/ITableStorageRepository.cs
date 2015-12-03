using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using SmallApi.Data.Azure;

namespace SmallApi.Data.Repository
{
    public interface ITableStorageRepository<T> where T : AzureAdminTableEntity, new()
    {
        CloudTable GetOrCreateTable(string tableName);

        void Insert(IList<TableEntity> entities);

        void Insert(TableEntity entity);

        IEnumerable<T> List<T>(string partitionKey = null) where T : AzureAdminTableEntity, new();

        IEnumerable<T> ListByRange<T>(string rowKey, string partitionKey) where T : AzureAdminTableEntity, new();

        T Get<T>(string partitionkey, string rowkey) where T : AzureAdminTableEntity, new();
        CloudTable Table { get; }

        string Update<T>(string partitionkey, string rowkey, T newEntity) where T : AzureAdminTableEntity, new();

        string Delete<T>(string partitionkey, string rowkey) where T : AzureAdminTableEntity, new();

        void DeleteTable();
    }
}