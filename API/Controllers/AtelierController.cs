﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Domain.Entities;
using AutoMapper;
using Domain.Commands;
using Domain.DataTransferObjects;
using Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AtelierController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<AtelierController> _logger;

        public AtelierController(
            IMediator mediator,
            IMapper mapper,
            ILogger<AtelierController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<AtelierDTO>>> GetAteliers()
        {
            try
            {
                GetAllGenericQuery<Atelier> query = new GetAllGenericQuery<Atelier>(includes: query => query.Include(atelier => atelier.Filiale));
                IEnumerable<Atelier> ateliers = await _mediator.Send(query);

                IEnumerable<AtelierDTO> ateliersDTO = ateliers.Select(atelier => _mapper.Map<AtelierDTO>(atelier)).ToList();

                return Ok(ateliersDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetAteliers.");
                throw new Exception($"An unexpected error occurred while handling GetAteliers: {ex.Message}", ex);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<AtelierDTO>> GetAtelierById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<Atelier> query = new GetByGenericQuery<Atelier>(atelier => atelier.AtelierId == id, includes: query => query.Include(atelier => atelier.Filiale));

                Atelier atelier = await _mediator.Send(query);

                if (atelier == null)
                    return NotFound("Atelier not found");

                AtelierDTO atelierDTO = _mapper.Map<AtelierDTO>(atelier);

                return Ok(atelierDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetAtelierById.");
                throw new Exception($"An unexpected error occurred while handling GetAtelierById: {ex.Message}", ex);
            }
        }

        [HttpPost("post")]
        public async Task<ActionResult<string>> PostAtelier([FromBody] AtelierDTO atelierDTO)
        {
            try
            {
                // check if the atelier already exists based on its label
                GetByGenericQuery<Atelier> query = new GetByGenericQuery<Atelier>(atelier => atelier.AtelierLabel == atelierDTO.AtelierLabel);
                Atelier existingAtelierByLabel = await _mediator.Send(query);

                if (existingAtelierByLabel != null)
                    return Conflict($"Atelier with label '{atelierDTO.AtelierLabel}' already exists");

                //check if atelierDTO.FilialeLabel exists
                Filiale existingFilialeByLabel = await _mediator.Send(new GetByGenericQuery<Filiale>(filiale => filiale.FilialeLabel == atelierDTO.FilialeLabel));
                if (existingFilialeByLabel == null)
                    return NotFound($"Filiale with label '{atelierDTO.FilialeLabel}' not found");

                Atelier atelier = _mapper.Map<Atelier>(atelierDTO);
                
                atelier.FkFiliale = existingFilialeByLabel.FilialeId;

                PostGenericCommand<Atelier> command = new PostGenericCommand<Atelier>(atelier);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostAtelier.");
                throw new Exception($"An unexpected error occurred while handling PostAtelier: {ex.Message}", ex);
            }
        }

        [HttpPut("put/{id}")]
        public async Task<ActionResult<string>> PutAtelier([FromRoute] Guid id, [FromBody] AtelierDTO atelierDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<Atelier> queryById = new GetByGenericQuery<Atelier>(atelier => atelier.AtelierId == id);
                Atelier existingAtelierById = await _mediator.Send(queryById);

                if (existingAtelierById == null)
                    return NotFound($"Atelier with id '{id}' not found");

                // check if another atelier with the same label as atelierDTO.AtelierLabel exists
                GetByGenericQuery<Atelier> queryByIdAndLabel = new GetByGenericQuery<Atelier>(atelier =>
                    (atelier.AtelierId != id) && (atelier.AtelierLabel == atelierDTO.AtelierLabel));

                Atelier existingAtelierByIdAndLabel = await _mediator.Send(queryByIdAndLabel);

                if (existingAtelierByIdAndLabel != null)
                    return Conflict($"Atelier with label '{atelierDTO.AtelierLabel}' already exists");

                //check if atelierDTO.FilialeLabel exists
                Filiale existingFilialeByLabel = await _mediator.Send(new GetByGenericQuery<Filiale>(filiale => filiale.FilialeLabel == atelierDTO.FilialeLabel));
                if (existingFilialeByLabel == null)
                    return NotFound($"Filiale with label '{atelierDTO.FilialeLabel}' not found");

                Atelier atelier = _mapper.Map<Atelier>(atelierDTO);
                atelier.AtelierId = id;
                atelier.FkFiliale = existingFilialeByLabel.FilialeId;

                PutGenericCommand<Atelier> command = new PutGenericCommand<Atelier>(atelier);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutAtelier.");
                throw new Exception($"An unexpected error occurred while handling PutAtelier: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> DeleteAtelier([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Atelier> query = new GetByGenericQuery<Atelier>(atelier => atelier.AtelierId == id);
                Atelier atelier = await _mediator.Send(query);

                if (atelier == null)
                    return NotFound($"Atelier with id '{id}' not found");

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Atelier>(id));

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteAtelier.");
                throw new Exception($"An unexpected error occurred while handling DeleteAtelier: {ex.Message}", ex);
            }
        }
    }
}
