using MediatR;

namespace Domain.Commands
{
    public class PostGenericCommand<T> : IRequest<string> where T : class
    {
        public T Entity { get; }

        public PostGenericCommand(T entity)
        {
            Entity = entity;
        }
    }
}