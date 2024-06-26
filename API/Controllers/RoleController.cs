﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Domain.DataTransferObjects;
using Domain.Models;
using Domain.Queries;
using Domain.Interfaces;
using Domain.Commands;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _repository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            IRoleRepository repository,
            IMediator mediator,
            IMapper mapper,
            ILogger<RoleController> logger
            )
        {
            _repository = repository;
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        /**/

        [HttpGet("get/all")]
        //public async Task<IEnumerable<RoleDTO>> GetRoles()
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            try
            {
                GetAllGenericQuery<Role> query = new GetAllGenericQuery<Role>();
                IEnumerable<Role> roles = await _mediator.Send(query);

                if (roles.IsNullOrEmpty())
                    return NotFound(new { message = "Roles not found" });

                // Map Role entities to RoleDTO using AutoMapper
                //var rolesDTO = roles.Select(role => _mapper.Map<RoleDTO>(role)).ToList();
                IEnumerable<RoleDTO> rolesDTO = roles.Select(role => _mapper.Map<RoleDTO>(role)).ToList();

                return Ok(rolesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetRoles.");
                throw new Exception($"An unexpected error occurred while handling GetRoles: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/{id}")]
        //public async Task<RoleDTO> GetRoleById( Guid id)
        public async Task<ActionResult<RoleDTO>> GetRoleById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<Role> query = new GetByGenericQuery<Role>(role => role.IdRole == id);
                Role role = await _mediator.Send(query);

                if (role == null)
                    return NotFound(new { message = $"Role with ID '{id}' not found" });

                RoleDTO roleDTO = _mapper.Map<RoleDTO>(role);

                return Ok(roleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetRoleById.");
                throw new Exception($"An unexpected error occurred while handling GetRoleById: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("post")]
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //default method
        //public async Task<ActionResult<Role>> PostRole(Role role)
        //{
        //    _context.Role.Add(role);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRole", new { id = role.IdRole }, role);
        //}

        //public async Task<string> PostRole([FromBody] RoleDTO roleDTO)
        //public async Task<ActionResult> PostRole([FromBody] RoleDTO roleDTO)
        //public async Task<ActionResult<string>> PostRole([FromBody] RoleDTO roleDTO)
        public async Task<ActionResult<MessageResponseDTO>> PostRole([FromBody] RoleDTO roleDTO)
        {
            try
            {
                EntityResponseDTO<Role> response = await _repository.CreateRole(roleDTO);

                if (!response.IsSuccessful)
                    switch (response.Message)
                    {
                        case "Invalid role label":
                            //return NotFound($"Role with label '{roleDTO.NomRole}' is invalid");
                            return BadRequest(new MessageResponseDTO { Message = response.Message });

                        case "Role already exists":
                            //return Conflict($"Role with label '{roleDTO.NomRole}' already exists");
                            return Conflict(new MessageResponseDTO { Message = response.Message });

                        default:
                            //return StatusCode(500, response.Message);
                            return StatusCode(500, new MessageResponseDTO { Message = "An error occurred while processing your request" });
                    }

                PostGenericCommand<Role> command = new PostGenericCommand<Role>(response.Entity);
                string mediatorResponse = await _mediator.Send(command);

                //return Ok(mediatorResponse);
                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostRole.");
                throw new Exception($"An unexpected error occurred while handling PostRole: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPut("put/{id}")]
        //public async Task<string> PutRole( Guid id, [FromBody] RoleDTO roleDTO)
        //public async Task<ActionResult<string>> PutRole([FromRoute] Guid id, [FromBody] RoleDTO roleDTO)
        public async Task<ActionResult<MessageResponseDTO>> PutRole([FromRoute] Guid id, [FromBody] RoleDTO roleDTO)
        {
            try
            {
                EntityResponseDTO<Role> response = await _repository.UpdateRole(id, roleDTO);

                if (!response.IsSuccessful)

                    return response.Message switch
                    {
                        "Role not found" =>
                        //NotFound($"Role with id '{id}' not found"),
                        NotFound(new MessageResponseDTO { Message = response.Message }),

                        "Invalid role label" =>
                        //NotFound($"Role with label '{roleDTO.NomRole}' is invalid"),
                        BadRequest(new MessageResponseDTO { Message = response.Message }),

                        "Role already exists" => Conflict($"Role with label '{roleDTO.NomRole}' already exists"),

                        _ => StatusCode(500, new MessageResponseDTO { Message = "An error occurred while processing your request" }),
                    };

                PutGenericCommand<Role> command = new PutGenericCommand<Role>(response.Entity);
                string mediatorResponse = await _mediator.Send(command);

                //return Ok(mediatorResponse);
                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutRole.");
                throw new Exception($"An unexpected error occurred while handling PutRole: {ex.Message}", ex);
            }
        }

        /**/

        [HttpDelete("delete/{id}")]
        //public async Task<string> DeleteRole( Guid id)
        public async Task<ActionResult<MessageResponseDTO>> DeleteRole([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Role> query = new GetByGenericQuery<Role>(role => role.IdRole == id);
                Role role = await _mediator.Send(query);

                if (role == null)
                    //return NotFound($"Role with id '{id}' don't exist");
                    return NotFound(new MessageResponseDTO { Message = $"Role with ID '{id}' not found" });

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Role>(id));

                //return Ok(mediatorResponse);
                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteRole.");
                throw new Exception($"An unexpected error occurred while handling DeleteRole: {ex.Message}", ex);
            }
        }
    }
}
