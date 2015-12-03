using System.Collections.Generic;

namespace SmallApi.Data.Repository
{
    public interface IRepository<T> where T : class 
    {
        void Add(T item);
        
        void Add(IEnumerable<T> items);

        void Remove(T item);
        void Update(T item);
        
        void DeleteTable();
    }
}
