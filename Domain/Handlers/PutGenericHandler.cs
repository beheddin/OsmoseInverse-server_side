using MediatR;
using Domain.Commands;
using Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers
{
    public class PutGenericHandler<T> : IRequestHandler<PutGenericCommand<T>, string> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly ILogger<PutGenericCommand<T>> _logger;

        public PutGenericHandler(IGenericRepository<T> repository, ILogger<PutGenericCommand<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> Handle(PutGenericCommand<T> request, CancellationToken cancellationToken)
        {
            try
            {
                return await _repository.UpdateAsync(request.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while handling PutGenericHandler.");
                throw new Exception($"An unexpected error occurred while handling PutGenericHandler: {ex.Message}", ex);
            }
        }
    }
}