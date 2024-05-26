/*
using MediatR;
using Domain.Commands;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class DeleteObjectHandler<T> : IRequestHandler<DeleteObjectCommand<T>, string> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public DeleteObjectHandler(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        //sync
        //public Task<string> Handle(DeleteObject<T> request, CancellationToken cancellationToken)
        //{
        //    var result = _repository.Removeobject(request.Entity);
        //    return Task.FromResult(result);
        //}

        //async
        public async Task<string> Handle(DeleteObjectCommand<T> request, CancellationToken cancellationToken)
        {
            return await _repository.RemoveObjectAsync(request.Entity);
        }
    }
}
*/