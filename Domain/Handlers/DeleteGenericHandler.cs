using MediatR;
using Domain.Commands;
using Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers
{
    public class DeleteGenericHandler<T> : IRequestHandler<DeleteGenericCommand<T>, string> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly ILogger<DeleteGenericCommand<T>> _logger;

        public DeleteGenericHandler(IGenericRepository<T> repository, ILogger<DeleteGenericCommand<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> Handle(DeleteGenericCommand<T> request, CancellationToken cancellationToken)
        {
            try
            {
                return await _repository.RemoveAsync(request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while handling DeleteGenericHandler.");
                throw new Exception($"An unexpected error occurred while handling DeleteGenericHandler: {ex.Message}", ex);
            }
        }
    }
}