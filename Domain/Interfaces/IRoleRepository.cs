﻿using Domain.DataTransferObjects;
using Domain.Models;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<EntityResponseDTO<Role>> CreateRole(RoleDTO roleDTO);
        Task<EntityResponseDTO<Role>> UpdateRole(Guid id, RoleDTO roleDTO);
    }
}
