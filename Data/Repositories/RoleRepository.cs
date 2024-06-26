﻿using AutoMapper;
using Data.Context;
using Domain.DataTransferObjects;
using Domain.Interfaces;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly OsmoseInverseDbContext _context;
        private readonly ILogger<CompteRepository> _logger;
        private readonly IMapper _mapper;

        //dependency injection
        public RoleRepository(OsmoseInverseDbContext context, ILogger<CompteRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EntityResponseDTO<Role>> CreateRole([FromBody] RoleDTO roleDTO)
        {
            try
            {
                // Verify if the roleLabel is a valid TypeRole
                TypeRole typeRole;

                if (!Enum.TryParse(roleDTO.NomRole, true, out typeRole))
                    return new EntityResponseDTO<Role>
                    {
                        IsSuccessful = false,
                        Message = "Invalid role label",
                        Entity = null
                    };

                // Check if the role already exists in the database
                Role existingRole = await _context.Roles.FirstOrDefaultAsync(role => role.NomRole == typeRole);

                if (existingRole != null)
                    return new EntityResponseDTO<Role>
                    {
                        IsSuccessful = false,
                        Message = "Role already exists",
                        Entity = null
                    };

                // Map RoleDTO to Role
                Role role = _mapper.Map<Role>(roleDTO);

                role.NomRole = typeRole;

                return new EntityResponseDTO<Role>
                {
                    IsSuccessful = true,
                    //Message = "Compte created successfully",    //the success msg is returned by AddAsync(T entity) in GenericRepository
                    Entity = role
                };
            }
            // Handle database update errors
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to add the role due to a database update error.");
                throw new DbUpdateException($"Failed to add due to a database update error: {ex.Message}", ex);
            }
            // Handle other unexpected exceptions
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while adding the role.");
                throw new Exception($"An unexpected error occurred while adding the compte: {ex.Message}", ex);
            }
        }

        public async Task<EntityResponseDTO<Role>> UpdateRole([FromRoute]  Guid id, [FromBody] RoleDTO roleDTO)
        {
            try
            {
                // check if id is valid
                Role existingRoleById = await _context.Roles.FirstOrDefaultAsync(role => role.IdRole == id);

                if (existingRoleById == null)
                    return new EntityResponseDTO<Role>
                    {
                        IsSuccessful = false,
                        Message = "Role not found",
                        Entity = null
                    };

                // Verify if the roleLabel is a valid TypeRole
                TypeRole typeRole;

                if (!Enum.TryParse(roleDTO.NomRole, true, out typeRole))
                    return new EntityResponseDTO<Role>
                    {
                        IsSuccessful = false,
                        Message = "Invalid role label",
                        Entity = null
                    };

                // Check if the role already exists
                Role existingRoleByLabel = await _context.Roles.FirstOrDefaultAsync(role => role.NomRole == typeRole);

                if (existingRoleByLabel != null)
                    return new EntityResponseDTO<Role>
                    {
                        IsSuccessful = false,
                        Message = "Role already exists",
                        Entity = null
                    };

                // Map RoleDTO to Role
                Role role = _mapper.Map<Role>(roleDTO);

                role.IdRole = id;
                role.NomRole = typeRole;

                return new EntityResponseDTO<Role>
                {
                    IsSuccessful = true,
                    //Message = "Compte created successfully",    //the success msg is returned by AddAsync(T entity) in GenericRepository
                    Entity = role
                };
            }
            // Handle database update errors
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException: Failed to add the role due to a database update error.");
                throw new DbUpdateException($"Failed to add due to a database update error: {ex.Message}", ex);
            }
            // Handle other unexpected exceptions
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An unexpected error occurred while adding the role.");
                throw new Exception($"An unexpected error occurred while adding the compte: {ex.Message}", ex);
            }
        }
    }
}
