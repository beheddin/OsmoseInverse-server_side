using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Domain.DataTransferObjects
{
    public class RepositoryResponseDTO<T>
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public T Entity { get; set; }
        public string Token { get; set; }
    }
}
