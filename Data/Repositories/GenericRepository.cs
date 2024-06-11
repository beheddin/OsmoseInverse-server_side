using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Data.Context;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Polly;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly OsmoseInverseDbContext _context;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(OsmoseInverseDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Async functions

        //GET
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                if (includes != null)
                    query = includes(query);

                if (condition != null)
                    return await query.Where(condition).ToListAsync();

                return await query.ToListAsync();
            }
            // Handle database update errors
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to retrieve entities due to a database update error.");
                throw new InvalidOperationException($"Failed to execute the query. Please check your query condition or includes: {ex.Message}", ex);
            }
            // Handle other unexpected exceptions
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while retrieving entities.");
                throw new Exception($"An unexpected error occurred while retrieving entities: {ex.Message}", ex);
            }
        }

        ////public virtual async Task<T> GetByAsync( Guid id)
        //public async Task<T> GetByAsync( Guid id)
        //{
        //    try
        //    {
        //        return await _context.Set<T>().FirstOrDefaultAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        //public virtual async Task<T> GetAsync(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        public async Task<T> GetByAsync(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                if (includes != null)
                    query = includes(query);

                if (condition != null)
                    // return the entity matching the condition
                    //return await query.FirstOrDefaultAsync(condition);
                    return await query.SingleOrDefaultAsync(condition);


                // If no condition provided, retrieve the single entity
                return await query.SingleOrDefaultAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "InvalidOperationException: Failed to execute the query. Please check your query condition or includes.");
                throw new InvalidOperationException($"Failed to execute the query. Please check your query condition or includes: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while retrieving the entity");
                throw new Exception($"An unexpected error occurred while retrieving the entity: {ex.Message}", ex);
            }
        }

        /**/

        //ADD
        public async Task<string> AddAsync(T entity)
        {
            try
            {
                // Add the entity to the context
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();

                return "Added successfully";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to add the entity due to a database update error.");
                throw new DbUpdateException($"Failed to add due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while adding the entity.");
                throw new Exception($"An unexpected error occurred while adding the entity: {ex.Message}", ex);
            }
        }

        /**/

        //UPDATE
        public async Task<string> UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;    // Mark the entity as modified
                await _context.SaveChangesAsync(); // Save changes to the database

                return "Updated successfully";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to update  the entity due to a database update error.");
                throw new Exception($"Failed to update due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while updating the entity.");
                throw new Exception($"An unexpected error occurred while updating the entity: {ex.Message}", ex);
            }
        }

        /**/

        //REMOVE
        //public async Task<string> RemoveAsync( Guid id)
        public async Task<string> RemoveAsync(Guid id)
        {
            try
            {
                T entity = await _context.Set<T>().FindAsync(id);

                // Check if the entity with the provided ID exists
                if (entity == null)
                {
                    return $"Failed to delete: Entity with ID {id} not found";
                }

                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();

                return "Deleted successfully";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to delete the entity due to a database update error.");
                throw new Exception($"Failed to delete due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while deleting the entity.");
                throw new Exception($"An unexpected error occurred while deleting the entity: {ex.Message}", ex);
            }
        }

        #region Not implemented
        public IEnumerable<T> GetByIDAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        //public async Task<string> RemoveObjectAsync(T entity)
        //{
        //    string response;

        //    try
        //    {
        //        _context.Set<T>().Remove(entity);
        //        await _context.SaveChangesAsync();
        //        response = "Deleted successfully";

        //    }
        //    catch (Exception e)
        //    {
        //        response = "Failed to delete\n" + e.ToString();
        //    }

        //    return response;
        //}

        public virtual IEnumerable<T> ExecuteStoreQueryAsync(string commandText, params object[] parameters)
        {
            throw new NotImplementedException();
            //    try
            //    {
            //        return await _context.Set<T>().FromSql(commandText, parameters).ToListAsync();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //        throw;
            //    }
        }

        public virtual IEnumerable<T> ExecuteStoreQueryAsync(string commandText, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            //try
            //{
            //    return await _context.Set<T>().FromSql(commandText, includes).ToListAsync();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Sync functions

        //GET
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                if (includes != null)
                    query = includes(query);


                if (condition != null)
                    return query.Where(condition).ToList();


                return query.ToList();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to retrieve entities due to a database update error.");
                throw new InvalidOperationException($"Failed to execute the query. Please check your query condition or includes: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while retrieving entities.");
                throw new Exception($"An unexpected error occurred while retrieving entities: {ex.Message}", ex);
            }
        }

        //public T GetBy( Guid id)
        //{
        //    try
        //    {
        //        return _context.Set<T>().FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        /**/

        //GETBY
        public T GetBy(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                // Apply includes if provided
                if (includes != null)
                    query = includes(query);


                // Apply condition if provided
                if (condition != null)
                {
                    // Retrieve the entity matching the condition
                    T entity = query.FirstOrDefault(condition);

                    // Check if the entity exists
                    if (entity == null)
                        throw new Exception($"Entity of type {typeof(T).Name} with the provided condition was not found");

                    return entity;
                }

                // If no condition provided, retrieve the first entity
                return query.FirstOrDefault();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "InvalidOperationException: Failed to execute the query. Please check your query condition or includes.");
                throw new InvalidOperationException($"Failed to execute the query. Please check your query condition or includes: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while retrieving the entity");
                throw new Exception($"An unexpected error occurred while retrieving the entity: {ex.Message}", ex);
            }
        }

        /**/

        //ADD
        public string Add(T entity)
        {
            try
            {
                // Add the entity to the context
                _context.Set<T>().Add(entity);
                _context.SaveChanges();

                return "Added successfully";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to add the entity due to a database update error.");
                throw new DbUpdateException($"Failed to add due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while adding the entity.");
                throw new Exception($"An unexpected error occurred while adding the entity: {ex.Message}", ex);
            }
        }

        /**/

        //UPDATE
        public string Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;    // Mark the entity as modified
                _context.SaveChanges(); // Save changes to the database

                return "Updated successfully";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to update  the entity due to a database update error.");
                throw new Exception($"Failed to update due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while updating the entity.");
                throw new Exception($"An unexpected error occurred while updating the entity: {ex.Message}", ex);
            }
        }

        /**/

        //REMOVE
        public string Remove(Guid id)
        {
            try
            {
                T entity = _context.Set<T>().Find(id);

                // Check if the entity with the provided ID exists
                if (entity == null)
                {
                    return $"Failed to delete: Entity with ID {id} not found";
                }

                _context.Set<T>().Remove(entity);
                _context.SaveChanges();

                return "Deleted successfully";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to delete the entity due to a database update error.");
                throw new Exception($"Failed to delete due to a database update error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while deleting the entity.");
                throw new Exception($"An unexpected error occurred while deleting the entity: {ex.Message}", ex);
            }
        }

        #region Not implemented

        public IEnumerable<T> GetByID(Guid id)
        {
            throw new NotImplementedException();
        }

        //public string RemoveObject(T entity)
        //{
        //    string response;

        //    try
        //    {
        //        _context.Set<T>().Remove(entity);
        //        _context.SaveChanges();
        //        return "delete done";
        //    }
        //    catch (Exception e)
        //    {
        //        response = "Failed to delete\n" + e.ToString();
        //    }

        //    return response;
        //}

        public IEnumerable<T> ExecuteStoreQuery(string commandText, params object[] parameters)
        {
            //try
            //{
            //    return _context.Set<T>().FromSql(commandText, parameters).ToList();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

            throw new NotImplementedException();
        }

        public IEnumerable<T> ExecuteStoreQuery(string commandText, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            //try
            //{
            //    return _context.Set<T>().FromSql(commandText, includes).ToList();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

            throw new NotImplementedException();
        }
        #endregion

        #endregion
    }
}