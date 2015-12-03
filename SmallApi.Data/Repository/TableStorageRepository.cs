using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using SmallApi.Data.Azure;

namespace SmallApi.Data.Repository
{
    public class TableStorageRepository<T> : ITableStorageRepository<T> where T : AzureAdminTableEntity, new()
    {
        private readonly ICloudClientWrapper cloudClientWrapper;

        public TableStorageRepository(ICloudClientWrapper cloudClientWrapper)
        {
            this.cloudClientWrapper = cloudClientWrapper;
        }
        
        public CloudTable Table
        {
            get
            {
                var typeParam = (AzureAdminTableEntity)Activator.CreateInstance(typeof(T));
                return GetOrCreateTable(typeParam.TableName);
            }
        }
        
        public CloudTable GetOrCreateTable(string tableName)
        {
            var table = cloudClientWrapper.TableClient.GetTableReference(tableName);
            try
            {
                table.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                if (ex.Message == "The remote server returned an error: (409) Conflict.")
                    return GetOrCreateTable(tableName);
                throw;
            }
            return table;
        }

        public void Insert(TableEntity entity)
        {
            TableOperation insertOperation = TableOperation.Insert(entity);
            Table.Execute(insertOperation);
        }

        public void Insert(IList<TableEntity> entities)
        {
            if(entities.Count > 100)
                throw new Exception("Can only insert up to 100 items");

            var batchOperation = new TableBatchOperation();
            foreach (var entity in entities)
            {
                batchOperation.Insert(entity);
            }
            Table.ExecuteBatch(batchOperation);
        }

        public IEnumerable<T> List<T>(string partitionKey = null) where T : AzureAdminTableEntity, new()
        {
            var query = new TableQuery<T>();
            if(partitionKey != null)
                query = query.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return ListExecution(query);
        }



        public IEnumerable<T> ListByRange<T>(string rowKey, string partitionKey) where T : AzureAdminTableEntity, new()
        {
            var rangeQuery = new TableQuery<T>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, rowKey)));
            return ListExecution(rangeQuery);
        }

        private IEnumerable<T> ListExecution<T>(TableQuery<T> query) where T : AzureAdminTableEntity, new()
        {
            var entities = Table.ExecuteQuery(query);
            return entities;
        }

        public T Get<T>(string partitionkey, string rowkey) where T : AzureAdminTableEntity, new()
        {
            // Create a retrieve operation that takes a customer entity.
            var retrieveOperation = TableOperation.Retrieve<T>(partitionkey, rowkey);

            // Execute the retrieve operation.
            var retrievedResult = Table.Execute(retrieveOperation);

            return (T) retrievedResult.Result;
        }

        public string Update<T>(string partitionkey, string rowkey, T newEntity) where T : AzureAdminTableEntity, new()
        {
            // Assign the result to a CustomerEntity object.
            var updateEntity = Get<T>(partitionkey, rowkey);

            if (updateEntity != null)
            {
                // Change something.
                updateEntity = newEntity;

                // Create the InsertOrReplace TableOperation
                var insertOrReplaceOperation = TableOperation.InsertOrReplace(updateEntity);

                // Execute the operation.
                Table.Execute(insertOrReplaceOperation);

                return "Entity updated.";
            }

            return "Entity could not be retrieved.";
        }

        public string Delete<T>(string partitionkey, string rowkey) where T : AzureAdminTableEntity, new()
        {
            var retrieveOperation = TableOperation.Retrieve<T>(partitionkey, rowkey);
            var retrievedResult = Table.Execute(retrieveOperation);
            var deleteEntity = (T)retrievedResult.Result;

            if (deleteEntity != null)
            {
                var deleteOperation = TableOperation.Delete(deleteEntity);
                Table.Execute(deleteOperation);
                return "Entity deleted.";
            }

            return "Could not retrieve the entity.";
        }

        public void DeleteTable()
        {
            Table.DeleteIfExists();
        }
    }
}