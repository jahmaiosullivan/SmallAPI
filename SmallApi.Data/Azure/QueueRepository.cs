using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace SmallApi.Data.Azure
{
    public abstract class QueueRepository<T> : IQueueRepository<T> where T : QueueItem, new()
    {
        private readonly ICloudClientWrapper cloudClientWrapper;

        protected QueueRepository(ICloudClientWrapper cloudClientWrapper)
        {
            this.cloudClientWrapper = cloudClientWrapper;
        }


        public abstract string QueueName { get; }

        public void Insert(T item)
        {
            item.SubmittedDate = DateTime.UtcNow;
            var itemJsonString = JsonConvert.SerializeObject(item);
            var message = new CloudQueueMessage(itemJsonString);
            Queue.AddMessage(message);
        }

        public void Insert(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Insert(item);
            }
        }

        public IList<T> GetAll(int count)
        { 
            IList<T> results = new List<T>();
            if (count <= 0) return results;

            const int azureMaxPeekMessages = 32; //max number of messages to peek at is 32

            if (count > azureMaxPeekMessages) count = azureMaxPeekMessages;
            var messages = Queue.PeekMessages(count);
            foreach (var m in messages)
            {
                var deserializedMessage = JsonConvert.DeserializeObject<T>(m.AsString);
                deserializedMessage.PopReceipt = m.PopReceipt;
                deserializedMessage.Id = m.Id;
                deserializedMessage.QueueMessageStored = m.AsString;
                deserializedMessage.SubmittedDate = m.InsertionTime?.LocalDateTime ?? DateTime.MinValue;
                results.Add(deserializedMessage);
            }
            return results;
        }


        public IList<T> GetAll()
        {
            var count = Count();
            return GetAll(count);
        }

        public T Get()
        {
            var message = Queue.GetMessage();
            return message != null ? JsonConvert.DeserializeObject<T>(message.AsString) : null;
        }

        public void Delete(string messageId, string popreceipt)
        {
            try
            {
                Queue.DeleteMessage(messageId, popreceipt);
            }
            catch (StorageException ex)
            {
                if (ex.Message == "Message Not Found")
                {
                    // pop receipt must be invalid
                    // ignore or log (so we can tune the visibility timeout)
                }
                else
                {
                    // not the error we were expectisng
                    throw;
                }
            }
        }


        public int Count()
        {
            Queue.FetchAttributes();
            return Queue.ApproximateMessageCount ?? -1;
        }
        
        
        private CloudQueue queue;
        CloudQueue Queue
        {
            get
            {
                if (queue != null) return queue;

                queue = cloudClientWrapper.QueueClient.GetQueueReference(QueueName.ToLower().Trim());
                queue.CreateIfNotExists();
                return queue;
            }
        }
    }


}
