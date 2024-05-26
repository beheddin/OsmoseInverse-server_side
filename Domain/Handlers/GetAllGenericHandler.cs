using MediatR;
using Domain.Interfaces;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers
{
    public class GetAllGenericHandler<T> : IRequestHandler<GetAllGenericQuery<T>, IEnumerable<T>> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly ILogger<GetAllGenericHandler<T>> _logger;

        public GetAllGenericHandler(IGenericRepository<T> repository, ILogger<GetAllGenericHandler<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //sync
        //public Task<IEnumerable<T>> Handle(GetAllGenericQuery<T> request, CancellationToken cancellationToken)
        //{
        //    var result = _repository.GetAll(request.Condition, request.Includes);
        //    return Task.FromResult(result);
        //}

        //async
        public async Task<IEnumerable<T>> Handle(GetAllGenericQuery<T> request, CancellationToken cancellationToken)
        {
            try
            {
                return await _repository.GetAllAsync(request.Condition, request.Includes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while handling GetAllGenericHandler.");
                throw new Exception($"An unexpected error occurred while handling GetAllGenericHandler: {ex.Message}", ex);
            }
        }
    }
}