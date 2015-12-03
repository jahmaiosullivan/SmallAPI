using System.Collections.Generic;

namespace SmallApi.Data.Azure
{
    public interface IQueueRepository<T> where T : class, new()
    {
        string QueueName { get; }
        void Insert(T item);
        void Insert(IEnumerable<T> items);
        IList<T> GetAll();
        T Get();
        void Delete(string messageId, string popreceipt);
        int Count();
        IList<T> GetAll(int count);
    }
}