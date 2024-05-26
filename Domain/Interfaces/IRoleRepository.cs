using Domain.DataTransferObjects;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<RepositoryResponseDTO<Role>> CreateRole(RoleDTO roleDTO);
        Task<RepositoryResponseDTO<Role>> UpdateRole(Guid id, RoleDTO roleDTO);
    }
}
