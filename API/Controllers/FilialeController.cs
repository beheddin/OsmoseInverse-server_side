using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Domain.Entities;
using Domain.DataTransferObjects;
using Domain.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Domain.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilialeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<FilialeController> _logger;

        public FilialeController(
            IMediator mediator,
            IMapper mapper,
            ILogger<FilialeController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<FilialeDTO>>> GetFiliales()
        {
            try
            {
                GetAllGenericQuery<Filiale> query = new GetAllGenericQuery<Filiale>();
                IEnumerable<Filiale> filiales = await _mediator.Send(query);

                IEnumerable<FilialeDTO> filialesDTO = filiales.Select(filiale => _mapper.Map<FilialeDTO>(filiale)).ToList();

                return Ok(filialesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetFiliales.");
                throw new Exception($"An unexpected error occurred while handling GetFiliales: {ex.Message}", ex);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<FilialeDTO>> GetFilialeById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<Filiale> query = new GetByGenericQuery<Filiale>(filiale => filiale.FilialeId == id);

                Filiale filiale = await _mediator.Send(query);

                if (filiale == null)
                    return NotFound("Filiale not found");

                FilialeDTO filialeDTO = _mapper.Map<FilialeDTO>(filiale);

                return Ok(filialeDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetFilialeById.");
                throw new Exception($"An unexpected error occurred while handling GetFilialeById: {ex.Message}", ex);
            }
        }

        [HttpPost("post")]
        public async Task<ActionResult<string>> PostFiliale([FromBody] FilialeDTO filialeDTO)
        {
            try
            {
                // check if the filiale already exists based on its label
                GetByGenericQuery<Filiale> query = new GetByGenericQuery<Filiale>(filiale => filiale.FilialeLabel == filialeDTO.FilialeLabel);
                Filiale existingFilialeByLabel = await _mediator.Send(query);

                if (existingFilialeByLabel != null)
                    return Conflict($"Filiale with label '{filialeDTO.FilialeLabel}' already exists");

                Filiale filiale = _mapper.Map<Filiale>(filialeDTO);

                PostGenericCommand<Filiale> command = new PostGenericCommand<Filiale>(filiale);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostFiliale.");
                throw new Exception($"An unexpected error occurred while handling PostFiliale: {ex.Message}", ex);
            }
        }

        [HttpPut("put/{id}")]
        public async Task<ActionResult<string>> PutFiliale([FromRoute] Guid id, [FromBody] FilialeDTO filialeDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<Filiale> queryById = new GetByGenericQuery<Filiale>(filiale => filiale.FilialeId == id);
                Filiale existingFilialeById = await _mediator.Send(queryById);

                if (existingFilialeById == null)
                    return NotFound($"Filiale with id '{id}' not found");

                // check if another filiale with the same label exists
                GetByGenericQuery<Filiale> queryByLabel = new GetByGenericQuery<Filiale>(
                    filiale => filiale.FilialeLabel == filialeDTO.FilialeLabel && filiale.FilialeId != id);
                Filiale existingFilialeByLabel = await _mediator.Send(queryByLabel);

                if (existingFilialeByLabel != null)
                    return Conflict($"Filiale with label '{filialeDTO.FilialeLabel}' already exists");

                Filiale filiale = _mapper.Map<Filiale>(filialeDTO);
                
                filiale.FilialeId = id;

                PutGenericCommand<Filiale> command = new PutGenericCommand<Filiale>(filiale);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutFiliale.");
                throw new Exception($"An unexpected error occurred while handling PutFiliale: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> DeleteFiliale([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Filiale> query = new GetByGenericQuery<Filiale>(filiale => filiale.FilialeId == id);
                Filiale filiale = await _mediator.Send(query);

                if (filiale == null)
                    return NotFound($"Filiale with id '{id}' not found");

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Filiale>(id));

                return Ok(mediatorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeleteFiliale.");
                throw new Exception($"An unexpected error occurred while handling DeleteFiliale: {ex.Message}", ex);
            }
        }
    }
}
