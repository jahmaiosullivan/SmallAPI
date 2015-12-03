using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.WindowsAzure.Storage.Table;

namespace SmallApi.Data.Azure
{
    public class AzureTableService<T> : IAzureTableService<T> where T : TableEntity
    {
        protected readonly ITableRepository<T> Repository;

        public AzureTableService(ITableRepository<T> repository)
        {
            Repository = repository;
        }

        public virtual bool Add(T entity)
        {
            var item = Get(entity.PartitionKey,entity.RowKey);
            if (item != null) return false;
                Repository.Add(entity);
            return true;
        }

        public virtual void Delete(T entity)
        {
            var item = Get(entity.PartitionKey, entity.RowKey);
            if (item != null)
                Repository.Remove(item);
        }


        public virtual void Update(T entity)
        {
            Repository.Update(entity);
        }

        public virtual T Get(string partitionkey, string rowkey)
        {
            return Repository.FindById(partitionkey, rowkey);
        }

        public virtual T Get(string rowkey)
        {
            return Repository.Find(rowkey);
        }

        public virtual IEnumerable<T> All(string partitionKey = null)
        {
            return Repository.FindAll(partitionKey);
        }

        public void DeleteTable()
        {
            Repository.DeleteTable();
        }
    }
}
