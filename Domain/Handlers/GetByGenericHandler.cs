using MediatR;
using Domain.Interfaces;
using Domain.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers
{
    public class GetByGenericHandler<T> : IRequestHandler<GetByGenericQuery<T>, T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly ILogger<GetByGenericHandler<T>> _logger;

        public GetByGenericHandler(IGenericRepository<T> repository, ILogger<GetByGenericHandler<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<T> Handle(GetByGenericQuery<T> request, CancellationToken cancellationToken)
        {
            try
            {
                T entity = await _repository.GetByAsync(request.Condition, request.Includes);

                // Return the entity or default(T) if it's null
                return entity ?? default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while handling GetByGenericHandler.");
                throw new Exception($"An unexpected error occurred while handling GetByGenericHandler: {ex.Message}", ex);
            }
        }
    }
}