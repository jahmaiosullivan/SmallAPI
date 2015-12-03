using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SmallApi.Data.Repository;

namespace SmallApi.Data.Azure
{
    public interface ITableRepository<T> :  IRepository<T> where T : class
    {
        T FindById(string partitionkey, string rowkey);
        T Find(string rowkey);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAll(string partitionKey = null);

    }
}