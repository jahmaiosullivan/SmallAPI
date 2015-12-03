using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.WindowsAzure.Storage.Table;
using SmallApi.Data.Repository;

namespace SmallApi.Data.Azure
{
    public class TableRepository<T> : ITableRepository<T> where T : AzureAdminTableEntity, new()
    {
        private readonly ITableStorageRepository<T> tableStorageRepository;

        public TableRepository(ITableStorageRepository<T> tableStorageRepository)
        {
            this.tableStorageRepository = tableStorageRepository;
        }

        public void Add(T item)
        {
            tableStorageRepository.Insert(item);
        }

        public void Add(IEnumerable<T> items)
        {
            tableStorageRepository.Insert((IList<TableEntity>) items.ToList());
        }

        public void Remove(T item)
        {
            tableStorageRepository.Delete<T>(item.PartitionKey,item.RowKey);
        }

        public void Update(T item)
        {
            tableStorageRepository.Update(item.PartitionKey, item.RowKey, item);
        }

        public T FindById(string partitionkey, string rowkey)
        {
            return tableStorageRepository.Get<T>(partitionkey, rowkey);
        }
        
        public T Find(string key)
        {
            T item = new T { RowKey = key };
             return FindById(item.PartitionKey, item.RowKey);
        }
        
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindAll(string partitionKey = null)
        {
            return tableStorageRepository.List<T>(partitionKey);
        }

        public void DeleteTable()
        {
            tableStorageRepository.DeleteTable();
        }
    }
}
