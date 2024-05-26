using MediatR;
using Domain.Commands;
using Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers
{
    public class PostGenericHandler<T> : IRequestHandler<PostGenericCommand<T>, string> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly ILogger<PostGenericHandler<T>> _logger;

        public PostGenericHandler(IGenericRepository<T> repository, ILogger<PostGenericHandler<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> Handle(PostGenericCommand<T> request, CancellationToken cancellationToken)
        {
            try
            {
                return await _repository.AddAsync(request.Entity);  //repo response type is a string
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while handling PostGenericHandler.");
                throw new Exception($"An unexpected error occurred while handling PostGenericHandler: {ex.Message}", ex);
            }
        }
    }
}