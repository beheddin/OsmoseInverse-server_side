using MediatR;
using System;

namespace Domain.Commands
{
    public class DeleteGenericCommand<T> : IRequest<string> where T : class
    {
        public  Guid Id { get; }

        public DeleteGenericCommand( Guid id)
        {
            Id = id;
        }
    }
}