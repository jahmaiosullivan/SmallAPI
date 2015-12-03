using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace SmallApi.Data.Azure
{
    public interface IAzureTableService<T> where T : TableEntity
    {
        T Get(string partitionkey, string rowkey);

        T Get(string rowkey);
        bool Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IEnumerable<T> All(string partitionKey = null);
        void DeleteTable();
    }
}