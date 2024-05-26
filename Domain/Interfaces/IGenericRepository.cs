using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        #region Async functions
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        
        //Task<T> GetByAsync(Guid id);
        Task<T> GetByAsync(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        
        //Task<string> AddAsync(T entity);
        Task<string> AddAsync(T entity);
        //Task AddRangeAsync(IEnumerable<T> entities);

        //Task<string> UpdateAsync(T entity);
        Task<string> UpdateAsync(T entity);

        //Task<string> RemoveAsync(Guid id);
        Task<string> RemoveAsync(Guid id);

        //Task<string> RemoveObjectAsync(T entity);

        #region Not implemented
        IEnumerable<T> GetByIDAsync(Guid id);
        IEnumerable<T> ExecuteStoreQueryAsync(String commandText, params object[] parameters);
        IEnumerable<T> ExecuteStoreQueryAsync(String commandText, Func<IQueryable<T>,
            IIncludableQueryable<T, object>> includes = null);
        #endregion

        #endregion

        #region Sync functions
        IEnumerable<T> GetAll(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        //T GetBy(Guid id);
        T GetBy(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        string Add(T entity);
        string Update(T entity);
        string Remove(Guid id);
        //string RemoveObject(T entity);

        #region Not implemented
        IEnumerable<T> GetByID(Guid id);

        IEnumerable<T> ExecuteStoreQuery(String commandText, params object[] parameters);

        IEnumerable<T> ExecuteStoreQuery(String commandText,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        #endregion

        #endregion
    }
}