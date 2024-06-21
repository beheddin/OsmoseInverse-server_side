using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Domain.Models;
using AutoMapper;
using Domain.Commands;
using Domain.DataTransferObjects;
using Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PuitController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<PuitController> _logger;

        public PuitController(
            IMediator mediator,
            IMapper mapper,
            ILogger<PuitController> logger
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<PuitDTO>>> GetPuits()
        {
            try
            {
                GetAllGenericQuery<Puit> query = new GetAllGenericQuery<Puit>(includes: query => query.Include(puit => puit.Filiale));
                IEnumerable<Puit> puits = await _mediator.Send(query);

                if (puits.IsNullOrEmpty())
                    return NotFound(new { message = "No Puits were found" });

                IEnumerable<PuitDTO> puitsDTO = puits.Select(puit => _mapper.Map<PuitDTO>(puit)).ToList();

                return Ok(puitsDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetPuits.");
                throw new Exception($"An unexpected error occurred while handling GetPuits: {ex.Message}", ex);
            }
        }

        /**/

        [HttpGet("get/{id}")]
        public async Task<ActionResult<PuitDTO>> GetPuitById([FromRoute] Guid id)
        {
            try
            {
                GetByGenericQuery<Puit> query = new GetByGenericQuery<Puit>(puit => puit.IdSourceEau == id, includes: query => query.Include(puit => puit.Filiale));
                Puit puit = await _mediator.Send(query);

                if (puit == null)
                    return NotFound(new { message = $"Puit with ID '{id}' not found" });

                PuitDTO puitDTO = _mapper.Map<PuitDTO>(puit);

                return Ok(puitDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling GetPuitById.");
                throw new Exception($"An unexpected error occurred while handling GetPuitById: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPost("post")]
        public async Task<ActionResult<MessageResponseDTO>> PostPuit([FromBody] PuitDTO puitDTO)
        {
            try
            {
                //check if puitDTO.NomPuit already exists
                GetByGenericQuery<Puit> query = new GetByGenericQuery<Puit>(puit => puit.NomPuit == puitDTO.NomPuit);
                Puit existingPuitByNom = await _mediator.Send(query);

                if (existingPuitByNom != null)
                    return Conflict(new MessageResponseDTO { Message = $"Puit with Nom '{puitDTO.NomPuit}' already exists" });

                //check if puitDTO.NomFiliale exists
                Filiale existingFilialeByNom = await _mediator.Send(new GetByGenericQuery<Filiale>(filiale => filiale.NomFiliale == puitDTO.NomFiliale));
                if (existingFilialeByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Filiale with Nom '{puitDTO.NomFiliale}' not found" });

                Puit puit = _mapper.Map<Puit>(puitDTO);

                puit.FkFiliale = existingFilialeByNom.IdFiliale;

                PostGenericCommand<Puit> command = new PostGenericCommand<Puit>(puit);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PostPuit.");
                throw new Exception($"An unexpected error occurred while handling PostPuit: {ex.Message}", ex);
            }
        }

        /**/

        [HttpPut("put/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> PutPuit([FromRoute] Guid id, [FromBody] PuitDTO puitDTO)
        {
            try
            {
                // check if id is valid
                GetByGenericQuery<Puit> query = new GetByGenericQuery<Puit>(puit => puit.IdSourceEau == id);
                Puit existingPuitById = await _mediator.Send(query);

                if (existingPuitById == null)
                    return NotFound(new MessageResponseDTO { Message = $"Puit with ID '{id}' not found" });

                // check if another puit with the same Nom exists
                GetByGenericQuery<Puit> queryByLabel = new GetByGenericQuery<Puit>(
                    puit => puit.NomPuit == puitDTO.NomPuit && puit.IdSourceEau != id);
                Puit existingPuitByLabel = await _mediator.Send(queryByLabel);

                if (existingPuitByLabel != null)
                    return Conflict(new MessageResponseDTO { Message = $"Puit with Nom '{puitDTO.NomPuit}' already exists" });

                //check if puitDTO.NomFiliale exists
                Filiale existingFilialeByNom = await _mediator.Send(new GetByGenericQuery<Filiale>(filiale => filiale.NomFiliale == puitDTO.NomFiliale));
                if (existingFilialeByNom == null)
                    return NotFound(new MessageResponseDTO { Message = $"Filiale with Nom '{puitDTO.NomFiliale}' not found" });

                Puit puit = _mapper.Map<Puit>(puitDTO);

                puit.IdSourceEau = id;
                puit.FkFiliale = existingFilialeByNom.IdFiliale;

                PutGenericCommand<Puit> command = new PutGenericCommand<Puit>(puit);
                string mediatorResponse = await _mediator.Send(command);

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling PutPuit.");
                throw new Exception($"An unexpected error occurred while handling PutPuit: {ex.Message}", ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<MessageResponseDTO>> DeletePuit([FromRoute] Guid id)
        {
            try
            {
                //check if id is valid
                GetByGenericQuery<Puit> query = new GetByGenericQuery<Puit>(puit => puit.IdSourceEau == id);
                Puit puit = await _mediator.Send(query);

                if (puit == null)
                    return NotFound(new MessageResponseDTO { Message = $"Puit with ID '{id}' not found" });

                string mediatorResponse = await _mediator.Send(new DeleteGenericCommand<Puit>(id));

                return Ok(new MessageResponseDTO { IsSuccessful = true, Message = mediatorResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: An error occurred while handling DeletePuit.");
                throw new Exception($"An unexpected error occurred while handling DeletePuit: {ex.Message}", ex);
            }
        }
    }
}
